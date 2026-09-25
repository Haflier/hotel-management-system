import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { deleteOrder, getAdminOrders } from "./orderApi";
import "./OrdersAdminPage.css";

export function OrdersAdminPage() {
  const queryClient = useQueryClient();

  const ordersQuery = useQuery({
    queryKey: ["admin", "orders"],
    queryFn: getAdminOrders,
  });

  const deleteMutation = useMutation({
    mutationFn: deleteOrder,
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ["admin", "orders"],
      });
    },
  });

  if (ordersQuery.isLoading) {
    return <p>Loading orders...</p>;
  }

  if (ordersQuery.isError) {
    return <p>Failed to load orders.</p>;
  }

  const orders = ordersQuery.data ?? [];

  return (
    <section className="orders-admin text-[#000000]">
      <h1>Orders</h1>

      {orders.length === 0 ? (
        <p>No orders found.</p>
      ) : (
        <div className="orders-table-wrapper">
          <table className="orders-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>User</th>
                <th>Reservation</th>
                <th>Created</th>
                <th>Items</th>
                <th>Total</th>
                <th>Finalized</th>
                <th>Actions</th>
              </tr>
            </thead>

            <tbody>
              {orders.map((order) => (
                <tr key={order.id}>
                  <td>{order.id}</td>

                  <td className="orders-user-id">
                    {order.apiUserId}
                  </td>

                  <td>{order.reservationId}</td>

                  <td>
                    {new Date(order.createdAt).toLocaleString()}
                  </td>

                  <td>
                    <ul className="orders-items">
                      {order.items.map((item) => (
                        <li key={item.id}>
                          <span className="orders-item-name">
                            {item.foodName ??
                              item.drinkName ??
                              "Unknown item"}
                          </span>

                          <span className="orders-item-quantity">
                            × {item.quantity}
                          </span>

                          <span className="orders-item-price">
                            {item.totalPrice}
                          </span>
                        </li>
                      ))}
                    </ul>
                  </td>

                  <td>{order.totalPrice}</td>

                  <td>
                    {order.isFinalized ? "Yes" : "No"}
                  </td>

                  <td className="orders-actions">
                    <button
                      type="button"
                      className="delete-order-button"
                      onClick={() => {
                        if (
                          window.confirm(
                            `Delete order #${order.id}?`,
                          )
                        ) {
                          deleteMutation.mutate(order.id);
                        }
                      }}
                      disabled={deleteMutation.isPending}
                    >
                      Delete
                    </button>
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
