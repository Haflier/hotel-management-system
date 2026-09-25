import { zodResolver } from "@hookform/resolvers/zod";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useForm } from "react-hook-form";
import { z } from "zod";

import { getApiErrorMessage } from "../../api/errors";
import { getRooms } from "../rooms/roomApi";
import { createReservation } from "./reservationApi";

const reservationSchema = z
  .object({
    checkinDate: z.string().min(
      1,
      "Check-in date is required",
    ),
    checkOutDate: z.string().min(
      1,
      "Check-out date is required",
    ),
  })
  .refine(
    (data) =>
      new Date(data.checkOutDate) >
      new Date(data.checkinDate),
    {
      message: "Check-out must be after check-in",
      path: ["checkOutDate"],
    },
  );

type ReservationFormData =
  z.infer<typeof reservationSchema>;

function toUtcIsoString(value: string): string {
  return new Date(value).toISOString();
}

function toDateKey(value: string | Date): string {
  const date = new Date(value);

  return [
    date.getFullYear(),
    String(date.getMonth() + 1).padStart(2, "0"),
    String(date.getDate()).padStart(2, "0"),
  ].join("-");
}

function getDatesBetween(
  checkinDate: string,
  checkOutDate: string,
): string[] {
  const dates: string[] = [];

  const current = new Date(checkinDate);
  const checkout = new Date(checkOutDate);

  current.setHours(0, 0, 0, 0);
  checkout.setHours(0, 0, 0, 0);

  while (current < checkout) {
    dates.push(toDateKey(current));
    current.setDate(current.getDate() + 1);
  }

  return dates;
}

export function CreateReservationPage() {
  const { roomId } = useParams();
  const parsedRoomId = Number(roomId);
  const navigate = useNavigate();

  const queryClient = useQueryClient();

  const roomsQuery = useQuery({
    queryKey: ["rooms"],
    queryFn: getRooms,
  });

  const form = useForm<ReservationFormData>({
    resolver: zodResolver(reservationSchema),
  });

  const reservationMutation = useMutation({
    mutationFn: (data: ReservationFormData) =>
      createReservation({
        checkinDate: toUtcIsoString(data.checkinDate),
        checkOutDate: toUtcIsoString(data.checkOutDate),
        roomId: parsedRoomId,
      }),

    onSuccess: async (reservation) => {
      await queryClient.invalidateQueries({
        queryKey: ["rooms"],
      });

      await queryClient.invalidateQueries({
        queryKey: ["my-reservations"],
      });

      form.reset();

      navigate(`/reservations/${reservation.id}`);
    },
  });

  if (!Number.isInteger(parsedRoomId)) {
    return <p>Invalid room ID.</p>;
  }

  if (roomsQuery.isLoading) {
    return <p>Loading room...</p>;
  }

  if (roomsQuery.isError) {
    return (
      <div className="space-y-4">
        <p className="text-red-600">
          {getApiErrorMessage(roomsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => roomsQuery.refetch()}
          className="cursor-pointer underline"
        >
          Retry
        </button>
      </div>
    );
  }

  const room = roomsQuery.data?.find(
    (item) => item.id === parsedRoomId,
  );

  if (!room) {
    return (
      <div className="space-y-4">
        <p>Room not found.</p>

        <Link
          to="/rooms"
          className="text-[#fb4100] underline"
        >
          Back to rooms
        </Link>
      </div>
    );
  }

  const reservedDates = room.reservedDates ?? [];

  function onSubmit(data: ReservationFormData) {
    const requestedDates = getDatesBetween(
      data.checkinDate,
      data.checkOutDate,
    );

    const reservedDateKeys = new Set(
      reservedDates.map((date) => toDateKey(date)),
    );

    const conflictingDate = requestedDates.find(
      (date) => reservedDateKeys.has(date),
    );

    if (conflictingDate) {
      form.setError("checkOutDate", {
        type: "manual",
        message: `Room is already reserved on ${conflictingDate}.`,
      });

      return;
    }

    reservationMutation.mutate(data);
  }

  return (
    <section className="max-w-xl space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Reserve Room {room.roomNumber ?? room.id}
        </h1>

        <p className="text-gray-600">
          Hotel ID: {room.hotelId} · {room.bedNumbers} beds ·{" "}
          {room.basePricePerDay} per day
        </p>
      </div>

      <div className="rounded border text-[#000000]  p-4">
        <h2 className="font-semibold">
          Reserved dates
        </h2>

        {reservedDates.length === 0 ? (
          <p className="mt-2 text-sm text-gray-600">
            No reserved dates.
          </p>
        ) : (
          <ul className="mt-2 list-disc pl-5 text-sm">
            {reservedDates.map((date) => (
              <li key={date}>
                {new Date(date).toLocaleDateString()}
              </li>
            ))}
          </ul>
        )}
      </div>

      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-4 rounded border text-[#000000] p-6"
      >
        <div className="space-y-1">
          <label
            htmlFor="checkinDate"
            className="block font-medium"
          >
            Check-in
          </label>

          <input
            id="checkinDate"
            type="datetime-local"
            {...form.register("checkinDate")}
            className="cursor-pointer w-full rounded border px-3 py-2"
          />

          {form.formState.errors.checkinDate && (
            <p className="text-sm text-red-600">
              {form.formState.errors.checkinDate.message}
            </p>
          )}
        </div>

        <div className="space-y-1">
          <label
            htmlFor="checkOutDate"
            className="block font-medium"
          >
            Check-out
          </label>

          <input
            id="checkOutDate"
            type="datetime-local"
            {...form.register("checkOutDate")}
            className="cursor-pointer w-full rounded border px-3 py-2"
          />

          {form.formState.errors.checkOutDate && (
            <p className="text-sm text-red-600">
              {form.formState.errors.checkOutDate.message}
            </p>
          )}
        </div>

        {reservationMutation.isError && (
          <p className="text-sm text-red-600">
            {getApiErrorMessage(
              reservationMutation.error,
            )}
          </p>
        )}

        <button
          type="submit"
          disabled={reservationMutation.isPending}
          className="cursor-pointer rounded bg-[#000000] px-4 py-2 text-white disabled:opacity-50"
        >
          {reservationMutation.isPending
            ? "Creating..."
            : "Create reservation"}
        </button>
      </form>

      <Link
        to={`/hotels/${room.hotelId}/rooms`}
        className="text-[#fb4100] underline"
      >
        Back to hotel rooms
      </Link>
    </section>
  );
}
