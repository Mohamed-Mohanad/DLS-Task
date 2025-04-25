import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ProductService } from '../../../product/services/product.service';
import { CategoryService } from '../../../product/services/category.service';
import { Product, Category } from '../../../core/models/product.model';
import { PaginatedResponse } from '../../../core/models/api-response.model';

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
    MatSelectModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule
  ]
})
export class ProductFormComponent implements OnInit {
  productForm!: FormGroup;
  categories: Category[] = [];
  isEdit = false;
  formSubmitted = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private categoryService: CategoryService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product?: Product }
  ) { }

  ngOnInit(): void {
    this.loadCategories();
    this.initForm();

    if (this.data.product) {
      this.isEdit = true;
      this.populateForm(this.data.product);
    }
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (response: PaginatedResponse<Category>) => {
        this.categories = response.value;
      },
      error: (error) => {
        this.snackBar.open('Error loading categories', 'Close', { duration: 3000 });
      }
    });
  }

  initForm(): void {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required]],
      price: [0, [Validators.required, Validators.min(0.01)]],
      categoryId: [null, [Validators.required]]
    });
  }

  populateForm(product: Product): void {
    this.productForm.patchValue({
      name: product.name,
      description: product.description,
      price: product.price,
      categoryId: product.categoryId
    });
  }

  onSubmit(): void {
    this.formSubmitted = true;

    if (this.productForm.invalid) {
      return;
    }

    if (this.isEdit) {
      const updateData = {
        id: this.data.product!.id,
        ...this.productForm.value
      };

      this.productService.updateProduct( updateData).subscribe({
        next: (success) => {
          if (success) {
            this.snackBar.open('Product updated successfully', 'Close', { duration: 3000 });
            this.dialogRef.close(true);
          } else {
            this.snackBar.open('Failed to update product', 'Close', { duration: 3000 });
          }
        },
        error: (error) => {
          this.snackBar.open('Error updating product', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.productService.createProduct(this.productForm.value).subscribe({
        next: (productId) => {
          if (productId) {
            this.snackBar.open('Product created successfully', 'Close', { duration: 3000 });
            this.dialogRef.close(true);
          } else {
            this.snackBar.open('Failed to create product', 'Close', { duration: 3000 });
          }
        },
        error: (error) => {
          this.snackBar.open('Error creating product', 'Close', { duration: 3000 });
        }
      });
    }
  }

  // Helper for form control access
  get f() {
    return this.productForm.controls;
  }
} 