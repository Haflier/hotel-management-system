import { useQuery } from "@tanstack/react-query";
import { Link } from "react-router-dom";

import { getApiErrorMessage } from "../../api/errors";
import { getHotels } from "./hotelApi";

export function HotelsPage() {
  const {
    data: hotels,
    isLoading,
    isError,
    error,
    refetch,
  } = useQuery({
    queryKey: ["hotels"],
    queryFn: getHotels,
  });

  if (isLoading) {
    return <p>Loading hotels...</p>;
  }

  if (isError) {
    return (
      <section className="space-y-4">
        <h1 className="text-2xl font-semibold">
          Hotels
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
          Hotels
        </h1>

        <p className="mt-1 text-sm text-gray-600">
          Hotels returned by the API.
        </p>
      </div>

      {!hotels || hotels.length === 0 ? (
        <p className="rounded border bg-white p-6 text-gray-600">
          No hotels found.
        </p>
      ) : (
        <div className="overflow-x-auto rounded border text-[#000000]">
          <table className="w-full text-left text-sm">
            <thead className="border-b">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Name</th>
                <th className="px-4 py-3">Description</th>
                <th className="px-4 py-3">Phone</th>
                <th className="px-4 py-3">Address</th>
                <th className="px-4 py-3">City ID</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>

            <tbody>
              {hotels.map((hotel) => (
                <tr
                  key={hotel.id}
                  className="border-b last:border-b-0 text-[#000000]"
                >
                  <td className="px-4 py-3">
                    {hotel.id}
                  </td>

                  <td className="px-4 py-3 font-medium">
                    {hotel.name}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.description ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.phone ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.address ?? "-"}
                  </td>

                  <td className="px-4 py-3">
                    {hotel.cityId}
                  </td>

                  <td className="px-4 py-3">
                    <Link
                      to={`/hotels/${hotel.id}/rooms`}
                      className="text-[#fb4100] underline"
                    >
                      View rooms
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
