import { apiClient } from "../../api/client";

import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from "./types";

export async function register(
  request: RegisterRequest,
): Promise<void> {
  await apiClient.post("/register", request);
}

export async function login(
  request: LoginRequest,
): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>(
    "/login",
    request,
  );

  return response.data;
}

export async function refreshToken(
  request: AuthResponse,
): Promise<AuthResponse> {
  const response = await apiClient.post<AuthResponse>(
    "/refreshtoken",
    request,
  );

  return response.data;
}
