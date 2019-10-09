import { Product } from './../../catalog/models/product';

export class CartRow {

    constructor(
        public product: Product,
        public quantity: number =  1) {}


   amount(){
       return this.product.price * this.quantity;
   }




}