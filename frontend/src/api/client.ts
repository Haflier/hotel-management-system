import axios, {
  AxiosError,
  type InternalAxiosRequestConfig,
} from "axios";

import {
  clearAuthStorage,
  getAccessToken,
  getRefreshToken,
  getUserId,
  saveAuthResponse,
} from "../features/auth/tokenStorage";

import type { AuthResponse } from "../features/auth/types";

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 10_000,
});

let refreshPromise: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
  const userId = getUserId();
  const token = getAccessToken();
  const refreshToken = getRefreshToken();

  if (!userId || !token || !refreshToken) {
    clearAuthStorage();
    return null;
  }

  if (!refreshPromise) {
    refreshPromise = axios
      .post<AuthResponse>(
        `${import.meta.env.VITE_API_BASE_URL}/refreshtoken`,
        {
          userId,
          token,
          refreshToken,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        },
      )
      .then((response) => {
        saveAuthResponse(response.data);
        return response.data.token;
      })
      .catch(() => {
        clearAuthStorage();
        return null;
      })
      .finally(() => {
        refreshPromise = null;
      });
  }

  return refreshPromise;
}

apiClient.interceptors.request.use(
  (config) => {
    const token = getAccessToken();

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest =
      error.config as
      | (InternalAxiosRequestConfig & {
        _retry?: boolean;
      })
      | undefined;

    if (
      error.response?.status !== 401 ||
      !originalRequest ||
      originalRequest._retry ||
      originalRequest.url?.includes("/refreshtoken") ||
      originalRequest.url?.includes("/login")
    ) {
      return Promise.reject(error);
    }

    originalRequest._retry = true;

    const newAccessToken =
      await refreshAccessToken();

    if (!newAccessToken) {
      return Promise.reject(error);
    }

    originalRequest.headers.Authorization =
      `Bearer ${newAccessToken}`;

    return apiClient(originalRequest);
  },
);
