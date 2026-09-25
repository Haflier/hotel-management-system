import { apiClient } from "../../api/client";

import type { Room } from "./types";

export async function getRooms(): Promise<Room[]> {
  const response = await apiClient.get<Room[]>(
    "/api/Room/GetAll",
  );

  return response.data;
}
