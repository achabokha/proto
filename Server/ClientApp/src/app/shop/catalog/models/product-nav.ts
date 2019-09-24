import { Product } from './product';

export interface ProductNav {
    product: Product;
    previousId: string;
    nextId: string;
    index: number;
    count: number;
}