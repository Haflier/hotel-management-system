import { useAuth } from "./AuthContext";

export function DashboardPage() {
  const { logout } = useAuth();

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Dashboard
        </h1>

        <p className="mt-1 text-gray-600">
          You are authenticated.
        </p>
      </div>

      <div className="rounded border p-6 text-[#000000]">
        <h2 className="font-medium">
          Authentication test
        </h2>

        <p className="mt-2 text-sm text-gray-600">
          Your JWT is currently stored in the browser and will
          be attached to API requests.
        </p>

        <button
          type="button"
          onClick={logout}
          className="mt-4 cursor-pointer rounded bg-red-600 px-4 py-2 text-white"
        >
          Logout
        </button>
      </div>
    </section>
  );
}
