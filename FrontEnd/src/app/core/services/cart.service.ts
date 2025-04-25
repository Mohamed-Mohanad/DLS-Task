import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { Cart, AddToCartRequest, RemoveFromCartRequest } from '../models/cart.model';

@Injectable({
  providedIn: 'root'
})
export class CartService extends ApiService {
  private cartSubject = new BehaviorSubject<Cart | null>(null);
  public cart$ = this.cartSubject.asObservable();

  constructor(http: HttpClient) {
    super(http);
  }

  getCart(): Observable<Cart> {
    return this.get<Cart>('cart')
      .pipe(
        tap(cart => {
          this.cartSubject.next(cart);
        })
      );
  }

  addToCart(productId: number, quantity: number): Observable<boolean> {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const request: AddToCartRequest = {
      userId: user.id,
      productId,
      quantity
    };
    
    return this.post<boolean>('cart/add', request)
      .pipe(
        tap(() => {
          this.getCart().subscribe();
        })
      );
  }

  removeFromCart(productId: number): Observable<boolean> {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const request: RemoveFromCartRequest = {
      userId: user.id,
      productId
    };
    
    return this.post<boolean>('cart/remove', request)
      .pipe(
        tap(() => {
          this.getCart().subscribe();
        })
      );
  }

  clearCart(): void {
    this.cartSubject.next(null);
  }
} 