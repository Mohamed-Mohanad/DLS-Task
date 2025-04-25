import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CategoryService } from '../../../product/services/category.service';
import { Category, CategoryCreate, CategoryUpdate } from '../../../core/models/product.model';

interface CategoryFormData {
    category?: Category;
}

@Component({
    selector: 'app-category-form',
    templateUrl: './category-form.component.html',
    styleUrls: ['./category-form.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatSnackBarModule
    ]
})
export class CategoryFormComponent implements OnInit {
    categoryForm!: FormGroup;
    isEditMode: boolean = false;
    categories: Category[] = [];

    constructor(
        private dialogRef: MatDialogRef<CategoryFormComponent>,
        @Inject(MAT_DIALOG_DATA) public data: CategoryFormData,
        private fb: FormBuilder,
        private categoryService: CategoryService,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit(): void {
        this.isEditMode = !!this.data.category;
        this.loadCategories();
        this.initForm();
    }

    private initForm(): void {
        this.categoryForm = this.fb.group({
            name: [this.data.category?.name || '', [Validators.required, Validators.maxLength(100)]],
            description: [this.data.category?.description || '', [Validators.required, Validators.maxLength(500)]],
            parentId: [this.data.category?.parentId || null]
        });
    }

    private loadCategories(): void {
        this.categoryService.getCategories(1, 100).subscribe({
            next: (response) => {
                // Filter out the current category and its children if editing
                if (this.isEditMode) {
                    this.categories = response.value.filter(c => c.id !== this.data.category?.id);
                } else {
                    this.categories = response.value;
                }
            },
            error: (error) => {
                this.snackBar.open('Error loading categories', 'Close', { duration: 3000 });
            }
        });
    }

    onSubmit(): void {
        if (this.categoryForm.valid) {
            if (this.isEditMode) {
                this.updateCategory();
            } else {
                this.createCategory();
            }
        }
    }

    private createCategory(): void {
        const categoryData: CategoryCreate = {
            name: this.categoryForm.value.name,
            description: this.categoryForm.value.description,
            parentId: this.categoryForm.value.parentId
        };

        this.categoryService.createCategory(categoryData).subscribe({
            next: (id) => {
                this.snackBar.open('Category created successfully', 'Close', { duration: 3000 });
                this.dialogRef.close(true);
            },
            error: (error) => {
                this.snackBar.open('Error creating category', 'Close', { duration: 3000 });
            }
        });
    }

    private updateCategory(): void {
        const categoryData: CategoryUpdate = {
            id: this.data.category!.id,
            name: this.categoryForm.value.name,
            description: this.categoryForm.value.description,
            parentId: this.categoryForm.value.parentId
        };

        this.categoryService.updateCategory(categoryData).subscribe({
            next: (success) => {
                if (success) {
                    this.snackBar.open('Category updated successfully', 'Close', { duration: 3000 });
                    this.dialogRef.close(true);
                } else {
                    this.snackBar.open('Failed to update category', 'Close', { duration: 3000 });
                }
            },
            error: (error) => {
                this.snackBar.open('Error updating category', 'Close', { duration: 3000 });
            }
        });
    }

    onCancel(): void {
        this.dialogRef.close(false);
    }
} 