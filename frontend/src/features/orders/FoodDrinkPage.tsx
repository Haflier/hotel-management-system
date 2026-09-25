import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Link, useParams } from "react-router-dom";
import { useState } from "react";

import { getApiErrorMessage } from "../../api/errors";
import { getReservation } from "../reservations/reservationApi";
import {
  createOrderItem,
  getDrinksForReservation,
  getFoodsForReservation,
} from "./orderApi";

export function FoodDrinkPage() {
  const { reservationId } = useParams();
  const parsedReservationId = Number(reservationId);
  const queryClient = useQueryClient();

  const [orderId, setOrderId] = useState<number | null>(null);

  const reservationQuery = useQuery({
    queryKey: ["reservation", parsedReservationId],
    queryFn: () => getReservation(parsedReservationId),
    enabled: Number.isInteger(parsedReservationId),
  });

  const foodsQuery = useQuery({
    queryKey: ["foods", parsedReservationId],
    queryFn: () => getFoodsForReservation(parsedReservationId),
    enabled: Number.isInteger(parsedReservationId),
  });

  const drinksQuery = useQuery({
    queryKey: ["drinks", parsedReservationId],
    queryFn: () => getDrinksForReservation(parsedReservationId),
    enabled: Number.isInteger(parsedReservationId),
  });

  const addItemMutation = useMutation({
    mutationFn: createOrderItem,
    onSuccess: (item) => {
      setOrderId(item.orderId);

      queryClient.invalidateQueries({
        queryKey: ["order", item.orderId],
      });
    },
  });

  if (!Number.isInteger(parsedReservationId)) {
    return <p>Invalid reservation ID.</p>;
  }

  if (
    reservationQuery.isLoading ||
    foodsQuery.isLoading ||
    drinksQuery.isLoading
  ) {
    return <p>Loading menu...</p>;
  }

  if (reservationQuery.isError) {
    return (
      <p className="rounded bg-red-50 p-4 text-red-700">
        {getApiErrorMessage(reservationQuery.error)}
      </p>
    );
  }

  if (foodsQuery.isError) {
    return (
      <p className="rounded bg-red-50 p-4 text-red-700">
        {getApiErrorMessage(foodsQuery.error)}
      </p>
    );
  }

  if (drinksQuery.isError) {
    return (
      <p className="rounded bg-red-50 p-4 text-red-700">
        {getApiErrorMessage(drinksQuery.error)}
      </p>
    );
  }

  const foods = foodsQuery.data ?? [];
  const drinks = drinksQuery.data ?? [];

  function addFood(foodId: number) {
    addItemMutation.mutate({
      reservationId: parsedReservationId,
      foodId,
      quantity: 1,
    });
  }

  function addDrink(drinkId: number) {
    addItemMutation.mutate({
      reservationId: parsedReservationId,
      drinkId,
      quantity: 1,
    });
  }

  return (
    <section className="space-y-8">
      <div>
        <h1 className="text-2xl font-semibold">
          Food & Drinks
        </h1>

        <p className="mt-1 text-gray-600">
          Reservation #{parsedReservationId}
        </p>
      </div>

      {addItemMutation.isError && (
        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(addItemMutation.error)}
        </p>
      )}

      {addItemMutation.isSuccess && orderId && (
        <div className="rounded border text-[#000000]  p-4">
          <p>Item added to your order.</p>

          <Link
            to={`/orders/${orderId}`}
            className="mt-2 inline-block text-[#fb4100] underline"
          >
            View order
          </Link>
        </div>
      )}

      <div className="space-y-4">
        <h2 className="text-xl font-semibold">
          Food
        </h2>

        {foods.length === 0 ? (
          <p>No food available.</p>
        ) : (
          <div className="overflow-x-auto rounded border text-[#000000]">
            <table className="min-w-full text-left text-sm">
              <thead className="border-b">
                <tr>
                  <th className="px-4 py-3">Name</th>
                  <th className="px-4 py-3">Description</th>
                  <th className="px-4 py-3">Meal</th>
                  <th className="px-4 py-3">Price</th>
                  <th className="px-4 py-3"></th>
                </tr>
              </thead>

              <tbody>
                {foods.map((food) => (
                  <tr
                    key={food.id}
                    className="border-b last:border-b-0"
                  >
                    <td className="px-4 py-3">{food.name}</td>
                    <td className="px-4 py-3">{food.description}</td>
                    <td className="px-4 py-3">{food.meal}</td>
                    <td className="px-4 py-3">{food.price}</td>
                    <td className="px-4 py-3">
                      <button
                        type="button"
                        disabled={addItemMutation.isPending}
                        onClick={() => addFood(food.id)}
                        className="cursor-pointer text-[#fb4100]  underline disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Add
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div className="space-y-4">
        <h2 className="text-xl font-semibold">
          Drinks
        </h2>

        {drinks.length === 0 ? (
          <p>No drinks available.</p>
        ) : (
          <div className="overflow-x-auto rounded border text-[#000000]">
            <table className="min-w-full text-left text-sm">
              <thead className="border-b">
                <tr>
                  <th className="px-4 py-3">Name</th>
                  <th className="px-4 py-3">Description</th>
                  <th className="px-4 py-3">Meal</th>
                  <th className="px-4 py-3">Price</th>
                  <th className="px-4 py-3"></th>
                </tr>
              </thead>

              <tbody>
                {drinks.map((drink) => (
                  <tr
                    key={drink.id}
                    className="border-b last:border-b-0"
                  >
                    <td className="px-4 py-3">{drink.name}</td>
                    <td className="px-4 py-3">{drink.description}</td>
                    <td className="px-4 py-3">{drink.meal}</td>
                    <td className="px-4 py-3">{drink.price}</td>
                    <td className="px-4 py-3">
                      <button
                        type="button"
                        disabled={addItemMutation.isPending}
                        onClick={() => addDrink(drink.id)}
                        className="cursor-pointer text-[#fb4100]  underline disabled:cursor-not-allowed disabled:opacity-50"
                      >
                        Add
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <Link
        to={`/reservations/${parsedReservationId}`}
        className="text-[#fb4100] underline"
      >
        Back to reservation
      </Link>
    </section>
  );
}
