import { useQuery } from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import { getReservations } from "./reservationApi";

export function ReservationsPage() {
  const reservationsQuery = useQuery({
    queryKey: ["admin", "reservations"],
    queryFn: getReservations,
  });

  if (reservationsQuery.isLoading) {
    return <p>Loading reservations...</p>;
  }

  if (reservationsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Reservations
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(reservationsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => reservationsQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  const reservations = reservationsQuery.data ?? [];

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Reservations
        </h1>

        <p className="mt-1 text-gray-600">
          All reservations in the system.
        </p>
      </div>

      {reservations.length === 0 ? (
        <p>No reservations found.</p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Room</th>
                <th className="px-4 py-3">User</th>
                <th className="px-4 py-3">Check-in</th>
                <th className="px-4 py-3">Check-out</th>
                <th className="px-4 py-3">Nights</th>
                <th className="px-4 py-3">Price / Day</th>
                <th className="px-4 py-3">Total</th>
              </tr>
            </thead>

            <tbody>
              {reservations.map((reservation) => (
                <tr
                  key={reservation.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {reservation.id}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.roomId}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.apiUserId}
                  </td>

                  <td className="px-4 py-3">
                    {new Date(
                      reservation.checkinDate,
                    ).toLocaleDateString()}
                  </td>

                  <td className="px-4 py-3">
                    {new Date(
                      reservation.checkOutDate,
                    ).toLocaleDateString()}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.totalNights}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.pricePerDay}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.totalPrice}
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
