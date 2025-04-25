import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ApiResponse, PaginatedResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  protected baseUrl = environment.apiUrl;
  protected apiVersion = 'v1'; // Default API version

  constructor(protected http: HttpClient) { }

  protected get<T>(endpoint: string, params?: any): Observable<T> {
    const httpParams = this.createHttpParams(params);
    return this.http.get<ApiResponse<T>>(`${this.baseUrl}/api/${this.apiVersion}/${endpoint}`, { params: httpParams })
      .pipe(
        retry(1),
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.error?.message || 'Unknown error');
          }
          return response.value;
        }),
        catchError(this.handleError)
      );
  }

  protected getPaginated<T>(endpoint: string, params?: any): Observable<PaginatedResponse<T>> {
    const httpParams = this.createHttpParams(params);
    return this.http.get<PaginatedResponse<T>>(`${this.baseUrl}/api/${this.apiVersion}/${endpoint}`, { params: httpParams })
      .pipe(
        retry(1),
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.error?.message || 'Unknown error');
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  protected post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<ApiResponse<T>>(`${this.baseUrl}/api/${this.apiVersion}/${endpoint}`, data)
      .pipe(
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.error?.message || 'Unknown error');
          }
          return response.value;
        }),
        catchError(this.handleError)
      );
  }

  protected put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<ApiResponse<T>>(`${this.baseUrl}/api/${this.apiVersion}/${endpoint}`, data)
      .pipe(
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.error?.message || 'Unknown error');
          }
          return response.value;
        }),
        catchError(this.handleError)
      );
  }

  protected delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<ApiResponse<T>>(`${this.baseUrl}/api/${this.apiVersion}/${endpoint}`)
      .pipe(
        map(response => {
          if (!response.isSuccess) {
            throw new Error(response.error?.message || 'Unknown error');
          }
          return response.value;
        }),
        catchError(this.handleError)
      );
  }

  private createHttpParams(params: any): HttpParams {
    let httpParams = new HttpParams();
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.append(key, params[key].toString());
        }
      });
    }
    return httpParams;
  }

  private handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else if (error.error?.error?.message) {
      // API error with message
      errorMessage = error.error.error.message;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
} 