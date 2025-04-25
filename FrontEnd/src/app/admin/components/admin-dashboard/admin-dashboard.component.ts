import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTabsModule
  ]
})
export class AdminDashboardComponent {
  navLinks = [
    { path: '/admin/products', label: 'Products' },
    { path: '/admin/categories', label: 'Categories' }
  ];
} 