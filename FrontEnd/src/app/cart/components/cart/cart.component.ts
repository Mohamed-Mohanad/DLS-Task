import { Component, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CartService, CartResponse, CartItemResponse } from '../../services/cart.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatCardModule,
    MatSnackBarModule,
    CurrencyPipe
  ]
})
export class CartComponent implements OnInit {
  cart: CartResponse | null = null;
  displayedColumns: string[] = ['productName', 'quantity', 'unitPrice', 'totalPrice', 'actions'];
  isLoading = false;

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    console.log('CartComponent initialized');
    this.loadCart();
  }

  loadCart(): void {
    this.isLoading = true;
    console.log('CartComponent: Loading cart...');
    this.cartService.getCart().subscribe({
      next: (cart) => {
        console.log('CartComponent: Cart loaded successfully:', cart);
        if (!cart.items) {
          console.warn('CartComponent: No items array in cart response');
        }
        this.cart = cart;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('CartComponent: Error loading cart:', error);
        this.snackBar.open('Error loading cart: ' + (error.message || 'Unknown error'), 'Close', { duration: 5000 });
        this.isLoading = false;
      }
    });
  }

  updateQuantity(item: CartItemResponse, newQuantity: number): void {
    console.log(`CartComponent: Updating quantity for item ${item.productId} to ${newQuantity}`);
    // Remove item if quantity is less than 1
    if (newQuantity < 1) {
      this.removeFromCart(item.productId);
      return;
    }

    const userId = this.authService.getCurrentUserId();
    // Calculate quantity difference
    const quantityDiff = newQuantity - item.quantity;

    // Add to cart with the difference in quantity
    this.cartService.addToCart(userId, item.productId, quantityDiff).subscribe({
      next: () => {
        console.log('CartComponent: Quantity updated successfully');
        this.snackBar.open('Cart updated successfully', 'Close', { duration: 3000 });
        this.loadCart(); // Reload cart after update
      },
      error: (error) => {
        console.error('CartComponent: Error updating quantity:', error);
        this.snackBar.open('Error updating cart: ' + (error.message || 'Unknown error'), 'Close', { duration: 3000 });
      }
    });
  }

  removeFromCart(productId: number): void {
    console.log(`CartComponent: Removing item ${productId} from cart`);
    const userId = this.authService.getCurrentUserId();
    this.cartService.removeFromCart(userId, productId).subscribe({
      next: () => {
        console.log('CartComponent: Item removed successfully');
        this.snackBar.open('Item removed from cart', 'Close', { duration: 3000 });
        this.loadCart(); // Reload cart after removal
      },
      error: (error) => {
        console.error('CartComponent: Error removing item:', error);
        this.snackBar.open('Error removing item: ' + (error.message || 'Unknown error'), 'Close', { duration: 3000 });
      }
    });
  }

  checkout(): void {
    console.log('CartComponent: Checkout initiated');
    // Implement checkout logic or navigation here
    this.snackBar.open('Checkout feature coming soon!', 'Close', { duration: 3000 });
  }
} 