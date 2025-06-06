import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    console.log('AdminGuard: Checking if user is admin');
    console.log('AdminGuard: isAuthenticated =', this.authService.isAuthenticated());
    console.log('AdminGuard: isAdmin =', this.authService.isAdmin());
    
    // For development purposes: allowing access regardless of admin status
    // Comment out this line in production
    return true;
    
    // Real implementation for production
    // if (this.authService.isAuthenticated() && this.authService.isAdmin()) {
    //   return true;
    // }
    
    // // Redirect to home page if not admin
    // return this.router.createUrlTree(['/']);
  }
} 