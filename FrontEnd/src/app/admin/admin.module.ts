import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { ProductManagementComponent } from './components/product-management/product-management.component';
import { ProductFormComponent } from './components/product-form/product-form.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { CategoryManagementComponent } from './components/category-management/category-management.component';
import { CategoryFormComponent } from './components/category-form/category-form.component';

// Updated routes for easier debugging
const routes: Routes = [
    {
        path: '',
        component: AdminDashboardComponent,
        children: [
            { path: '', redirectTo: 'products', pathMatch: 'full' },
            { path: 'products', component: ProductManagementComponent },
            { path: 'categories', component: CategoryManagementComponent }
        ]
    },
    // Direct routes for easier testing
    { path: 'products-direct', component: ProductManagementComponent },
    { path: 'categories-direct', component: CategoryManagementComponent }
];

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ProductManagementComponent,
        ProductFormComponent,
        AdminDashboardComponent,
        CategoryManagementComponent,
        CategoryFormComponent,
        MatButtonModule,
        MatCardModule,
        MatDialogModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatPaginatorModule,
        MatSelectModule,
        MatSnackBarModule,
        MatSortModule,
        MatTableModule,
        MatTabsModule,
        RouterModule.forChild(routes)
    ]
})
export class AdminModule {
    constructor() {
        console.log('AdminModule initialized');
    }
} 