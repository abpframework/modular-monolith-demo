
export interface Order
{
  id: string;
  state: number;
  totalPrice: number;
  shippingAddress: string;
  cargoNo: string;
  creationTime: string;
  orderLines: OrderLine[];
}

export interface OrderLine {
  id: string;
  creationTime: string;
  creatorId: string;
  lastModificationTime: string;
  lastModifierId: string;
  isDeleted: true,
  deleterId: string;
  deletionTime: string;
  orderId: string;
  productId: string;
  name: string;
  price: number;
  amount: number;
  totalPrice: number;
}
export type CreateOrder = {
  products: {productId: string, amount: number}[];
  shippingAddress: string;
}
export type OrderList = {items: Order[]};
