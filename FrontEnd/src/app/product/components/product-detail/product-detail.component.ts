import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { CartService } from '../../../core/services/cart.service';
import { Product } from '../../../core/models/product.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    RouterModule,
    MatProgressSpinnerModule
  ]
})
export class ProductDetailComponent implements OnInit {
  product: Product | null = null;
  quantity = 1;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(): void {
    this.isLoading = true;
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id) {
      this.productService.getProduct(id).subscribe({
        next: (response) => {
          this.product = response.value;
          this.isLoading = false;
        },
        error: (error) => {
          this.snackBar.open('Error loading product', 'Close', { duration: 3000 });
          this.isLoading = false;
        }
      });
    }
  }

  addToCart(): void {
    if (this.product && this.quantity > 0) {
      this.cartService.addToCart(this.product.id, this.quantity).subscribe({
        next: () => {
          this.snackBar.open(`${this.quantity} item(s) added to cart`, 'Close', { duration: 3000 });
        },
        error: (error) => {
          this.snackBar.open('Error adding to cart', 'Close', { duration: 3000 });
        }
      });
    }
  }

  updateQuantity(value: number): void {
    this.quantity = Math.max(1, this.quantity + value);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
} 