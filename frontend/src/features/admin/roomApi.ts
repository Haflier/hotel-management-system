import { apiClient } from "../../api/client";

import type { Room } from "../rooms/types";

export interface CreateRoomRequest {
  roomNumber: string;
  bedNumbers: number;
  basePricePerDay: number;
  hotelId: number;
}

export interface UpdateRoomRequest extends CreateRoomRequest {
  id: number;
}

export async function createRoom(
  request: CreateRoomRequest,
): Promise<Room> {
  const response = await apiClient.post<Room>(
    "/api/Room",
    request,
  );

  return response.data;
}

export async function updateRoom(
  request: UpdateRoomRequest,
): Promise<void> {
  await apiClient.put(
    `/api/Room/${request.id}`,
    request,
  );
}

export async function deleteRoom(
  roomId: number,
): Promise<void> {
  await apiClient.delete(
    `/api/Room/${roomId}`,
  );
}
