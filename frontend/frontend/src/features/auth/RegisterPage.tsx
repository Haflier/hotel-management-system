import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { getApiErrorMessage } from "../../api/errors";
import { register as registerUser } from "./authApi";
import {
  registerSchema,
  type RegisterFormData,
} from "./validation";

export function RegisterPage() {
  const navigate = useNavigate();

  const [serverError, setServerError] = useState<string | null>(
    null,
  );

  const {
    register: registerField,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  async function onSubmit(data: RegisterFormData) {
    setServerError(null);

    try {
      await registerUser(data);

      navigate("/login");
    } catch (error) {
      setServerError(getApiErrorMessage(error));
    }
  }

  return (
    <section className="mx-auto max-w-md space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">Register</h1>

        <p className="mt-1 text-sm text-gray-600">
          Create an account for the hotel management API.
        </p>
      </div>

      <form
        onSubmit={handleSubmit(onSubmit)}
        className="space-y-4 rounded border bg-white p-6"
      >
        <div>
          <label
            htmlFor="firstName"
            className="mb-1 block text-sm font-medium"
          >
            First name
          </label>

          <input
            id="firstName"
            type="text"
            {...registerField("firstName")}
            className="w-full rounded border px-3 py-2"
          />

          {errors.firstName && (
            <p className="mt-1 text-sm text-red-600">
              {errors.firstName.message}
            </p>
          )}
        </div>

        <div>
          <label
            htmlFor="lastName"
            className="mb-1 block text-sm font-medium"
          >
            Last name
          </label>

          <input
            id="lastName"
            type="text"
            {...registerField("lastName")}
            className="w-full rounded border px-3 py-2"
          />

          {errors.lastName && (
            <p className="mt-1 text-sm text-red-600">
              {errors.lastName.message}
            </p>
          )}
        </div>

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
          className="w-full rounded bg-gray-900 px-4 py-2 text-white disabled:opacity-50"
        >
          {isSubmitting ? "Creating account..." : "Register"}
        </button>

        <p className="text-sm text-gray-600">
          Already have an account?{" "}
          <Link
            to="/login"
            className="text-blue-600 underline"
          >
            Login
          </Link>
        </p>
      </form>
    </section>
  );
}
