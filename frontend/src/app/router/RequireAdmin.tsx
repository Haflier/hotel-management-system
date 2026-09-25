import { Navigate, Outlet } from "react-router-dom";

import { useAuth } from "../../features/auth/AuthContext";

export function RequireAdmin() {
  const { isAuthenticated, role } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  if (role !== "Administrator") {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
