import { apiClient } from "../../api/client";
import type { Order } from "../orders/types";

export async function getAdminOrders(): Promise<Order[]> {
  const response = await apiClient.get<Order[]>("/api/Order");
  return response.data;
}

export async function deleteOrder(orderId: number): Promise<void> {
  await apiClient.delete(`/api/Order/${orderId}`);
}
