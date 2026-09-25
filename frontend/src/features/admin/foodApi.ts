import { apiClient } from "../../api/client";

export interface Food {
  id: number;
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface CreateFoodRequest {
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface UpdateFoodRequest extends CreateFoodRequest {
  id: number;
}

export async function getFoods(): Promise<Food[]> {
  const response = await apiClient.get<Food[]>("/api/Food");
  return response.data;
}

export async function createFood(
  request: CreateFoodRequest,
): Promise<Food> {
  const response = await apiClient.post<Food>(
    "/api/Food",
    request,
  );

  return response.data;
}

export async function updateFood(
  request: UpdateFoodRequest,
): Promise<void> {
  await apiClient.put(`/api/Food/${request.id}`, request);
}

export async function deleteFood(foodId: number): Promise<void> {
  await apiClient.delete(`/api/Food/${foodId}`);
}
