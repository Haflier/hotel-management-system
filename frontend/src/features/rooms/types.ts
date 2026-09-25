export interface Room {
  id: number;
  roomNumber: string | null;
  bedNumbers: number;
  reservedDates: string[] | null;
  basePricePerDay: number;
  hotelId: number;
}
