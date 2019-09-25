import { ProductNav } from './../models/product-nav';
import { Product } from './../models/product';
import { Action } from '@ngrx/store';

export const LOAD_PRODUCTS = '[Catalog] Load';
export const LOAD_PRODUCTS_SUCCESS = '[Catalog] Load Success';
export const LOAD_PRODUCTS_FAIL = '[Catalog] Load Fail';

export const SELECT_PRODUCT = '[Catalog] Select';

export const VIEW_PRODUCT = '[Product] Load';
export const VIEW_PRODUCT_SUCCESS = '[Product] Load Success';
export const VIEW_PRODUCTS_FAIL = '[Product] Load Fail';


export const ADD_PRODUCT_TO_CART = '[Product] add';
export const ADD_PRODUCT_TO_CART_SUCCESS = '[Product] add success'
export const ADD_PRODUCT_TO_CART_FAIL = '[Product] add fail'

/**
 * Load Catalog Actions
 */
export class LoadProducts implements Action {
    readonly type = LOAD_PRODUCTS;
}

export class LoadProductsSuccess implements Action {
    readonly type = LOAD_PRODUCTS_SUCCESS;

    constructor(public payload: Product[]) { }
}

export class LoadProductsFail implements Action {
    readonly type = LOAD_PRODUCTS_FAIL;

    constructor(public payload: any) { }
}

export class SelectProduct implements Action {
    readonly type = SELECT_PRODUCT;

    constructor(public payload: any) { }
}

/**
 * view product action
 * 
 * @export
 * @class ViewProduct
 * @implements {Action}
 */
export class ViewProduct implements Action {
    readonly type = VIEW_PRODUCT;
    constructor(public payload: any) { }

}

export class ViewProductSuccess implements Action {
    readonly type = VIEW_PRODUCT_SUCCESS;

    constructor(public payload: ProductNav) { }
}

export class ViewProductFail implements Action {
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
    constructor(public payload: Product) { }
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
export type ActionType = LoadProducts | LoadProductsSuccess | LoadProductsFail
    | ViewProduct | ViewProductSuccess | ViewProductFail | SelectProduct
    | AddToCart | AddToCartSuccess | AddToCartFail;

