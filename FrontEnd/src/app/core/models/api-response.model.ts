export interface ApiResponse<T> {
  isSuccess: boolean;
  error: ApiError | null;
  value: T;
}

export interface ApiError {
  code: string;
  message: string;
}

export interface PaginatedResponse<T> extends ApiResponse<T[]> {
  currentPage: number;
  pageSize: number;
  totalPages: number;
  totalItems: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
} 