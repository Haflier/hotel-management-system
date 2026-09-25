export interface CreateReservationRequest {
  checkinDate: string;
  checkOutDate: string;
  roomId: number;
}

export interface Reservation {
  id: number;
  roomId: number;
  apiUserId: string;
  checkinDate: string;
  checkOutDate: string;
  pricePerDay: number;
  totalNights: number;
  totalPrice: number;
  createdAt: string;
}
