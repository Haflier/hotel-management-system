import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import { getHotels } from "../hotels/hotelApi";
import { getApiErrorMessage } from "../../api/errors";
import {
  createFood,
  deleteFood,
  getFoods,
  updateFood,
  type Food,
} from "./foodApi";

interface FoodForm {
  name: string;
  description: string;
  price: string;
  meal: string;
  hotelId: string;
}

const emptyForm: FoodForm = {
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
  "Starter",
  "Dessert"
];

export function FoodsAdminPage() {
  const queryClient = useQueryClient();

  const [form, setForm] = useState<FoodForm>(emptyForm);
  const [editingFood, setEditingFood] = useState<Food | null>(
    null,
  );
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<
    string | null
  >(null);

  const foodsQuery = useQuery({
    queryKey: ["admin", "foods"],
    queryFn: getFoods,
  });

  const hotelsQuery = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  const createMutation = useMutation({
    mutationFn: createFood,
    onSuccess: async (food) => {
      setForm(emptyForm);
      setError(null);

      setSuccessMessage(
        `Food "${food.name}" was added to hotel with ID ${food.hotelId}.`,
      );

      await queryClient.invalidateQueries({
        queryKey: ["admin", "foods"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateFood,
    onSuccess: async () => {
      setForm(emptyForm);
      setEditingFood(null);
      setError(null);
      setSuccessMessage("Food was updated successfully.");

      await queryClient.invalidateQueries({
        queryKey: ["admin", "foods"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteFood,
    onSuccess: async () => {
      setError(null);
      setSuccessMessage("Food was deleted successfully.");

      await queryClient.invalidateQueries({
        queryKey: ["admin", "foods"],
      });
    },
    onError: (mutationError) => {
      setSuccessMessage(null);
      setError(getApiErrorMessage(mutationError));
    },
  });

  if (foodsQuery.isLoading || hotelsQuery.isLoading) {
    return <p>Loading foods...</p>;
  }

  if (foodsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">Foods</h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(foodsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => foodsQuery.refetch()}
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
        <h1 className="text-2xl font-semibold">Foods</h1>

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

  const foods = foodsQuery.data ?? [];

  const hotels = hotelsQuery.data ?? [];

  const isSubmitting =
    createMutation.isPending || updateMutation.isPending;

  function handleChange(
    field: keyof FoodForm,
    value: string,
  ) {
    setForm((current) => ({
      ...current,
      [field]: value,
    }));

    setError(null);
    setSuccessMessage(null);
  }

  function handleEdit(food: Food) {
    setEditingFood(food);

    setForm({
      name: food.name,
      description: food.description,
      price: String(food.price),
      meal: food.meal,
      hotelId: String(food.hotelId),
    });

    setError(null);
    setSuccessMessage(null);
  }

  function handleCancelEdit() {
    setEditingFood(null);
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

    if (editingFood) {
      updateMutation.mutate({
        id: editingFood.id,
        ...request,
      });
    } else {
      createMutation.mutate(request);
    }
  }

  function handleDelete(food: Food) {
    const confirmed = window.confirm(
      `Delete "${food.name}"?`,
    );

    if (!confirmed) {
      return;
    }

    setError(null);
    setSuccessMessage(null);

    deleteMutation.mutate(food.id);
  }

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">Foods</h1>

        <p className="mt-1 text-gray-600">
          Manage food items available in hotels.
        </p>
      </div>

      <form
        onSubmit={handleSubmit}
        className="space-y-4 rounded border text-[#000000] p-6"
      >
        <h2 className="text-lg font-semibold">
          {editingFood ? "Edit Food" : "Create Food"}
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
              : editingFood
                ? "Update Food"
                : "Create Food"}
          </button>

          {editingFood && (
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

      {foods.length === 0 ? (
        <p>No foods found.</p>
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
              {foods.map((food) => (
                <tr
                  key={food.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {food.id}
                  </td>

                  <td className="px-4 py-3">
                    {food.name}
                  </td>

                  <td className="px-4 py-3">
                    {food.description}
                  </td>

                  <td className="px-4 py-3">
                    {food.price}
                  </td>

                  <td className="px-4 py-3">
                    {food.meal}
                  </td>

                  <td className="px-4 py-3">
                    {food.hotelId}
                  </td>

                  <td className="px-4 py-3">
                    <div className="flex gap-4">
                      <button
                        type="button"
                        onClick={() => handleEdit(food)}
                        className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Edit
                      </button>

                      <button
                        type="button"
                        onClick={() => handleDelete(food)}
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
