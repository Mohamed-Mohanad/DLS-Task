import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { AuthService } from './core/services/auth.service';
import { CartService } from './cart/services/cart.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'DLS';
  isAuthenticated$: Observable<boolean>;
  cartItemCount$: Observable<number>;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private router: Router
  ) {
    this.isAuthenticated$ = this.authService.currentUser$.pipe(
      map(user => !!user)
    );

    this.cartItemCount$ = this.cartService.cart$.pipe(
      map(cart => cart?.itemCount || 0)
    );
  }

  ngOnInit(): void {
    // Ensure current user is loaded on app initialization if token exists
    if (this.authService.isAuthenticated() && !this.authService.isUserLoaded) {
      console.log('App: Loading current user on initialization');
      this.authService.getCurrentUser().subscribe(user => {
        console.log('App: User loaded on initialization:', user);

        // Load cart after user is authenticated
        this.loadCart();
      });
    }
  }

  loadCart(): void {
    if (this.authService.isAuthenticated()) {
      this.cartService.getCart().subscribe({
        error: (error) => {
          console.error('Error loading cart', error);
        }
      });
    }
  }

  navigateToCart(): void {
    console.log('Navigating to cart...');

    // Force load cart data before navigation
    if (this.authService.isAuthenticated()) {
      this.cartService.getCart().subscribe({
        next: (cart) => {
          console.log('Cart data loaded:', cart);
          this.router.navigate(['/cart']);
        },
        error: (error) => {
          console.error('Error loading cart data:', error);
          // Navigate anyway to show empty cart state
          this.router.navigate(['/cart']);
        }
      });
    } else {
      // If not authenticated, navigate to login
      this.router.navigate(['/auth/login']);
    }
  }

  logout(): void {
    this.authService.logout();
  }
}
