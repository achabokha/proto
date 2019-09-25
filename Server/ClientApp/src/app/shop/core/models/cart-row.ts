import { Product } from './../../catalog/models/product';

export class CartRow {

    constructor(
        public product: Product,
        public quantity: number =  1) {}


   amout(){
       return this.product.price * this.quantity;
   }




}