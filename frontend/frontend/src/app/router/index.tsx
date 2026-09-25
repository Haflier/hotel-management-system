import { createBrowserRouter } from "react-router-dom";

import App from "../../App";
import { LoginPage } from "../../features/auth/LoginPage";
import { RegisterPage } from "../../features/auth/RegisterPage";
import { PublicLayout } from "../layouts/PublicLayout";

export const router = createBrowserRouter([
  {
    element: <PublicLayout />,
    errorElement: <RouterErrorPage />,
    children: [
      {
        path: "/",
        element: <App />,
      },
      {
        path: "/login",
        element: <LoginPage />,
      },
      {
        path: "/register",
        element: <RegisterPage />,
      },
    ],
  },
]);

function RouterErrorPage() {
  return (
    <main className="mx-auto max-w-2xl space-y-4 px-6 py-12">
      <h1 className="text-2xl font-semibold">
        Page not found
      </h1>

      <p className="text-gray-600">
        The requested page does not exist.
      </p>

      <a
        href="/"
        className="inline-block rounded bg-gray-900 px-4 py-2 text-white"
      >
        Return home
      </a>
    </main>
  );
}
