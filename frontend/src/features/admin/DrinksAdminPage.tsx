import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import { getHotels } from "../hotels/hotelApi";
import {
  createDrink,
  deleteDrink,
  getDrinks,
  updateDrink,
  type Drink,
} from "./drinkApi";

interface DrinkForm {
  name: string;
  description: string;
  price: string;
  meal: string;
  hotelId: string;
}

const emptyForm: DrinkForm = {
  name: "",
  description: "",
  price: "",
  meal: "",
  hotelId: "",
};

const mealOptions = [
  "Breakfast",
  "Lunch",
  "Dinner",
];

export function DrinksAdminPage() {
  const queryClient = useQueryClient();

  const [form, setForm] = useState<DrinkForm>(emptyForm);
  const [editingDrink, setEditingDrink] =
    useState<Drink | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] =
    useState<string | null>(null);

  const drinksQuery = useQuery({
    queryKey: ["admin", "drinks"],
    queryFn: getDrinks,
  });

  const hotelsQuery = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  const createMutation = useMutation({
    mutationFn: createDrink,
    onSuccess: async (drink) => {
      setForm(emptyForm);
      setError(null);

      setSuccessMessage(
        `Drink "${drink.name}" was added to hotel with ID ${drink.hotelId}.`,
      );

      await queryClient.invalidateQueries({
        queryKey: ["admin", "drinks"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateDrink,
    onSuccess: async () => {
      setForm(emptyForm);
      setEditingDrink(null);
      setError(null);
      setSuccessMessage("Drink was updated successfully.");

      await queryClient.invalidateQueries({
        queryKey: ["admin", "drinks"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteDrink,
    onSuccess: async () => {
      setError(null);
      setSuccessMessage("Drink was deleted successfully.");

      await queryClient.invalidateQueries({
        queryKey: ["admin", "drinks"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  if (drinksQuery.isLoading || hotelsQuery.isLoading) {
    return <p>Loading drinks...</p>;
  }

  if (drinksQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">Drinks</h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(drinksQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => drinksQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  if (hotelsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">Drinks</h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(hotelsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => hotelsQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  const drinks = drinksQuery.data ?? [];
  const hotels = hotelsQuery.data ?? [];

  const isSubmitting =
    createMutation.isPending || updateMutation.isPending;

  function handleChange(
    field: keyof DrinkForm,
    value: string,
  ) {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));

    setError(null);
    setSuccessMessage(null);
  }

  function handleEdit(drink: Drink) {
    setEditingDrink(drink);

    setForm({
      name: drink.name,
      description: drink.description,
      price: String(drink.price),
      meal: drink.meal,
      hotelId: String(drink.hotelId),
    });

    setError(null);
    setSuccessMessage(null);
  }

  function handleCancelEdit() {
    setEditingDrink(null);
    setForm(emptyForm);
    setError(null);
    setSuccessMessage(null);
  }

  function handleSubmit(
    event: React.FormEvent<HTMLFormElement>,
  ) {
    event.preventDefault();

    setError(null);
    setSuccessMessage(null);

    const request = {
      name: form.name.trim(),
      description: form.description.trim(),
      price: Number(form.price),
      meal: form.meal,
      hotelId: Number(form.hotelId),
    };

    if (
      !request.name ||
      !request.description ||
      !request.meal ||
      !form.price ||
      !form.hotelId
    ) {
      setError("Please fill in all fields.");
      return;
    }

    if (request.price < 0) {
      setError("Price cannot be negative.");
      return;
    }

    if (request.hotelId <= 0) {
      setError("Hotel ID must be greater than 0.");
      return;
    }

    if (editingDrink) {
      updateMutation.mutate({
        id: editingDrink.id,
        ...request,
      });
    } else {
      createMutation.mutate(request);
    }
  }

  function handleDelete(drink: Drink) {
    const confirmed = window.confirm(
      `Delete "${drink.name}"?`,
    );

    if (!confirmed) {
      return;
    }

    setError(null);
    setSuccessMessage(null);

    deleteMutation.mutate(drink.id);
  }

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">Drinks</h1>

        <p className="mt-1 text-gray-600">
          Manage drink items available in hotels.
        </p>
      </div>

      <form
        onSubmit={handleSubmit}
        className="space-y-4 rounded border text-[#000000] p-6"
      >
        <h2 className="text-lg font-semibold">
          {editingDrink ? "Edit Drink" : "Create Drink"}
        </h2>

        <div className="grid gap-4 md:grid-cols-2">
          <label className="space-y-1">
            <span className="block text-sm font-medium">
              Name
            </span>

            <input
              type="text"
              value={form.name}
              onChange={(event) =>
                handleChange("name", event.target.value)
              }
              className="w-full rounded border px-3 py-2"
            />
          </label>

          <label className="space-y-1">
            <span className="block text-sm font-medium">
              Meal
            </span>

            <select
              value={form.meal}
              onChange={(event) =>
                handleChange("meal", event.target.value)
              }
              className="cursor-pointer w-full rounded border px-3 py-2"
            >
              <option value="">Select meal</option>

              {mealOptions.map((meal) => (
                <option key={meal} value={meal}>
                  {meal}
                </option>
              ))}
            </select>
          </label>

          <label className="space-y-1 md:col-span-2">
            <span className="block text-sm font-medium">
              Description
            </span>

            <input
              type="text"
              value={form.description}
              onChange={(event) =>
                handleChange(
                  "description",
                  event.target.value,
                )
              }
              className="w-full rounded border px-3 py-2"
            />
          </label>

          <label className="space-y-1">
            <span className="block text-sm font-medium">
              Price
            </span>

            <input
              type="number"
              min="0"
              step="0.01"
              value={form.price}
              onChange={(event) =>
                handleChange("price", event.target.value)
              }
              className="w-full rounded border px-3 py-2"
            />
          </label>

          <label className="space-y-1">
            <span className="block text-sm font-medium">
              Hotel
            </span>

            <select
              value={form.hotelId}
              onChange={(event) =>
                handleChange("hotelId", event.target.value)
              }
              className="cursor-pointer w-full rounded border px-3 py-2"
            >
              <option value="">Select hotel</option>

              {hotels.map((hotel) => (
                <option key={hotel.id} value={hotel.id}>
                  {hotel.name} (ID: {hotel.id})
                </option>
              ))}
            </select>
          </label>
        </div>

        {error && (
          <p className="rounded bg-red-50 p-4 text-red-700">
            {error}
          </p>
        )}

        {successMessage && (
          <p className="rounded bg-green-50 p-4 text-green-700">
            {successMessage}
          </p>
        )}

        <div className="flex gap-4">
          <button
            type="submit"
            disabled={isSubmitting}
            className="cursor-pointer rounded bg-black px-4 py-2 text-white disabled:cursor-not-allowed disabled:opacity-50"
          >
            {isSubmitting
              ? "Saving..."
              : editingDrink
                ? "Update Drink"
                : "Create Drink"}
          </button>

          {editingDrink && (
            <button
              type="button"
              onClick={handleCancelEdit}
              className="cursor-pointer text-red-600  px-4 py-2 underline hover:text-gray-600"
            >
              Cancel
            </button>
          )}
        </div>
      </form>

      {drinks.length === 0 ? (
        <p>No drinks found.</p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Name</th>
                <th className="px-4 py-3">Description</th>
                <th className="px-4 py-3">Price</th>
                <th className="px-4 py-3">Meal</th>
                <th className="px-4 py-3">Hotel</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>

            <tbody>
              {drinks.map((drink) => (
                <tr
                  key={drink.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {drink.id}
                  </td>

                  <td className="px-4 py-3">
                    {drink.name}
                  </td>

                  <td className="px-4 py-3">
                    {drink.description}
                  </td>

                  <td className="px-4 py-3">
                    {drink.price}
                  </td>

                  <td className="px-4 py-3">
                    {drink.meal}
                  </td>

                  <td className="px-4 py-3">
                    {drink.hotelId}
                  </td>

                  <td className="px-4 py-3">
                    <div className="flex gap-4">
                      <button
                        type="button"
                        onClick={() => handleEdit(drink)}
                        className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        onClick={() => handleDelete(drink)}
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
    </section>
  );
}
