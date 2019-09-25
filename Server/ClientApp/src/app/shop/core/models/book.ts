
/**
 * Contract of a Product
 * 
 * @export
 * @interface Product
 */
export interface Product {
    id: string;
    isbn10?: string;
    title: string;
    author: string;
    price: number;
    pages?: number;
    rating?: number;
    votes?: number;
    image?: string;
    description?: string;
}