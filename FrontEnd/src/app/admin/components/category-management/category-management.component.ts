import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CategoryService } from '../../../product/services/category.service';
import { Category } from '../../../core/models/product.model';
import { CategoryFormComponent } from '../category-form/category-form.component';

@Component({
    selector: 'app-category-management',
    templateUrl: './category-management.component.html',
    styleUrls: ['./category-management.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatTableModule,
        MatSortModule,
        MatPaginatorModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatSnackBarModule
    ]
})
export class CategoryManagementComponent implements OnInit {
    displayedColumns: string[] = ['id', 'name', 'description', 'parentName', 'actions'];
    dataSource: MatTableDataSource<Category> = new MatTableDataSource<Category>([]);
    totalItems: number = 0;
    currentPage: number = 1;
    pageSize: number = 10;

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    constructor(
        private categoryService: CategoryService,
        private dialog: MatDialog,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit(): void {
        this.loadCategories();
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
    }

    loadCategories(): void {
        this.categoryService.getCategories(this.currentPage, this.pageSize).subscribe({
            next: (response) => {
                this.dataSource.data = [...response.value];
                this.totalItems = response.totalItems;
                this.dataSource._updateChangeSubscription();
            },
            error: (error) => {
                this.snackBar.open('Error loading categories', 'Close', { duration: 3000 });
            }
        });
    }

    onPageChange(event: any): void {
        this.currentPage = event.pageIndex + 1;
        this.pageSize = event.pageSize;
        this.loadCategories();
    }

    openCategoryForm(category?: Category): void {
        const dialogRef = this.dialog.open(CategoryFormComponent, {
            width: '600px',
            data: { category }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.loadCategories();
            }
        });
    }

    deleteCategory(id: number): void {
        if (confirm('Are you sure you want to delete this category?')) {
            this.categoryService.deleteCategory(id).subscribe({
                next: (success) => {
                    if (success) {
                        this.snackBar.open('Category deleted successfully', 'Close', { duration: 3000 });
                        this.loadCategories();
                    } else {
                        this.snackBar.open('Failed to delete category', 'Close', { duration: 3000 });
                    }
                },
                error: (error) => {
                    this.snackBar.open('Error deleting category', 'Close', { duration: 3000 });
                }
            });
        }
    }
} 