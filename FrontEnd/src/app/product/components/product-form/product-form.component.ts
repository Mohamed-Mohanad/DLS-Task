import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../services/category.service';
import { CommonModule } from '@angular/common';
import { Product, Category } from '../../../core/models/product.model';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiResponse } from '../../../core/models/api-response.model';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ]
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  isEdit = false;
  isDialogMode = false;
  categories: Category[] = [];
  isLoading = false;
  productId?: number;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private categoryService: CategoryService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    @Optional() @Inject(MatDialogRef) private dialogRef: MatDialogRef<ProductFormComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Product,
    private authService: AuthService
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      price: [0, [Validators.required, Validators.min(0)]],
      categoryId: ['', [Validators.required]],
      imageUrl: ['', [Validators.required]],
      stock: [0, [Validators.required, Validators.min(0)]]
    });
    
    this.isDialogMode = !!this.dialogRef;
  }

  ngOnInit(): void {
    this.loadCategories();
    
    // Check if we're in edit mode from route params
    this.route.params.subscribe(params => {
      const id = params['id'];
      console.log(id);
      
      if (id) {
        this.productId = +id;
        this.isEdit = true;
        this.loadProductDetails(this.productId);
      }
    });
    
    // Also support dialog mode for backward compatibility
    if (this.data) {
      this.isEdit = true;
      this.productForm.patchValue(this.data);
    }
  }
  
  loadProductDetails(id: number): void {
    this.isLoading = true;
    this.productService.getProduct(id).subscribe({
      next: (response: ApiResponse<Product>) => {
        if (response && response.isSuccess) {
          console.log('Product details loaded:', response.value);
          this.productForm.patchValue(response.value);
          this.isLoading = false;
        } else {
          this.snackBar.open('Error loading product details', 'Close', { duration: 3000 });
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.error('Error loading product details:', error);
        this.snackBar.open('Error loading product details', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }
  
  loadCategories(): void {
    this.isLoading = true;
    this.categoryService.getCategories(1, 10).subscribe({
      next: (response) => {
        this.categories = response.value;
        this.isLoading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading categories', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const productData = {...this.productForm.value, id: this.productId};
      
      if (this.isEdit) {
        const id = this.productId || (this.data?.id);
        if (!id) {
          this.snackBar.open('Error: Product ID not found', 'Close', { duration: 3000 });
          return;
        }
        
        this.productService.updateProduct(  productData).subscribe({
          next: () => {
            this.snackBar.open('Product updated successfully', 'Close', { duration: 3000 });
            this.handleSuccess();
          },
          error: (error) => {
            this.snackBar.open('Error updating product', 'Close', { duration: 3000 });
          }
        });
      } else {
        this.productService.createProduct(productData).subscribe({
          next: () => {
            this.snackBar.open('Product created successfully', 'Close', { duration: 3000 });
            this.handleSuccess();
          },
          error: (error) => {
            this.snackBar.open('Error creating product', 'Close', { duration: 3000 });
          }
        });
      }
    }
  }
  
  private handleSuccess(): void {
    if (this.isDialogMode) {
      this.dialogRef.close(true);
    } else {
      const isAdmin = this.authService.isAdmin();
      if (isAdmin) {
        this.router.navigate(['/admin/products']);
      } else {
        this.router.navigate(['/']);
      }
    }
  }

  onCancel(): void {
    if (this.isDialogMode) {
      this.dialogRef.close();
    } else {
      this.router.navigate(['/']);
    }
  }
} 