import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap, map, catchError } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';
import { throwError } from 'rxjs';

export interface CartResponse {
    id?: number;
    userId?: number;
    totalPrice: number;
    itemCount: number;
    items: CartItemResponse[];
    isSuccess?: boolean;
}

export interface CartItemResponse {
    id: number;
    productId: number;
    productName: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
}

@Injectable({
    providedIn: 'root'
})
export class CartService extends ApiService {
    private cartEndpoint = 'cart';
    private _cart = new BehaviorSubject<CartResponse | null>(null);

    public cart$ = this._cart.asObservable();

    constructor(http: HttpClient) {
        super(http);
    }

    getCart(): Observable<CartResponse> {
        console.log('CartService: Getting cart');
        return this.http.get<any>(`${this.baseUrl}/api/${this.apiVersion}/${this.cartEndpoint}`)
            .pipe(
                tap(response => {
                    console.log('Cart API response:', response);
                    if (response.isSuccess) {
                        this._cart.next(response.value || response);
                    } else {
                        console.error('Cart API error:', response);
                    }
                }),
                map(response => response.value || response),
                catchError(error => {
                    console.error('Cart API error:', error);
                    return throwError(() => error);
                })
            );
    }

    addToCart(userId: number, productId: number, quantity: number = 1): Observable<boolean> {
        return this.post<boolean>(`${this.cartEndpoint}/add`, {
            userId,
            productId,
            quantity
        }).pipe(
            tap(() => this.getCart().subscribe())
        );
    }

    removeFromCart(userId: number, productId: number): Observable<boolean> {
        return this.post<boolean>(`${this.cartEndpoint}/remove`, {
            userId,
            productId
        }).pipe(
            tap(() => this.getCart().subscribe())
        );
    }

    isProductInCart(productId: number): boolean {
        const cart = this._cart.value;
        if (!cart) return false;

        return cart.items.some(item => item.productId === productId);
    }
} 