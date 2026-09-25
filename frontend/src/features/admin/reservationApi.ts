import { apiClient } from "../../api/client";

import type { Reservation } from "../reservations/types";

export async function getReservations(): Promise<Reservation[]> {
  const response = await apiClient.get<Reservation[]>(
    "/api/Reservation",
  );

  return response.data;
}
