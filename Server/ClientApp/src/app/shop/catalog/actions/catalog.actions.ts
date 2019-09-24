import { ProductNav } from './../models/product-nav';
import { Book } from './../models/book';
import { Action } from '@ngrx/store';
import { Product } from '../models/product';

export const LOAD_PRODUCTS = '[Catalog] Load';
export const LOAD_PRODUCTS_SUCCESS = '[Catalog] Load Success';
export const LOAD_PRODUCTS_FAIL = '[Catalog] Load Fail';

export const SELECT_PRODUCT = '[Catalog] Select';

export const VIEW_PRODUCT = '[Book] Load';
export const VIEW_PRODUCT_SUCCESS = '[Book] Load Success';
export const VIEW_PRODUCTS_FAIL = '[Book] Load Fail';


export const ADD_PRODUCT_TO_CART = '[Book] add';
export const ADD_PRODUCT_TO_CART_SUCCESS = '[Book] add success'
export const ADD_PRODUCT_TO_CART_FAIL = '[Book] add fail'

/**
 * Load Catalog Actions
 */
export class LoadBooks implements Action {
    readonly type = LOAD_PRODUCTS;
}

export class LoadBooksSuccess implements Action {
    readonly type = LOAD_PRODUCTS_SUCCESS;

    constructor(public payload: Product[]) { }
}

export class LoadBooksFail implements Action {
    readonly type = LOAD_PRODUCTS_FAIL;

    constructor(public payload: any) { }
}

export class SelectBook implements Action {
    readonly type = SELECT_PRODUCT;

    constructor(public payload: any) { }
}

/**
 * view book action
 * 
 * @export
 * @class ViewBook
 * @implements {Action}
 */
export class ViewBook implements Action {
    readonly type = VIEW_PRODUCT;
    constructor(public payload: any) { }

}

export class ViewBookSuccess implements Action {
    readonly type = VIEW_PRODUCT_SUCCESS;

    constructor(public payload: ProductNav) { }
}

export class ViewBookFail implements Action {
    readonly type = VIEW_PRODUCTS_FAIL;

    constructor(public payload: any) { }
}

/**
 * Action Add To Cart, dispatched when the user add a product
 * to the cart
 * 
 * @export
 * @class AddToCart
 * @implements {Action}
 */
export class AddToCart implements Action {
    readonly type = ADD_PRODUCT_TO_CART;
    constructor(public payload: Book) { }
}

export class AddToCartSuccess implements Action {
    readonly type = ADD_PRODUCT_TO_CART_SUCCESS;
    constructor(public payload?: any) { }
}

export class AddToCartFail implements Action {
    readonly type = ADD_PRODUCT_TO_CART_FAIL;
    constructor(public payload?: any) { }
}


// export a new type that represents the ctalog actions 
export type ActionType = LoadBooks | LoadBooksSuccess | LoadBooksFail
    | ViewBook | ViewBookSuccess | ViewBookFail | SelectBook
    | AddToCart | AddToCartSuccess | AddToCartFail;

