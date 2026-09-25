import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Link, useParams } from "react-router-dom";

import { getApiErrorMessage } from "../../api/errors";
import {
  finalizeOrder,
  getOrder,
} from "./orderApi";

export function OrderPage() {
  const { orderId } = useParams();
  const parsedOrderId = Number(orderId);
  const queryClient = useQueryClient();

  const orderQuery = useQuery({
    queryKey: ["order", parsedOrderId],
    queryFn: () => getOrder(parsedOrderId),
    enabled: Number.isInteger(parsedOrderId),
  });

  const finalizeMutation = useMutation({
    mutationFn: () => finalizeOrder(parsedOrderId),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["order", parsedOrderId],
      });
    },
  });

  if (!Number.isInteger(parsedOrderId)) {
    return <p>Invalid order ID.</p>;
  }

  if (orderQuery.isLoading) {
    return <p>Loading order...</p>;
  }

  if (orderQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Order
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(orderQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => orderQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  const order = orderQuery.data;

  if (!order) {
    return <p>Order not found.</p>;
  }

  return (
    <section className="max-w-3xl space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Order #{order.id}
        </h1>

        <p className="mt-1 text-gray-600">
          Reservation #{order.reservationId}
        </p>
      </div>

      {finalizeMutation.isError && (
        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(finalizeMutation.error)}
        </p>
      )}

      {order.items.length === 0 ? (
        <div className="rounded border text-red  p-6">
          <p>Your order is empty.</p>

          <Link
            to={`/reservations/${order.reservationId}/menu`}
            className="mt-4 inline-block text-[#fb4100] underline"
          >
            Browse food & drinks
          </Link>
        </div>
      ) : (
        <>
          <div className="overflow-x-auto rounded border text-[#000000]">
            <table className="min-w-full text-left text-sm">
              <thead className="border-b">
                <tr>
                  <th className="px-4 py-3">Item</th>
                  <th className="px-4 py-3">Quantity</th>
                  <th className="px-4 py-3">Unit price</th>
                  <th className="px-4 py-3">Total</th>
                </tr>
              </thead>

              <tbody>
                {order.items.map((item) => (
                  <tr
                    key={item.id}
                    className="border-b last:border-b-0"
                  >
                    <td className="px-4 py-3">
                      {item.foodName ?? item.drinkName ?? "Unknown item"}
                    </td>

                    <td className="px-4 py-3">
                      {item.quantity}
                    </td>

                    <td className="px-4 py-3">
                      {item.unitPrice}
                    </td>

                    <td className="px-4 py-3">
                      {item.totalPrice}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="rounded border text-[#000000] p-6">
            <div className="flex items-center justify-between">
              <span className="font-semibold">
                Total
              </span>

              <span className="font-semibold">
                {order.totalPrice}
              </span>
            </div>

            <div className="mt-4">
              <span className="text-sm text-gray-500">
                Status
              </span>

              <p>
                {order.isFinalized
                  ? "Finalized"
                  : "Active"}
              </p>
            </div>
          </div>

          {!order.isFinalized && (
            <button
              type="button"
              disabled={finalizeMutation.isPending}
              onClick={() => finalizeMutation.mutate()}
              className="cursor-pointer bg-[#000000]  text-white px-4 py-2 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {finalizeMutation.isPending
                ? "Finalizing..."
                : "Finalize Order"}
            </button>
          )}

          <div className="flex gap-4">
            <Link
              to={`/reservations/${order.reservationId}/menu`}
              className="text-[#fb4100] underline"
            >
              Add more items
            </Link>

            <Link
              to={`/reservations/${order.reservationId}`}
              className="text-[#fb4100] underline"
            >
              Back to reservation
            </Link>
          </div>
        </>
      )}
    </section>
  );
}
