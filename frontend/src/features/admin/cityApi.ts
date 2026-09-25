import { apiClient } from "../../api/client";

export interface City {
  id: number;
  name: string;
}

export async function getCities(): Promise<City[]> {
  const response = await apiClient.get<City[]>(
    "/api/City/GetAll",
  );

  return response.data;
}
