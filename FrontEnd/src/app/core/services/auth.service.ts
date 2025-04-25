import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, of, map, catchError, switchMap } from 'rxjs';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { User, LoginRequest, RegisterRequest } from '../models/user.model';
import { Router } from '@angular/router';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends ApiService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  public isUserLoaded = false;

  constructor(http: HttpClient, private router: Router) {
    super(http);
    this.loadStoredUser();
  }

  private loadStoredUser(): void {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    if (token && user) {
      try {
        const parsedUser = JSON.parse(user);
        this.currentUserSubject.next(parsedUser);
        this.isUserLoaded = true;
        console.log('Loaded stored user:', parsedUser);
      } catch (error) {
        console.error('Error parsing stored user:', error);
        localStorage.removeItem('user');
      }
    }
  }

  login(credentials: LoginRequest): Observable<User | null> {
    console.log('AuthService: Attempting login');
    return this.post<string>('authentication/login', credentials)
      .pipe(
        tap(token => {
          console.log('AuthService: Login successful, storing token');
          localStorage.setItem('token', token);
        }),
        switchMap(() => this.getCurrentUser()),
        tap(user => {
          if (user) {
            console.log('AuthService: Login complete, user loaded:', user);
            // Navigate to appropriate route based on role
            if (user.role === 'Admin') {
              this.router.navigate(['/admin/products']);
            } else {
              this.router.navigate(['/']);
            }
          } else {
            console.error('Failed to retrieve user after login');
          }
        }),
        catchError(error => {
          console.error('AuthService: Login error:', error);
          this.logout();
          return of(null);
        })
      );
  }

  register(userData: RegisterRequest): Observable<User | null> {
    return this.post<string>('authentication/register', userData)
      .pipe(
        tap(token => {
          console.log('AuthService: Registration successful, storing token');
          localStorage.setItem('token', token);
        }),
        switchMap(() => this.getCurrentUser()),
        tap(user => {
          if (user) {
            console.log('AuthService: Registration complete, user loaded:', user);
            this.router.navigate(['/']);
          }
        }),
        catchError(error => {
          console.error('AuthService: Registration error:', error);
          return of(null);
        })
      );
  }

  getCurrentUser(): Observable<User | null> {
    if (!this.getToken()) {
      console.log('AuthService: No token found, cannot get current user');
      return of(null);
    }

    console.log('AuthService: Fetching current user');
    return this.http.get<ApiResponse<User>>(`${this.baseUrl}/api/${this.apiVersion}/authentication/get-current-user`)
      .pipe(
        tap(response => {
          if (response.isSuccess) {
            const user = response.value;
            console.log('Retrieved current user:', user);
            localStorage.setItem('user', JSON.stringify(user));
            this.currentUserSubject.next(user);
            this.isUserLoaded = true;
          } else {
            console.error('Error getting current user:', response.error);
          }
        }),
        map(response => response.isSuccess ? response.value : null),
        catchError(error => {
          console.error('Error getting current user:', error);
          return of(null);
        })
      );
  }

  logout(): void {
    console.log('AuthService: Logging out user');
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.isUserLoaded = false;
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    // Check if the user is authenticated and has the admin role
    const user = this.currentUserSubject.value;
    return this.isAuthenticated() && user?.role === 'Admin';
  }

  getCurrentUserId(): number {
    const user = this.currentUserSubject.value;
    return user?.id || 0;
  }
} 