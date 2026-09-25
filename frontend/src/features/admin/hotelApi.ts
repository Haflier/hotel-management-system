import { apiClient } from "../../api/client";

import type { Hotel } from "../hotels/types";

export interface CreateHotelRequest {
  name: string;
  description: string;
  phone: string;
  address: string;
  cityId: number;
}

export interface UpdateHotelRequest extends CreateHotelRequest {
  id: number;
}

export async function createHotel(
  request: CreateHotelRequest,
): Promise<Hotel> {
  const response = await apiClient.post<Hotel>(
    "/api/Hotel",
    request,
  );

  return response.data;
}

export async function updateHotel(
  request: UpdateHotelRequest,
): Promise<void> {
  await apiClient.put(
    `/api/Hotel/${request.id}`,
    request,
  );
}

export async function deleteHotel(
  hotelId: number,
): Promise<void> {
  await apiClient.delete(
    `/api/Hotel/${hotelId}`,
  );
}
