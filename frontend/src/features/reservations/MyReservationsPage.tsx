import { useQuery } from "@tanstack/react-query";
import { Link } from "react-router-dom";

import { getApiErrorMessage } from "../../api/errors";
import { getMyReservations } from "./reservationApi";

export function MyReservationsPage() {
  const reservationsQuery = useQuery({
    queryKey: ["my-reservations"],
    queryFn: getMyReservations,
  });

  if (reservationsQuery.isLoading) {
    return <p>Loading reservations...</p>;
  }

  if (reservationsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          My Reservations
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
          My Reservations
        </h1>

        <p className="mt-1 text-gray-600">
          Your hotel reservations.
        </p>
      </div>

      {reservations.length === 0 ? (
        <div className="rounded border text-[#000000]  p-6">
          <p>You do not have any reservations.</p>

          <Link
            to="/hotels"
            className="mt-4 inline-block text-[#fb4100] underline"
          >
            Find a room
          </Link>
        </div>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">Reservation</th>
                <th className="px-4 py-3">Room</th>
                <th className="px-4 py-3">Check-in</th>
                <th className="px-4 py-3">Check-out</th>
                <th className="px-4 py-3">Total</th>
                <th className="px-4 py-3"></th>
              </tr>
            </thead>

            <tbody>
              {reservations.map((reservation) => (
                <tr
                  key={reservation.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    #{reservation.id}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.roomId}
                  </td>

                  <td className="px-4 py-3">
                    {new Date(
                      reservation.checkinDate,
                    ).toLocaleString()}
                  </td>

                  <td className="px-4 py-3">
                    {new Date(
                      reservation.checkOutDate,
                    ).toLocaleString()}
                  </td>

                  <td className="px-4 py-3">
                    {reservation.totalPrice}
                  </td>

                  <td className="px-4 py-3">
                    <Link
                      to={`/reservations/${reservation.id}`}
                      className="text-[#fb4100] underline"
                    >
                      Details
                    </Link>
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
