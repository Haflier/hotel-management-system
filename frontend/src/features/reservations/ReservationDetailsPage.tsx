import { useQuery } from "@tanstack/react-query";
import { Link, useParams } from "react-router-dom";

import { getApiErrorMessage } from "../../api/errors";
import { getReservation } from "./reservationApi";

export function ReservationDetailsPage() {
  const { reservationId } = useParams();

  const parsedReservationId = Number(reservationId);

  const reservationQuery = useQuery({
    queryKey: ["reservation", parsedReservationId],
    queryFn: () => getReservation(parsedReservationId),
    enabled: Number.isInteger(parsedReservationId),
  });

  if (!Number.isInteger(parsedReservationId)) {
    return <p>Invalid reservation ID.</p>;
  }

  if (reservationQuery.isLoading) {
    return <p>Loading reservation...</p>;
  }

  if (reservationQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Reservation
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(reservationQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => reservationQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </section>
    );
  }

  const reservation = reservationQuery.data;

  if (!reservation) {
    return <p>Reservation not found.</p>;
  }

  return (
    <section className="max-w-xl space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Reservation #{reservation.id}
        </h1>

        <p className="mt-1 text-gray-600">
          Reservation details
        </p>
      </div>

      <div className="rounded border text-[#000000]  p-6">
        <dl className="space-y-4">
          <div>
            <dt className="text-sm text-gray-500">Room ID</dt>
            <dd>{reservation.roomId}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">Check-in</dt>
            <dd>
              {new Date(
                reservation.checkinDate,
              ).toLocaleString()}
            </dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">Check-out</dt>
            <dd>
              {new Date(
                reservation.checkOutDate,
              ).toLocaleString()}
            </dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">
              Price per day
            </dt>
            <dd>{reservation.pricePerDay}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">
              Total nights
            </dt>
            <dd>{reservation.totalNights}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">
              Total price
            </dt>
            <dd>{reservation.totalPrice}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">Created</dt>
            <dd>
              {new Date(
                reservation.createdAt,
              ).toLocaleString()}
            </dd>
          </div>
        </dl>
      </div>

      <div className="flex gap-4">
        <Link
          to={`/reservations/${reservation.id}/menu`}
          className="text-[#fb4100] underline"
        >
          Food & Drinks
        </Link>

        <Link
          to={`/rooms/${reservation.roomId}/reserve`}
          className="text-[#fb4100] underline"
        >
          Back to room
        </Link>
      </div>
    </section>
  );
}
