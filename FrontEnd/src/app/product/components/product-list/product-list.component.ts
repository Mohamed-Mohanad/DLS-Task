import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { Router, RouterModule } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../services/category.service';
import { CartService } from '../../../cart/services/cart.service';
import { Product, Category } from '../../../core/models/product.model';
import { AuthService } from '../../../core/services/auth.service';
import { ProductFilterComponent } from '../product-filter/product-filter.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatSnackBarModule,
    CurrencyPipe,
    ProductFilterComponent,
    RouterModule
  ]
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  categories: Category[] = [];
  isLoading = false;
  totalItems = 0;
  pageSize = 10;
  currentPage = 1;
  selectedCategory: string = '';
  dataSource = new MatTableDataSource<Product>([]);
  displayedColumns: string[] = ['name', 'category', 'price', 'stock', 'actions'];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private cartService: CartService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
    this.loadCart();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  loadProducts(categoryId?: number): void {
    this.isLoading = true;
    this.productService.getProducts(this.currentPage, this.pageSize, categoryId).subscribe({
      next: (response) => {
        this.products = response.value;
        this.dataSource.data = this.products;
        this.totalItems = response.totalItems;
        this.isLoading = false;
      },
      error: (error) => {
        this.snackBar.open('Error loading products', 'Close', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories = response.value;
      },
      error: (error) => {
        this.snackBar.open('Error loading categories', 'Close', { duration: 3000 });
      }
    });
  }

  loadCart(): void {
    if (this.isAuthenticated()) {
      this.cartService.getCart().subscribe({
        error: (error) => {
          console.error('Error loading cart', error);
        }
      });
    }
  }

  onFilterChange(filters: { categoryId?: number }): void {
    this.currentPage = 1;
    if (this.paginator) {
      this.paginator.firstPage();
    }
    this.loadProducts(filters.categoryId);
  }

  onPageChange(event: any): void {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadProducts();
  }

  addToCart(product: Product, event: Event): void {
    event.stopPropagation(); // Prevent row click event

    if (!this.isAuthenticated()) {
      this.snackBar.open('Please log in to add items to your cart', 'Login', {
        duration: 3000
      }).onAction().subscribe(() => {
        this.router.navigate(['/auth/login']);
      });
      return;
    }

    const userId = this.authService.getCurrentUserId();

    this.cartService.addToCart(userId, product.id, 1).subscribe({
      next: () => {
        this.snackBar.open('Product added to cart', 'View Cart', {
          duration: 3000
        }).onAction().subscribe(() => {
          this.router.navigate(['/cart']);
        });
      },
      error: (error) => {
        this.snackBar.open('Error adding product to cart', 'Close', { duration: 3000 });
      }
    });
  }

  removeFromCart(product: Product, event: Event): void {
    event.stopPropagation(); // Prevent row click event

    const userId = this.authService.getCurrentUserId();

    this.cartService.removeFromCart(userId, product.id).subscribe({
      next: () => {
        this.snackBar.open('Product removed from cart', 'Close', { duration: 3000 });
      },
      error: (error) => {
        this.snackBar.open('Error removing product from cart', 'Close', { duration: 3000 });
      }
    });
  }

  isProductInCart(productId: number): boolean {
    return this.cartService.isProductInCart(productId);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  filterByCategory(): void {
    if (this.selectedCategory) {
      this.dataSource.filter = this.selectedCategory.trim().toLowerCase();
    } else {
      this.dataSource.filter = '';
    }

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openProductForm(product?: Product): void {
    this.router.navigate(['/add-product']);
  }

  editProduct(product: Product, event: Event): void {
    event.stopPropagation(); // Prevent row click event
    this.router.navigate(['/edit-product', product.id]);
  }

  deleteProduct(productId: number, event: Event): void {
    event.stopPropagation(); // Prevent row click event
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(productId).subscribe({
        next: () => {
          this.snackBar.open('Product deleted successfully', 'Close', { duration: 3000 });
          this.loadProducts();
        },
        error: (error) => {
          this.snackBar.open('Error deleting product', 'Close', { duration: 3000 });
        }
      });
    }
  }

  viewProductDetails(product: Product): void {
    this.router.navigate(['/products', product.id]);
  }
}
