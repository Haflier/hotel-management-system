import { createBrowserRouter } from "react-router-dom";

import App from "../../App";
import { LoginPage } from "../../features/auth/LoginPage";
import { RegisterPage } from "../../features/auth/RegisterPage";
import { DashboardPage } from "../../features/auth/DashboardPage";
import { PublicLayout } from "../layouts/PublicLayout";
import { RequireAuth } from "./RequireAuth";
import { RequireAdmin } from "./RequireAdmin";
import { HotelsPage } from "../../features/hotels/HotelsPage";
import { RoomsPage } from "../../features/rooms/RoomsPage";
import { HotelRoomsPage } from "../../features/hotels/HotelRoomsPage";
import { CreateReservationPage } from "../../features/reservations/CreateReservationPage";
import { MyReservationsPage } from "../../features/reservations/MyReservationsPage";
import { ReservationDetailsPage } from "../../features/reservations/ReservationDetailsPage";
import { ReservationsPage } from "../../features/admin/ReservationsPage";
import { HotelsAdminPage } from "../../features/admin/HotelsAdminPage";
import { RoomsAdminPage } from "../../features/admin/RoomsAdminPage";
import { FoodDrinkPage } from "../../features/orders/FoodDrinkPage";
import { OrderPage } from "../../features/orders/OrderPage";
import { ServicesAdminPage } from "../../features/admin/ServicesAdminPage";
import { RoomServicesAdminPage } from "../../features/admin/RoomServicesAdminPage";
import { OrdersAdminPage } from "../../features/admin/OrdersAdminPage";
import { FactorsAdminPage } from "../../features/admin/FactorsAdminPage";
import { FoodsAdminPage } from "../../features/admin/FoodsAdminPage";
import { DrinksAdminPage } from "../../features/admin/DrinksAdminPage";

export const router = createBrowserRouter([
  {
    element: <PublicLayout />,
    errorElement: <RouterErrorPage />,
    children: [
      {
        path: "/",
        element: <App />,
      },
      {
        path: "/login",
        element: <LoginPage />,
      },
      {
        path: "/register",
        element: <RegisterPage />,
      },
      {
        element: <RequireAuth />,
        children: [
          {
            path: "/dashboard",
            element: <DashboardPage />,
          },
          {
            path: "/hotels",
            element: <HotelsPage />,
          },
          {
            path: "/hotels/:hotelId/rooms",
            element: <HotelRoomsPage />,
          },
          {
            path: "/rooms",
            element: <RoomsPage />,
          },
          {
            path: "/rooms/:roomId/reserve",
            element: <CreateReservationPage />,
          },
          {
            path: "/reservations",
            element: <MyReservationsPage />,
          },
          {
            path: "/reservations/:reservationId",
            element: <ReservationDetailsPage />,
          },
          {
            path: "/reservations/:reservationId/menu",
            element: <FoodDrinkPage />,
          },
          {
            element: <RequireAdmin />,
            children: [
              {
                path: "/admin/reservations",
                element: <ReservationsPage />,
              },
              {
                path: "/admin/hotels",
                element: <HotelsAdminPage />,
              },
              {
                path: "/admin/rooms",
                element: <RoomsAdminPage />,
              },
              {
                path: "/admin/services",
                element: <ServicesAdminPage />,
              },
              {
                path: "/admin/room-services",
                element: <RoomServicesAdminPage />,
              },
              {
                path: "/admin/orders",
                element: <OrdersAdminPage />,
              },
              {
                path: "/admin/factors",
                element: <FactorsAdminPage />,
              },
              {
                path: "/admin/foods",
                element: <FoodsAdminPage />,
              },
              {
                path: "/admin/drinks",
                element: <DrinksAdminPage />,
              },
            ],
          },
          {
            path: "/orders/:orderId",
            element: <OrderPage />,
          },
        ],
      },
    ],
  },
]);

function RouterErrorPage() {
  return (
    <main className="mx-auto max-w-2xl space-y-4 px-6 py-12">
      <h1 className="text-2xl font-semibold">
        Page not found
      </h1>

      <p className="text-gray-600">
        The requested page does not exist.
      </p>

      <a
        href="/"
        className="inline-block rounded bg-gray-900 px-4 py-2 text-white"
      >
        Return home
      </a>
    </main>
  );
}
