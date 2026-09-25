import { apiClient } from "../../api/client";

export interface Factor {
  id: number;
  apiUserId: string;
  createdAt: string;
  finalPrice: number;
}

export interface CreateFactorRequest {
  apiUserId: string;
  finalPrice: number;
}

export interface UpdateFactorRequest {
  id: number;
  apiUserId: string;
  createdAt: string;
  finalPrice: number;
}

export async function getFactors(): Promise<Factor[]> {
  const response = await apiClient.get<Factor[]>("/api/Factor");
  return response.data;
}

export async function createFactor(
  request: CreateFactorRequest,
): Promise<Factor> {
  const response = await apiClient.post<Factor>("/api/Factor", request);
  return response.data;
}

export async function updateFactor(
  request: UpdateFactorRequest,
): Promise<void> {
  await apiClient.put(`/api/Factor/${request.id}`, request);
}

export async function deleteFactor(factorId: number): Promise<void> {
  await apiClient.delete(`/api/Factor/${factorId}`);
}
