import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import {
  createService,
  deleteService,
  getServices,
  updateService,
} from "./serviceApi";

import type {
  CreateServiceRequest,
  UpdateServiceRequest,
} from "./serviceApi";

const emptyForm: CreateServiceRequest = {
  name: "",
  description: "",
  price: 0,
};

export function ServicesAdminPage() {
  const queryClient = useQueryClient();

  const [form, setForm] =
    useState<CreateServiceRequest>(emptyForm);

  const [editingId, setEditingId] =
    useState<number | null>(null);

  const servicesQuery = useQuery({
    queryKey: ["admin", "services"],
    queryFn: getServices,
  });

  const createMutation = useMutation({
    mutationFn: createService,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["admin", "services"],
      });

      setForm(emptyForm);
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateService,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["admin", "services"],
      });

      setEditingId(null);
      setForm(emptyForm);
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteService,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["admin", "services"],
      });
    },
  });

  function handleSubmit(event: React.FormEvent) {
    event.preventDefault();

    if (editingId !== null) {
      const request: UpdateServiceRequest = {
        id: editingId,
        ...form,
      };

      updateMutation.mutate(request);
      return;
    }

    createMutation.mutate(form);
  }

  function handleEdit(service: {
    id: number;
    name: string;
    description: string;
    price: number;
  }) {
    setEditingId(service.id);

    setForm({
      name: service.name,
      description: service.description,
      price: service.price,
    });
  }

  function handleDelete(serviceId: number) {
    if (!window.confirm("Delete this service?")) {
      return;
    }

    deleteMutation.mutate(serviceId);
  }

  function handleCancelEdit() {
    setEditingId(null);
    setForm(emptyForm);
  }

  if (servicesQuery.isLoading) {
    return <p>Loading services...</p>;
  }

  if (servicesQuery.isError) {
    return <p>Failed to load services.</p>;
  }

  const services = servicesQuery.data ?? [];

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold">
          Manage Services
        </h1>

        <p className="text-sm text-gray-600">
          Create, update and delete hotel services.
        </p>
      </div>

      <form
        onSubmit={handleSubmit}
        className="space-y-4 rounded border text-[#000000] p-4"
      >
        <h2 className="text-lg font-semibold">
          {editingId === null
            ? "Create Service"
            : `Edit Service #${editingId}`}
        </h2>

        <div className="space-y-1">
          <label
            htmlFor="service-name"
            className="block text-sm font-medium"
          >
            Name
          </label>

          <input
            id="service-name"
            type="text"
            value={form.name}
            onChange={(event) =>
              setForm({
                ...form,
                name: event.target.value,
              })
            }
            placeholder="e.g. Room Cleaning"
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="service-description"
            className="block text-sm font-medium"
          >
            Description
          </label>

          <textarea
            id="service-description"
            value={form.description}
            onChange={(event) =>
              setForm({
                ...form,
                description: event.target.value,
              })
            }
            placeholder="e.g. Daily room cleaning"
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="service-price"
            className="block text-sm font-medium"
          >
            Price
          </label>

          <input
            id="service-price"
            type="text"
            inputMode="decimal"
            value={form.price === 0 ? "" : form.price}
            onChange={(event) =>
              setForm({
                ...form,
                price:
                  Number(event.target.value) || 0,
              })
            }
            placeholder="e.g. 25.00"
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="flex gap-2">
          <button
            type="submit"
            disabled={
              createMutation.isPending ||
              updateMutation.isPending
            }
            className="cursor-pointer rounded px-4 py-2 bg-[#000000] text-white"
          >
            {editingId === null
              ? "Create Service"
              : "Update Service"}
          </button>

          {editingId !== null && (
            <button
              type="button"
              onClick={handleCancelEdit}
              className="rounded text-red-600 underline  px-4 py-2"
            >
              Cancel
            </button>
          )}
        </div>
      </form>

      <div>
        <h2 className="mb-3 text-lg font-semibold">
          Services
        </h2>

        <table className="w-full border-collapse border text-[#000000]">
          <thead>
            <tr>
              <th className="border p-2 text-left">ID</th>
              <th className="border p-2 text-left">Name</th>
              <th className="border p-2 text-left">
                Description
              </th>
              <th className="border p-2 text-left">
                Price
              </th>
              <th className="border p-2 text-left">
                Actions
              </th>
            </tr>
          </thead>

          <tbody>
            {services.map((service) => (
              <tr key={service.id}>
                <td className="border p-2">
                  {service.id}
                </td>

                <td className="border p-2">
                  {service.name}
                </td>

                <td className="border p-2">
                  {service.description}
                </td>

                <td className="border p-2">
                  {service.price}
                </td>

                <td className="border p-2">
                  <div className="flex gap-2">
                    <button
                      type="button"
                      onClick={() =>
                        handleEdit(service)
                      }
                      className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                      Edit
                    </button>

                    <button
                      type="button"
                      onClick={() =>
                        handleDelete(service.id)
                      }
                      disabled={deleteMutation.isPending}
                      className="cursor-pointer text-red-600 underline hover:text-red-800 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                      Delete
                    </button>
                  </div>
                </td>
              </tr>
            ))}

            {services.length === 0 && (
              <tr>
                <td
                  colSpan={5}
                  className="border p-4 text-center"
                >
                  No services found.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
