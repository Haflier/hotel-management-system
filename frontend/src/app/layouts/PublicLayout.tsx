import { Link, Outlet } from "react-router-dom";

import { AuthProvider, useAuth } from "../../features/auth/AuthContext";

export function PublicLayout() {
  return (
    <AuthProvider>
      <PublicLayoutContent />
    </AuthProvider>
  );
}

function PublicLayoutContent() {
  const { isAuthenticated, role, logout } = useAuth();

  return (
    <div className="min-h-screen bg-[#C0C08A] text-[#FFFFFF]">
      <header className="border-b border-[#000000] bg-[#000000]">
        <nav className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <Link to="/" className="font-semibold">
            Hotel Management
          </Link>

          <div className="flex items-center gap-4 text-sm">
            <Link to="/">Home</Link>

            {isAuthenticated ? (
              <>
                <Link to="/dashboard">Dashboard</Link>

                <Link to="/hotels">Hotels</Link>

                <Link to="/rooms">Rooms</Link>

                <Link to="/reservations">My Reservations</Link>

                {role === "Administrator" && (
                  <>
                    <Link to="/admin/reservations">
                      Reservations
                    </Link>

                    <Link to="/admin/hotels">
                      Hotels
                    </Link>

                    <Link to="/admin/rooms">
                      Rooms
                    </Link>

                    <Link to="/admin/services">
                      Services
                    </Link>

                    <Link to="/admin/room-services">
                      Room Services
                    </Link>

                    <Link to="/admin/orders">
                      Orders
                    </Link>

                    <Link to="/admin/factors">
                      Factors
                    </Link>

                    <Link to="/admin/foods">
                      Foods
                    </Link>

                    <Link to="/admin/drinks">
                      Drinks
                    </Link>
                  </>
                )}

                <button
                  type="button"
                  onClick={logout}
                  className="cursor-pointer text-red-600"
                >
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link to="/login">Login</Link>

                <Link to="/register">Register</Link>
              </>
            )}
          </div>
        </nav>
      </header>

      <main className="mx-auto max-w-6xl px-6 py-8 text-[#0747af]">
        <Outlet />
      </main>
    </div>
  );
}
