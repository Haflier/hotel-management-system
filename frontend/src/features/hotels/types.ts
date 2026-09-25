export interface Hotel {
  id: number;
  name: string;
  description?: string | null;
  phone?: string | null;
  address?: string | null;
  cityId: number;
}
