import { Review } from './../models/review.model';
import * as reviewActions from './../actions/review.actions';

export interface State {
    productId: string | null;
    reviews: Review[];
    loading: boolean;
}

export const initialState: State = {
    productId: null,
    reviews: [],
    loading: false
}

export function reducer(state:State = initialState,action:reviewActions.All){
    switch (action.type) {
        case reviewActions.LOAD_REVIEWS:
           return {...state,productId: action.payload,loading:true};
        
        case reviewActions.LOAD_REVIEWS_SUCCESS:
            return {...state,reviews: action.payload.slice(),loading:false};
         
        default:
            return state;
    }
}

