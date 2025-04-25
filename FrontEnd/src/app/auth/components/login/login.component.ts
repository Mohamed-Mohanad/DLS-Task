import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,RouterModule
  ]
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  formSubmitted = false;
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    this.formSubmitted = true;
    
    if (this.loginForm.invalid) {
      return;
    }

    this.isLoading = true;
    
    const credentials = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password
    };

    console.log('Login: Submitting credentials');
    this.authService.login(credentials).subscribe({
      next: (user) => {
        console.log('Login: Login successful', user);
        // Navigation is handled in the auth service based on user role
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Login: Login failed', error);
        this.snackBar.open('Login failed. Please check your credentials and try again.', 'Close', { duration: 5000 });
        this.isLoading = false;
      }
    });
  }

  get f() {
    return this.loginForm.controls;
  }
} 