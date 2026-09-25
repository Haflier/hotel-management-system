import { apiClient } from "./client";

export async function checkApiHealth(): Promise<number> {
  const response = await apiClient.get("/swagger/index.html");

  return response.status;
}
