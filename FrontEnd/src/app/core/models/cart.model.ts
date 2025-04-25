export interface Cart {
  id: number;
  userId: number;
  totalPrice: number;
  itemCount: number;
  items: CartItem[];
}

export interface CartItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface AddToCartRequest {
  userId: number;
  productId: number;
  quantity: number;
}

export interface RemoveFromCartRequest {
  userId: number;
  productId: number;
} 