import type { AuthResponse } from "./types";

const ACCESS_TOKEN_KEY = "accessToken";
const REFRESH_TOKEN_KEY = "refreshToken";
const USER_ID_KEY = "userId";

export function saveAuthResponse(
  authResponse: AuthResponse,
): void {
  if (authResponse.token) {
    localStorage.setItem(ACCESS_TOKEN_KEY, authResponse.token);
  }

  if (authResponse.refreshToken) {
    localStorage.setItem(
      REFRESH_TOKEN_KEY,
      authResponse.refreshToken,
    );
  }

  if (authResponse.userId) {
    localStorage.setItem(USER_ID_KEY, authResponse.userId);
  }
}

export function clearAuthStorage(): void {
  localStorage.removeItem(ACCESS_TOKEN_KEY);
  localStorage.removeItem(REFRESH_TOKEN_KEY);
  localStorage.removeItem(USER_ID_KEY);
}

export function getAccessToken(): string | null {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

export function getRefreshToken(): string | null {
  return localStorage.getItem(REFRESH_TOKEN_KEY);
}

export function getUserId(): string | null {
  return localStorage.getItem(USER_ID_KEY);
}

const ROLE_CLAIM =
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

export function getUserRole(): string | null {
  const token = getAccessToken();

  if (!token) {
    return null;
  }

  try {
    const payload = token.split(".")[1];

    if (!payload) {
      return null;
    }

    const decodedPayload = JSON.parse(
      atob(payload.replace(/-/g, "+").replace(/_/g, "/")),
    );

    return decodedPayload[ROLE_CLAIM] ?? null;
  } catch {
    return null;
  }
}
