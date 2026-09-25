import { apiClient } from "../../api/client";

import type { Hotel } from "./types";

export async function getHotels(): Promise<Hotel[]> {
  const response = await apiClient.get<Hotel[]>(
    "/api/Hotel/GetAll",
  );

  return response.data;
}
