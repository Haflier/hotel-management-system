import { apiClient } from "../../api/client";

export interface RoomService {
  id: number;
  name: string;
  description: string;
  price: number;
}

export interface RoomDetails {
  id: number;
  roomNumber: string;
  bedNumbers: number;
  reservedDates: string[];
  basePricePerDay: number;
  activeServices: RoomService[];
}

export async function getRoomDetails(
  roomId: number,
): Promise<RoomDetails> {
  const response = await apiClient.get<RoomDetails>(
    `/api/Room/AllDetails/${roomId}`,
  );

  return response.data;
}
