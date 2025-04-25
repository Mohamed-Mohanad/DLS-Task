export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  categoryName: string;
  stock: number;
  imageUrl?: string;
}

export interface ProductCreate {
  name: string;
  description: string;
  price: number;
  categoryId: number;
}

export interface ProductUpdate {
  id: number;
  name?: string;
  description?: string;
  price?: number;
  categoryId?: number;
}

export interface Category {
  id: number;
  name: string;
  description: string;
  parentId: number | null;
  parentName: string;
  childrenCount?: number;
  children?: CategoryChild[];
}

export interface CategoryChild {
  id: number;
  name: string;
  description: string;
}

export interface CategoryCreate {
  name: string;
  description: string;
  parentId: number | null;
}

export interface CategoryUpdate {
  id: number;
  name?: string;
  description?: string;
  parentId?: number | null;
} 