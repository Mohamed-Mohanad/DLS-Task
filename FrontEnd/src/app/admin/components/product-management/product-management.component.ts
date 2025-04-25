import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ProductService } from '../../../product/services/product.service';
import { CategoryService } from '../../../product/services/category.service';
import { Product } from '../../../core/models/product.model';
import { ProductFormComponent } from '../product-form/product-form.component';

@Component({
    selector: 'app-product-management',
    templateUrl: './product-management.component.html',
    styleUrls: ['./product-management.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatTableModule,
        MatSortModule,
        MatPaginatorModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatSnackBarModule,
        CurrencyPipe
    ]
})
export class ProductManagementComponent implements OnInit {
    displayedColumns: string[] = ['id', 'name', 'categoryName', 'price', 'actions'];
    dataSource: MatTableDataSource<Product> = new MatTableDataSource<Product>([]);
    totalItems: number = 0;
    currentPage: number = 1;
    pageSize: number = 10;

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    constructor(
        private productService: ProductService,
        private categoryService: CategoryService,
        private dialog: MatDialog,
        private snackBar: MatSnackBar
    ) { 
        console.log('ProductManagementComponent constructor called');
    }

    ngOnInit(): void {
        console.log('ProductManagementComponent ngOnInit called');
        this.loadProducts();
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    loadProducts(): void {
        this.productService.getProducts(this.currentPage, this.pageSize).subscribe({
            next: (response) => {
                this.dataSource.data = response.value;
                this.totalItems = response.totalItems;
            },
            error: (error) => {
                this.snackBar.open('Error loading products', 'Close', { duration: 3000 });
            }
        });
    }

    onPageChange(event: any): void {
        this.currentPage = event.pageIndex + 1;
        this.pageSize = event.pageSize;
        this.loadProducts();
    }

    openProductForm(product?: Product): void {
        const dialogRef = this.dialog.open(ProductFormComponent, {
            width: '600px',
            data: { product }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.loadProducts();
            }
        });
    }

    deleteProduct(id: number): void {
        if (confirm('Are you sure you want to delete this product?')) {
            this.productService.deleteProduct(id).subscribe({
                next: (success) => {
                    if (success) {
                        this.snackBar.open('Product deleted successfully', 'Close', { duration: 3000 });
                        this.loadProducts();
                    } else {
                        this.snackBar.open('Failed to delete product', 'Close', { duration: 3000 });
                    }
                },
                error: (error) => {
                    this.snackBar.open('Error deleting product', 'Close', { duration: 3000 });
                }
            });
        }
    }
} 