import { Product } from './../models/product';
import * as catalogActions from './../actions/catalog.actions';

export interface State {
    loading: boolean,
    products: Product[]
}

export const initialState: State = {
    loading: false,
    products: []
}

export function reducer(state = initialState, action: catalogActions.ActionType): State {
    switch (action.type) {
        case catalogActions.LOAD_PRODUCTS:
            return { ...state, loading: true }
        case catalogActions.LOAD_PRODUCTS_SUCCESS:
            return { ...state, products: action.payload, loading: false }

        default:
            return state;
    }
}

export const getProducts = (state: State) => state.products;
export const getLoading = (state: State) => state.loading;