import {
  createContext,
  useContext,
  useMemo,
  type ReactNode,
} from "react";
import { useNavigate } from "react-router-dom";

import {
  clearAuthStorage,
  getAccessToken,
  getUserRole,
} from "./tokenStorage";

interface AuthContextValue {
  isAuthenticated: boolean;
  role: string | null;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(
  undefined,
);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const navigate = useNavigate();

  const isAuthenticated = Boolean(getAccessToken());
  const role = getUserRole();

  function logout() {
    clearAuthStorage();
    navigate("/login");
  }

  const value = useMemo(
    () => ({
      isAuthenticated,
      role,
      logout,
    }),
    [isAuthenticated, role],
  );

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error(
      "useAuth must be used inside an AuthProvider",
    );
  }

  return context;
}
