import { createFeatureSelector, createSelector } from '@ngrx/store';

import * as fromReview from './review';
import * as fromRoot from './../../core/reducers'

export interface ReviewState {
    review: fromReview.State
}

export interface State extends fromRoot.State {
    'productReview': ReviewState;
}

export const reducers = {
    review: fromReview.reducer
}

// Select the review slice feature from the state
export const getReviewState = createFeatureSelector<ReviewState>('productReview');

export const getReview = createSelector(getReviewState,
    (state: ReviewState) => state.review);

export const getListOfReviews = createSelector(getReview,
    (state: fromReview.State) => state.reviews);

export const getProductSubjectToReview = createSelector(getReview,
    (state: fromReview.State) => state.productId);

export const getLoading = createSelector(getReview,
    (state: fromReview.State) => state.loading);