import { apiClient } from "../../api/client";

import type { Service } from "./types";

export interface CreateServiceRequest {
  name: string;
  description: string;
  price: number;
}

export interface UpdateServiceRequest extends CreateServiceRequest {
  id: number;
}

export async function getServices(): Promise<Service[]> {
  const response = await apiClient.get<Service[]>(
    "/api/Service/GetAll",
  );

  return response.data;
}

export async function createService(
  request: CreateServiceRequest,
): Promise<Service> {
  const response = await apiClient.post<Service>(
    "/api/Service",
    request,
  );

  return response.data;
}

export async function updateService(
  request: UpdateServiceRequest,
): Promise<void> {
  await apiClient.put(
    `/api/Service/${request.id}`,
    request,
  );
}

export async function deleteService(
  serviceId: number,
): Promise<void> {
  await apiClient.delete(
    `/api/Service/${serviceId}`,
  );
}

export async function assignServiceToRoom(
  roomId: number,
  serviceId: number,
): Promise<void> {
  await apiClient.post(
    `/api/Room/${roomId}/services/${serviceId}`,
  );
}

export async function removeServiceFromRoom(
  roomId: number,
  serviceId: number,
): Promise<void> {
  await apiClient.delete(
    `/api/Room/${roomId}/services/${serviceId}`,
  );
}
