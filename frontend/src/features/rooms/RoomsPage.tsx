import { useQuery } from "@tanstack/react-query";

import { getApiErrorMessage } from "../../api/errors";
import { getRooms } from "./roomApi";

export function RoomsPage() {
  const {
    data: rooms,
    isLoading,
    isError,
    error,
    refetch,
  } = useQuery({
    queryKey: ["rooms"],
    queryFn: getRooms,
  });

  if (isLoading) {
    return <p>Loading rooms...</p>;
  }

  if (isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Rooms
        </h1>

        <p className="rounded bg-red-50 p-4 text-red-700">
          {getApiErrorMessage(error)}
        </p>

        <button
          type="button"
          onClick={() => refetch()}
          className="cursor-pointer rounded bg-gray-900 px-4 py-2 text-white"
        >
          Try again
        </button>
      </section>
    );
  }

  return (
    <section className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold">
          Rooms
        </h1>

        <p className="mt-1 text-sm text-gray-600">
          Rooms returned by the API.
        </p>
      </div>

      {!rooms || rooms.length === 0 ? (
        <p className="rounded border p-6 text-gray-600">
          No rooms found.
        </p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Room</th>
                <th className="px-4 py-3">Beds</th>
                <th className="px-4 py-3">Price / Day</th>
                <th className="px-4 py-3">Hotel ID</th>
                <th className="px-4 py-3">Reserved Dates</th>
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
                    {room.hotelId}
                  </td>

                  <td className="px-4 py-3">
                    {room.reservedDates?.length ? (
                      <div className="space-y-1">
                        {room.reservedDates.map((date) => (
                          <div key={date}>
                            {new Date(date).toLocaleDateString()}
                          </div>
                        ))}
                      </div>
                    ) : (
                      "-"
                    )}
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
