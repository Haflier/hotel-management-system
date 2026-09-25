import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { getApiErrorMessage } from "../../api/errors";
import { login } from "./authApi";
import { saveAuthResponse } from "./tokenStorage";
import {
  loginSchema,
  type LoginFormData,
} from "./validation";

export function LoginPage() {
  const navigate = useNavigate();

  const [serverError, setServerError] = useState<string | null>(
    null,
  );

  const {
    register: registerField,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  async function onSubmit(data: LoginFormData) {
    setServerError(null);

    try {
      const authResponse = await login(data);

      if (!authResponse.token) {
        setServerError("The API did not return an access token.");
        return;
      }

      saveAuthResponse(authResponse);

      navigate("/dashboard");
    } catch (error) {
      setServerError(getApiErrorMessage(error));
    }
  }

  return (
    <section className="mx-auto max-w-md space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">Login</h1>
        <p className="mt-1 text-sm text-gray-600">
          Sign in using your API account.
        </p>
      </div>

      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-4 rounded border text-[#000000] p-6"
      >
        <div>
          <label
            htmlFor="email"
            className="mb-1 block text-sm font-medium"
          >
            Email
          </label>

          <input
            id="email"
            type="email"
            {...registerField("email")}
            className="w-full rounded border px-3 py-2"
          />

          {errors.email && (
            <p className="mt-1 text-sm text-red-600">
              {errors.email.message}
            </p>
          )}
        </div>

        <div>
          <label
            htmlFor="password"
            className="mb-1 block text-sm font-medium"
          >
            Password
          </label>

          <input
            id="password"
            type="password"
            {...registerField("password")}
            className="w-full rounded border px-3 py-2"
          />

          {errors.password && (
            <p className="mt-1 text-sm text-red-600">
              {errors.password.message}
            </p>
          )}
        </div>

        {serverError && (
          <p className="rounded bg-red-50 p-3 text-sm text-red-700">
            {serverError}
          </p>
        )}

        <button
          type="submit"
          disabled={isSubmitting}
          className="cursor-pointer w-full rounded bg-[#000000] px-4 py-2 text-white disabled:opacity-50"
        >
          {isSubmitting ? "Logging in..." : "Login"}
        </button>

        <p className="text-sm text-gray-600">
          Don't have an account?{" "}
          <Link
            to="/register"
            className="text-[#fb4100] underline"
          >
            Register
          </Link>
        </p>
      </form>
    </section>
  );
}
