export interface Food {
  id: number;
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface Drink {
  id: number;
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface CreateOrderItemRequest {
  quantity: number;
  foodId?: number;
  drinkId?: number;
  reservationId: number;
}

export interface OrderItem {
  id: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  foodId?: number | null;
  foodName?: string | null;
  drinkId?: number | null;
  drinkName?: string | null;
  orderId: number;
}

export interface Order {
  id: number;
  apiUserId: string;
  reservationId: number;
  createdAt: string;
  items: OrderItem[];
  totalPrice: number;
  isFinalized: boolean;
}
