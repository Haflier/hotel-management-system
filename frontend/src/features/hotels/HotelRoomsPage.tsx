import { useQuery } from "@tanstack/react-query";
import { Link, useParams } from "react-router-dom";

import { getApiErrorMessage } from "../../api/errors";
import { getHotels } from "./hotelApi";
import { getRooms } from "../rooms/roomApi";

export function HotelRoomsPage() {
  const { hotelId } = useParams();

  const parsedHotelId = Number(hotelId);

  const hotelsQuery = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  const roomsQuery = useQuery({
    queryKey: ["rooms", parsedHotelId],
    queryFn: getRooms,
  });

  if (hotelsQuery.isLoading || roomsQuery.isLoading) {
    return <p>Loading...</p>;
  }

  if (hotelsQuery.isError) {
    return (
      <p className="rounded bg-red-50 p-4 text-red-700">
        {getApiErrorMessage(hotelsQuery.error)}
      </p>
    );
  }

  if (roomsQuery.isError) {
    return (
      <p className="rounded bg-red-50 p-4 text-red-700">
        {getApiErrorMessage(roomsQuery.error)}
      </p>
    );
  }

  const hotel = hotelsQuery.data?.find(
    (item) => item.id === parsedHotelId,
  );

  if (!hotel) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Hotel not found
        </h1>

        <Link
          to="/hotels"
          className="text-[#fb4100] underline"
        >
          Back to hotels
        </Link>
      </section>
    );
  }

  const rooms = roomsQuery.data?.filter(
    (room) => room.hotelId === parsedHotelId,
  );

  return (
    <section className="space-y-6 text-[#000000]">
      <div>
        <Link
          to="/hotels"
          className="text-sm text-[#fb4100] underline"
        >
          ← Back to hotels
        </Link>

        <h1 className="mt-3 text-2xl font-semibold">
          {hotel.name}
        </h1>

        <p className="mt-1 text-gray-600">
          {hotel.description ?? "No description available."}
        </p>
      </div>

      <div className="rounded border p-6">
        <dl className="grid gap-3 sm:grid-cols-2">
          <div>
            <dt className="text-sm text-gray-500">
              Address
            </dt>
            <dd>{hotel.address ?? "-"}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">
              Phone
            </dt>
            <dd>{hotel.phone ?? "-"}</dd>
          </div>

          <div>
            <dt className="text-sm text-gray-500">
              City ID
            </dt>
            <dd>{hotel.cityId}</dd>
          </div>
        </dl>
      </div>

      <div>
        <h2 className="mb-3 text-xl font-semibold">
          Rooms
        </h2>

        {!rooms || rooms.length === 0 ? (
          <p className="rounded border bg-white p-6 text-gray-600">
            No rooms found for this hotel.
          </p>
        ) : (
          <div className="overflow-x-auto rounded border text-[#000000]">
            <table className="w-full text-left text-sm">
              <thead className="border-b">
                <tr>
                  <th className="px-4 py-3">Room</th>
                  <th className="px-4 py-3">Beds</th>
                  <th className="px-4 py-3">Price / Day</th>
                  <th className="px-4 py-3">Reserved Dates</th>
                  <th>Action</th>
                </tr>
              </thead>

              <tbody>
                {rooms.map((room) => (
                  <tr
                    key={room.id}
                    className="border-b last:border-b-0"
                  >
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

                    <td>
                      <Link
                        to={`/rooms/${room.id}/reserve`}
                        className="text-[#fb4100] underline"
                      >
                        Reserve
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </section>
  );
}
