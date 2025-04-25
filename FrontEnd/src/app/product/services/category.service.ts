import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { HttpClient } from '@angular/common/http';
import { Category, CategoryCreate, CategoryUpdate } from '../../core/models/product.model';
import { PaginatedResponse } from '../../core/models/api-response.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryService extends ApiService {
    private endpoint = 'categories';

    constructor(http: HttpClient) {
        super(http);
    }

    getCategories(page: number = 1, pageSize: number = 10): Observable<PaginatedResponse<Category>> {
        return this.getPaginated<Category>(this.endpoint, { page, pageSize });
    }

    getCategory(id: number): Observable<Category> {
        return this.get<Category>(`${this.endpoint}/${id}`);
    }

    createCategory(category: CategoryCreate): Observable<number> {
        return this.post<number>(this.endpoint, category);
    }

    updateCategory(category: CategoryUpdate): Observable<boolean> {
        return this.put<boolean>(this.endpoint, category);
    }

    deleteCategory(id: number): Observable<boolean> {
        return this.delete<boolean>(`${this.endpoint}/${id}`);
    }
} 