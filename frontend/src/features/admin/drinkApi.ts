import { apiClient } from "../../api/client";

export interface Drink {
  id: number;
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface CreateDrinkRequest {
  name: string;
  description: string;
  price: number;
  meal: string;
  hotelId: number;
}

export interface UpdateDrinkRequest extends CreateDrinkRequest {
  id: number;
}

export async function getDrinks(): Promise<Drink[]> {
  const response = await apiClient.get<Drink[]>("/api/Drink");

  return response.data;
}

export async function createDrink(
  request: CreateDrinkRequest,
): Promise<Drink> {
  const response = await apiClient.post<Drink>(
    "/api/Drink",
    request,
  );

  return response.data;
}

export async function updateDrink(
  request: UpdateDrinkRequest,
): Promise<void> {
  await apiClient.put(
    `/api/Drink/${request.id}`,
    request,
  );
}

export async function deleteDrink(
  drinkId: number,
): Promise<void> {
  await apiClient.delete(`/api/Drink/${drinkId}`);
}
