import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import {
  createFactor,
  deleteFactor,
  getFactors,
  updateFactor,
  type Factor,
} from "./factorApi";

export function FactorsAdminPage() {
  const queryClient = useQueryClient();

  const [editingFactor, setEditingFactor] = useState<Factor | null>(null);
  const [apiUserId, setApiUserId] = useState("");
  const [finalPrice, setFinalPrice] = useState("");

  const factorsQuery = useQuery({
    queryKey: ["admin", "factors"],
    queryFn: getFactors,
  });

  const resetForm = () => {
    setEditingFactor(null);
    setApiUserId("");
    setFinalPrice("");
  };

  const createMutation = useMutation({
    mutationFn: createFactor,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["admin", "factors"],
      });
      resetForm();
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateFactor,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["admin", "factors"],
      });
      resetForm();
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteFactor,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["admin", "factors"],
      });
    },
  });

  if (factorsQuery.isLoading) {
    return <p>Loading factors...</p>;
  }

  if (factorsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Factors
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(factorsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => factorsQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  const factors = factorsQuery.data ?? [];

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();

    const price = Number(finalPrice);

    if (!apiUserId.trim() || Number.isNaN(price)) {
      return;
    }

    if (editingFactor) {
      updateMutation.mutate({
        id: editingFactor.id,
        apiUserId: apiUserId.trim(),
        createdAt: editingFactor.createdAt,
        finalPrice: price,
      });
    } else {
      createMutation.mutate({
        apiUserId: apiUserId.trim(),
        finalPrice: price,
      });
    }
  };

  const startEditing = (factor: Factor) => {
    setEditingFactor(factor);
    setApiUserId(factor.apiUserId);
    setFinalPrice(String(factor.finalPrice));
  };

  const isSubmitting =
    createMutation.isPending || updateMutation.isPending;

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Factors
        </h1>

        <p className="mt-1 text-gray-600">
          Manage factors in the system.
        </p>
      </div>

      <form
        onSubmit={handleSubmit}
        className="space-y-4 rounded border p-4 text-[#000000]"
      >
        <div className="grid gap-4 md:grid-cols-2">
          <div className="space-y-1">
            <label
              htmlFor="api-user-id"
              className="block text-sm font-medium"
            >
              API User ID
            </label>

            <input
              id="api-user-id"
              type="text"
              value={apiUserId}
              onChange={(event) =>
                setApiUserId(event.target.value)
              }
              placeholder="Enter API user ID"
              className="w-full rounded border px-3 py-2"
            />
          </div>

          <div className="space-y-1">
            <label
              htmlFor="final-price"
              className="block text-sm font-medium"
            >
              Final Price
            </label>

            <input
              id="final-price"
              type="text"
              inputMode="decimal"
              value={finalPrice}
              onChange={(event) =>
                setFinalPrice(event.target.value)
              }
              placeholder="Enter final price"
              className="w-full rounded border px-3 py-2"
            />
          </div>
        </div>

        <div className="flex gap-2">
          <button
            type="submit"
            disabled={isSubmitting}
            className="cursor-pointer rounded bg-[#000000]  px-4 py-2 text-white disabled:cursor-not-allowed disabled:opacity-50"
          >
            {editingFactor ? "Update" : "Create"}
          </button>

          {editingFactor && (
            <button
              type="button"
              onClick={resetForm}
              disabled={isSubmitting}
              className="cursor-pointer text-red-600 underline  px-4 py-2 disabled:cursor-not-allowed disabled:opacity-50"
            >
              Cancel
            </button>
          )}
        </div>

        {createMutation.isError && (
          <p className="rounded bg-red-50 p-3 text-red-700">
            {getApiErrorMessage(createMutation.error)}
          </p>
        )}

        {updateMutation.isError && (
          <p className="rounded bg-red-50 p-3 text-red-700">
            {getApiErrorMessage(updateMutation.error)}
          </p>
        )}
      </form>

      {factors.length === 0 ? (
        <p>No factors found.</p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">User</th>
                <th className="px-4 py-3">Created</th>
                <th className="px-4 py-3">Final Price</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>

            <tbody>
              {factors.map((factor) => (
                <tr
                  key={factor.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {factor.id}
                  </td>

                  <td className="px-4 py-3 whitespace-nowrap">
                    {factor.apiUserId}
                  </td>

                  <td className="px-4 py-3 whitespace-nowrap">
                    {new Date(
                      factor.createdAt,
                    ).toLocaleString()}
                  </td>

                  <td className="px-4 py-3">
                    {factor.finalPrice}
                  </td>

                  <td className="px-4 py-3">
                    <div className="flex gap-3">
                      <button
                        type="button"
                        onClick={() => startEditing(factor)}
                        className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        onClick={() => {
                          if (
                            window.confirm(
                              `Delete factor #${factor.id}?`,
                            )
                          ) {
                            deleteMutation.mutate(factor.id);
                          }
                        }}
                        disabled={deleteMutation.isPending}
                        className="cursor-pointer text-red-600 underline hover:text-red-800 disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Delete
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {deleteMutation.isError && (
        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(deleteMutation.error)}
        </p>
      )}
    </section>
  );
}
