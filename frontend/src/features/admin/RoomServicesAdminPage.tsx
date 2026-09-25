import { useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
} from "@tanstack/react-query";

import { getRooms } from "../rooms/roomApi";
import { getServices } from "./serviceApi";
import {
  assignServiceToRoom,
  removeServiceFromRoom,
} from "./serviceApi";
import { getRoomDetails } from "./roomServiceApi";

export function RoomServicesAdminPage() {
  const queryClient = useQueryClient();

  const [selectedRoomId, setSelectedRoomId] =
    useState<number | null>(null);

  const roomsQuery = useQuery({
    queryKey: ["rooms"],
    queryFn: getRooms,
  });

  const servicesQuery = useQuery({
    queryKey: ["admin", "services"],
    queryFn: getServices,
  });

  const roomDetailsQuery = useQuery({
    queryKey: ["admin", "room-details", selectedRoomId],
    queryFn: () => getRoomDetails(selectedRoomId!),
    enabled: selectedRoomId !== null,
  });

  const assignMutation = useMutation({
    mutationFn: ({
      roomId,
      serviceId,
    }: {
      roomId: number;
      serviceId: number;
    }) => assignServiceToRoom(roomId, serviceId),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [
          "admin",
          "room-details",
          selectedRoomId,
        ],
      });
    },
  });

  const removeMutation = useMutation({
    mutationFn: ({
      roomId,
      serviceId,
    }: {
      roomId: number;
      serviceId: number;
    }) => removeServiceFromRoom(roomId, serviceId),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: [
          "admin",
          "room-details",
          selectedRoomId,
        ],
      });
    },
  });

  if (roomsQuery.isLoading || servicesQuery.isLoading) {
    return <p>Loading...</p>;
  }

  if (roomsQuery.isError || servicesQuery.isError) {
    return <p>Failed to load rooms or services.</p>;
  }

  const rooms = roomsQuery.data ?? [];
  const services = servicesQuery.data ?? [];
  const roomDetails = roomDetailsQuery.data;

  const assignedServiceIds = new Set(
    roomDetails?.activeServices.map(
      (service) => service.id,
    ) ?? [],
  );

  const assignedServices = services.filter((service) =>
    assignedServiceIds.has(service.id),
  );

  const availableServices = services.filter(
    (service) => !assignedServiceIds.has(service.id),
  );

  return (
    <div className="space-y-6 text-[#000000]">
      <div>
        <h1 className="text-2xl font-bold">
          Room Services
        </h1>

        <p className="text-sm text-gray-600">
          Assign and remove services from rooms.
        </p>
      </div>

      <div className="space-y-1">
        <label
          htmlFor="room"
          className="block text-sm font-medium"
        >
          Room
        </label>

        <select
          id="room"
          value={selectedRoomId ?? ""}
          onChange={(event) =>
            setSelectedRoomId(
              event.target.value
                ? Number(event.target.value)
                : null,
            )
          }
          className="cursor-pointer w-full rounded border px-3 py-2"
        >
          <option value="">Select a room</option>

          {rooms.map((room) => (
            <option key={room.id} value={room.id}>
              Room {room.roomNumber} — Hotel #{room.hotelId}
            </option>
          ))}
        </select>
      </div>

      {selectedRoomId !== null && (
        <>
          {roomDetailsQuery.isLoading && (
            <p>Loading room services...</p>
          )}

          {roomDetailsQuery.isError && (
            <p>Failed to load room services.</p>
          )}

          {roomDetailsQuery.isSuccess && roomDetails && (
            <div className="space-y-6">
              <section>
                <h2 className="mb-3 text-lg font-semibold">
                  Assigned Services
                </h2>

                {assignedServices.length === 0 ? (
                  <p className="text-sm text-gray-600">
                    No services assigned to this room.
                  </p>
                ) : (
                  <div className="space-y-2">
                    {assignedServices.map((service) => (
                      <div
                        key={service.id}
                        className="flex items-center justify-between rounded border p-3"
                      >
                        <div>
                          <p className="font-medium">
                            {service.name}
                          </p>

                          <p className="text-sm text-gray-600">
                            {service.description}
                          </p>

                          <p className="text-sm">
                            Price: {service.price}
                          </p>
                        </div>

                        <button
                          type="button"
                          onClick={() =>
                            removeMutation.mutate({
                              roomId: selectedRoomId,
                              serviceId: service.id,
                            })
                          }
                          disabled={removeMutation.isPending}
                          className="cursor-pointer text-red-600 underline hover:text-red-800 disabled:cursor-not-allowed disabled:opacity-50"
                        >
                          Delete
                        </button>
                      </div>
                    ))}
                  </div>
                )}
              </section>

              <section>
                <h2 className="mb-3 text-lg font-semibold">
                  Available Services
                </h2>

                {availableServices.length === 0 ? (
                  <p className="text-sm text-gray-600">
                    All services are assigned to this room.
                  </p>
                ) : (
                  <div className="space-y-2">
                    {availableServices.map((service) => (
                      <div
                        key={service.id}
                        className="flex items-center justify-between rounded border p-3"
                      >
                        <div>
                          <p className="font-medium">
                            {service.name}
                          </p>

                          <p className="text-sm text-gray-600">
                            {service.description}
                          </p>

                          <p className="text-sm">
                            Price: {service.price}
                          </p>
                        </div>

                        <button
                          type="button"
                          onClick={() =>
                            assignMutation.mutate({
                              roomId: selectedRoomId,
                              serviceId: service.id,
                            })
                          }
                          disabled={assignMutation.isPending}
                          className="cursor-pointer text-[#0747af] underline hover:text-blue-800 disabled:cursor-not-allowed disabled:opacity-50"
                        >
                          Assign
                        </button>
                      </div>
                    ))}
                  </div>
                )}
              </section>
            </div>
          )}
        </>
      )}
    </div>
  );
}
