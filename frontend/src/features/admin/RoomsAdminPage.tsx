import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import { getHotels } from "../hotels/hotelApi";
import { getRooms } from "../rooms/roomApi";
import type { Room } from "../rooms/types";
import {
  createRoom,
  deleteRoom,
  updateRoom,
  type CreateRoomRequest,
} from "./roomApi";

const emptyForm: CreateRoomRequest = {
  roomNumber: "",
  bedNumbers: 1,
  basePricePerDay: 0,
  hotelId: 0,
};

export function RoomsAdminPage() {
  const queryClient = useQueryClient();

  const [form, setForm] =
    useState<CreateRoomRequest>(emptyForm);

  const [editingRoom, setEditingRoom] =
    useState<Room | null>(null);

  const roomsQuery = useQuery({
    queryKey: ["rooms"],
    queryFn: getRooms,
  });

  const hotelsQuery = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  const createMutation = useMutation({
    mutationFn: createRoom,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["rooms"],
      });

      setForm(emptyForm);
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateRoom,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["rooms"],
      });

      setEditingRoom(null);
      setForm(emptyForm);
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteRoom,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["rooms"],
      });
    },
  });

  if (roomsQuery.isLoading || hotelsQuery.isLoading) {
    return <p>Loading...</p>;
  }

  if (roomsQuery.isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Manage Rooms
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(roomsQuery.error)}
        </p>

        <button
          type="button"
          onClick={() => roomsQuery.refetch()}
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
        <h1 className="text-2xl font-semibold">
          Manage Rooms
        </h1>

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

  const rooms = roomsQuery.data ?? [];
  const hotels = hotelsQuery.data ?? [];

  function startEditing(room: Room) {
    setEditingRoom(room);

    setForm({
      roomNumber: room.roomNumber ?? "",
      bedNumbers: room.bedNumbers,
      basePricePerDay: room.basePricePerDay,
      hotelId: room.hotelId,
    });
  }

  function cancelEditing() {
    setEditingRoom(null);
    setForm(emptyForm);
  }

  function submit(event: React.FormEvent) {
    event.preventDefault();

    if (editingRoom) {
      updateMutation.mutate({
        id: editingRoom.id,
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
          Manage Rooms
        </h1>

        <p className="mt-1 text-gray-600">
          Create, update and delete rooms.
        </p>
      </div>

      {mutationError && (
        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(mutationError)}
        </p>
      )}

      <form
        onSubmit={submit}
        className="space-y-4 rounded border text-[#000000] p-6"
      >
        <h2 className="font-semibold">
          {editingRoom
            ? `Edit Room #${editingRoom.id}`
            : "Create Room"}
        </h2>

        <div className="space-y-1">
          <label
            htmlFor="room-number"
            className="block text-sm font-medium"
          >
            Room Number
          </label>

          <input
            id="room-number"
            value={form.roomNumber}
            onChange={(event) =>
              setForm({
                ...form,
                roomNumber: event.target.value,
              })
            }
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="bed-numbers"
            className="block text-sm font-medium"
          >
            Number of Beds
          </label>

          <input
            id="bed-numbers"
            type="number"
            min="1"
            value={form.bedNumbers}
            onChange={(event) =>
              setForm({
                ...form,
                bedNumbers: Number(event.target.value),
              })
            }
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="base-price"
            className="block text-sm font-medium"
          >
            Base Price Per Day
          </label>

          <input
            id="base-price"
            type="text"
            inputMode="decimal"
            value={
              form.basePricePerDay === 0
                ? ""
                : form.basePricePerDay
            }
            onChange={(event) =>
              setForm({
                ...form,
                basePricePerDay:
                  Number(event.target.value) || 0,
              })
            }
            placeholder="e.g. 50.00"
            required
            className="w-full rounded border px-3 py-2"
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="hotel"
            className="block text-sm font-medium"
          >
            Hotel
          </label>

          <select
            id="hotel"
            value={form.hotelId}
            onChange={(event) =>
              setForm({
                ...form,
                hotelId: Number(event.target.value),
              })
            }
            required
            className="cursor-pointer w-full rounded border px-3 py-2"
          >
            <option value={0} disabled>
              Select a hotel
            </option>

            {hotels.map((hotel) => (
              <option
                key={hotel.id}
                value={hotel.id}
              >
                {hotel.name}
              </option>
            ))}
          </select>
        </div>

        <div className="flex gap-3">
          <button
            type="submit"
            disabled={
              createMutation.isPending ||
              updateMutation.isPending
            }
            className="cursor-pointer rounded bg-[#000000] px-4 py-2 text-white disabled:opacity-50"
          >
            {editingRoom
              ? updateMutation.isPending
                ? "Updating..."
                : "Update Room"
              : createMutation.isPending
                ? "Creating..."
                : "Create Room"}
          </button>

          {editingRoom && (
            <button
              type="button"
              onClick={cancelEditing}
              className="cursor-pointer text-red-600  rounded px-4 py-2"
            >
              Cancel
            </button>
          )}
        </div>
      </form>

      {rooms.length === 0 ? (
        <p className="rounded border bg-white p-6">
          No rooms found.
        </p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="min-w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Room</th>
                <th className="px-4 py-3">Beds</th>
                <th className="px-4 py-3">Price / Day</th>
                <th className="px-4 py-3">Hotel</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>

            <tbody>
              {rooms.map((room) => (
                <tr
                  key={room.id}
                  className="border-b last:border-b-0"
                >
                  <td className="px-4 py-3">
                    {room.id}
                  </td>

                  <td className="px-4 py-3 font-medium">
                    {room.roomNumber ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    {room.bedNumbers}
                  </td>

                  <td className="px-4 py-3">
                    {room.basePricePerDay}
                  </td>

                  <td className="px-4 py-3">
                    {hotels.find(
                      (hotel) =>
                        hotel.id === room.hotelId,
                    )?.name ?? room.hotelId}
                  </td>

                  <td className="px-4 py-3">
                    <div className="flex gap-3">
                      <button
                        type="button"
                        onClick={() =>
                          startEditing(room)
                        }
                        className="text-[#0747af] underline"
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
                              `Delete room "${room.roomNumber}"?`,
                            )
                          ) {
                            deleteMutation.mutate(
                              room.id,
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
