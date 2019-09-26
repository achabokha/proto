import { createSelector, createFeatureSelector } from "@ngrx/store";
import * as fromProduct from "./product";
import * as fromCollection from "./collection";
import * as fromRoot from "../../core/reducers";

export interface CatalogState {
    product: fromProduct.State;
    collection: fromCollection.State;
}

export interface State extends fromRoot.State {
    "catalog": CatalogState;
}

// Catalog feature reducers
export const reducers = {
    product: fromProduct.reducer,
    collection: fromCollection.reducer,
};

// Select the catalog slice feature from the store
export const getCatalogState = createFeatureSelector<CatalogState>("catalog");


export const getCollection = createSelector(
    getCatalogState,
    (state: CatalogState) => state.collection
);


export const getCatalogContent = createSelector(getCollection,
    (state: fromCollection.State) => state.products
);

export const getCatalogLoading = createSelector(getCollection,
    (state: fromCollection.State) => state.loading
); 

export const getProduct = createSelector(
    getCatalogState,
    (state: CatalogState) => state.product
);

// Select the selectedProduct slice from the Store
export const getSelectedProduct = createSelector(
    getProduct,
    (state: fromProduct.State) => state.selectedProduct
);

export const getProductLoading = createSelector(
    getProduct,
    (state: fromProduct.State) => state.loading
);

export const getCurrent = createSelector(
    getProduct,
    (state: fromProduct.State) => state.current
);

export const getTotal = createSelector(
    getProduct,
    (state: fromProduct.State) => state.total
);

export const getNextId = createSelector(
    getProduct,
    (state: fromProduct.State) => state.nextId
);

export const getPreviousId = createSelector(
    getProduct,
    (state: fromProduct.State) => state.previousId
);