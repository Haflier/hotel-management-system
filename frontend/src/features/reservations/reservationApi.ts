import { apiClient } from "../../api/client";

import type {
  CreateReservationRequest,
  Reservation,
} from "./types";

export async function createReservation(
  request: CreateReservationRequest,
): Promise<Reservation> {
  const response = await apiClient.post<Reservation>(
    "/api/Reservation",
    request,
  );

  return response.data;
}

export async function getReservation(
  id: number,
): Promise<Reservation> {
  const response = await apiClient.get<Reservation>(
    `/api/Reservation/${id}`,
  );

  return response.data;
}

export async function getMyReservations(): Promise<Reservation[]> {
  const response = await apiClient.get<Reservation[]>(
    "/api/Reservation/MyReservations",
  );

  return response.data;
}
