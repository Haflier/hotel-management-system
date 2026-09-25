import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import { getHotels } from "../hotels/hotelApi";
import type { Hotel } from "../hotels/types";
import {
  createHotel,
  deleteHotel,
  updateHotel,
  type CreateHotelRequest,
} from "./hotelApi";
import { getCities } from "./cityApi";

const emptyForm: CreateHotelRequest = {
  name: "",
  description: "",
  phone: "",
  address: "",
  cityId: 0,
};

export function HotelsAdminPage() {
  const queryClient = useQueryClient();

  const [form, setForm] =
    useState<CreateHotelRequest>(emptyForm);

  const [editingHotel, setEditingHotel] =
    useState<Hotel | null>(null);

  const hotelsQuery = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  const citiesQuery = useQuery({
    queryKey: ["cities"],
    queryFn: getCities,
  });

  const createMutation = useMutation({
    mutationFn: createHotel,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["hotels"],
      });

      setForm(emptyForm);
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateHotel,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["hotels"],
      });

      setEditingHotel(null);
      setForm(emptyForm);
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteHotel,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["hotels"],
      });
    },
  });

  if (hotelsQuery.isLoading || citiesQuery.isLoading) {
    return <p>Loading...</p>;
  }

  if (hotelsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Manage Hotels
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(hotelsQuery.error)}
        </p>
      </section>
    );
  }

  if (citiesQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Manage Hotels
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(citiesQuery.error)}
        </p>
      </section>
    );
  }

  const hotels = hotelsQuery.data ?? [];
  const cities = citiesQuery.data ?? [];

  function startEditing(hotel: Hotel) {
    setEditingHotel(hotel);

    setForm({
      name: hotel.name,
      description: hotel.description ?? "",
      phone: hotel.phone ?? "",
      address: hotel.address ?? "",
      cityId: hotel.cityId,
    });
  }

  function cancelEditing() {
    setEditingHotel(null);
    setForm(emptyForm);
  }

  function submit(event: React.FormEvent) {
    event.preventDefault();

    if (editingHotel) {
      updateMutation.mutate({
        id: editingHotel.id,
        ...form,
      });
    } else {
      createMutation.mutate(form);
    }
  }

  const mutationError =
    createMutation.error ??
    updateMutation.error ??
    deleteMutation.error;

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Manage Hotels
        </h1>

        <p className="mt-1 text-gray-600">
          Create, update and delete hotels.
        </p>
      </div>

      {mutationError && (
        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(mutationError)}
        </p>
      )}

      <form
        onSubmit={submit}
        className="space-y-4 rounded border p-6 text-[#000000]"
      >
        <h2 className="font-semibold">
          {editingHotel
            ? `Edit Hotel #${editingHotel.id}`
            : "Create Hotel"}
        </h2>

        <input
          value={form.name}
          onChange={(event) =>
            setForm({
              ...form,
              name: event.target.value,
            })
          }
          placeholder="Name"
          required
          className="w-full rounded border px-3 py-2"
        />

        <textarea
          value={form.description}
          onChange={(event) =>
            setForm({
              ...form,
              description: event.target.value,
            })
          }
          placeholder="Description"
          className="w-full rounded border px-3 py-2"
        />

        <input
          value={form.phone}
          onChange={(event) =>
            setForm({
              ...form,
              phone: event.target.value,
            })
          }
          placeholder="Phone"
          className="w-full rounded border px-3 py-2"
        />

        <input
          value={form.address}
          onChange={(event) =>
            setForm({
              ...form,
              address: event.target.value,
            })
          }
          placeholder="Address"
          className="w-full rounded border px-3 py-2"
        />

        <select
          value={form.cityId}
          onChange={(event) =>
            setForm({
              ...form,
              cityId: Number(event.target.value),
            })
          }
          required
          className="cursor-pointer w-full rounded border px-3 py-2"
        >
          <option value={0} disabled>
            Select city
          </option>

          {cities.map((city) => (
            <option
              key={city.id}
              value={city.id}
            >
              {city.name}
            </option>
          ))}
        </select>

        <div className="flex gap-3">
          <button
            type="submit"
            disabled={
              createMutation.isPending ||
              updateMutation.isPending
            }
            className="cursor-pointer rounded bg-[#000000] px-4 py-2 text-white disabled:opacity-50"
          >
            {editingHotel
              ? updateMutation.isPending
                ? "Updating..."
                : "Update Hotel"
              : createMutation.isPending
                ? "Creating..."
                : "Create Hotel"}
          </button>

          {editingHotel && (
            <button
              type="button"
              onClick={cancelEditing}
              className="cursor-pointer bg-red-600 text-white rounded px-4 py-2"
            >
              Cancel
            </button>
          )}
        </div>
      </form>

      {hotels.length === 0 ? (
        <p className="rounded border bg-white p-6">
          No hotels found.
        </p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Name</th>
                <th className="px-4 py-3">City</th>
                <th className="px-4 py-3">Phone</th>
                <th className="px-4 py-3">Address</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>

            <tbody>
              {hotels.map((hotel) => (
                <tr
                  key={hotel.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {hotel.id}
                  </td>

                  <td className="px-4 py-3 font-medium">
                    {hotel.name}
                  </td>

                  <td className="px-4 py-3">
                    {cities.find(
                      (city) =>
                        city.id === hotel.cityId,
                    )?.name ?? hotel.cityId}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.phone ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.address ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    <div className="flex gap-3">
                      <button
                        type="button"
                        onClick={() =>
                          startEditing(hotel)
                        }
                        className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        disabled={
                          deleteMutation.isPending
                        }
                        onClick={() => {
                          if (
                            window.confirm(
                              `Delete hotel "${hotel.name}"?`,
                            )
                          ) {
                            deleteMutation.mutate(
                              hotel.id,
                            );
                          }
                        }}
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
    </section>
  );
}
