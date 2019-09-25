
import { reducer } from './product';
import * as fromProducts from './product';
import { SelectProduct, ViewProduct } from '../actions/catalog.actions';
import { Product, mockProduct } from '../models/product';

describe('ProductReducer', () => {
  const product1 = mockProduct();
  const product2 = { ...product1, id: '2' };
  const product3 = { ...product1, id: '3' };
  const initialState: fromProducts.State = {
    selectedId: product1.id,
    selectedProduct: product1,
    loading: false,
    current: 1,
    total: 3,
    nextId: product2.id,
    previousId: product3.id
  };

  const stateWhenProductIsSelected: fromProducts.State = {
    selectedId: product1.id,
    selectedProduct: null,
    loading: true,
    current: 0,
    total: 0,
    nextId: null,
    previousId: null
  }

  describe('undefined action', () => {
    it('should return the default state', () => {
      const result = reducer(undefined, {} as any);

      expect(result).toEqual(fromProducts.initialState);
    });
  });

  describe('view product action', () => {
    it('should return a new state with the selected product id', () => {
      // create view action with productId = 1
      const viewProduct = new ViewProduct(product1.id);
      const result = reducer(undefined, viewProduct);

      expect(result).toEqual(stateWhenProductIsSelected);
    });
  });

});

