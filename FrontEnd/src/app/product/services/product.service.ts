import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../core/services/api.service';
import { HttpClient } from '@angular/common/http';
import { Product, ProductCreate, ProductUpdate } from '../../core/models/product.model';
import { PaginatedResponse, ApiResponse } from '../../core/models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends ApiService {
  private endpoint = 'products';

  constructor(http: HttpClient) {
    super(http);
  }

  getProducts(page: number = 1, pageSize: number = 10, categoryId?: number): Observable<PaginatedResponse<Product>> {
    const params: any = { page, pageSize };
    if (categoryId) {
      params.categoryId = categoryId;
    }
    return this.getPaginated<Product>(this.endpoint, params);
  }

  getProduct(id: number): Observable<ApiResponse<Product>> {
    return this.http.get<ApiResponse<Product>>(`${this.baseUrl}/api/${this.apiVersion}/${this.endpoint}/${id}`);
  }

  createProduct(product: ProductCreate): Observable<number> {
    return this.post<number>(this.endpoint, product);
  }

  updateProduct(product: ProductUpdate): Observable<any> {
    return this.put<any>(`${this.endpoint}`, product);
  }

  deleteProduct(id: number): Observable<boolean> {
    return this.delete<boolean>(`${this.endpoint}/${id}`);
  }

  getProductsByCategory(category: string): Observable<Product[]> {
    return this.get<Product[]>(this.endpoint, { category });
  }

  searchProducts(query: string): Observable<Product[]> {
    return this.get<Product[]>(this.endpoint, { search: query });
  }
} 