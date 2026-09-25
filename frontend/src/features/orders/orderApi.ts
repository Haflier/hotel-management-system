import axios from "axios";
import { apiClient } from "../../api/client";

import type {
  CreateOrderItemRequest,
  Drink,
  Food,
  Order,
  OrderItem,
} from "./types";

export async function getFoodsForReservation(
  reservationId: number,
): Promise<Food[]> {
  const response = await apiClient.get<Food[]>(
    `/api/Food/ForReservation/${reservationId}`,
  );

  return response.data;
}

export async function getDrinksForReservation(
  reservationId: number,
): Promise<Drink[]> {
  const response = await apiClient.get<Drink[]>(
    `/api/Drink/ForReservation/${reservationId}`,
  );

  return response.data;
}

export async function createOrderItem(
  request: CreateOrderItemRequest,
): Promise<OrderItem> {
  const response = await apiClient.post<OrderItem>(
    "/api/OrderItem",
    request,
  );

  return response.data;
}

export async function getOrder(
  orderId: number,
): Promise<Order> {
  const response = await apiClient.get<Order>(
    `/api/Order/${orderId}`,
  );

  return response.data;
}

export async function finalizeOrder(
  orderId: number,
): Promise<Order> {
  const response = await apiClient.post<Order>(
    `/api/Order/${orderId}/finalize`,
  );

  return response.data;
}

export async function getCurrentOrder(): Promise<Order | null> {
  try {
    const response = await apiClient.get<Order>(
      "/api/Order/Current",
    );

    return response.data;
  } catch (error) {
    if (
      axios.isAxiosError(error) &&
      error.response?.status === 404
    ) {
      return null;
    }

    throw error;
  }
}
