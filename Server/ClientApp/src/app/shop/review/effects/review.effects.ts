import { mergeMap, map, catchError } from 'rxjs/operators';

// effects are just like any angular service
// Angular service is a singletone class that can be injected into
// any angular concept :
import { Injectable } from '@angular/core';
import { Effect, Actions, ofType } from '@ngrx/effects';

import { Observable } from 'rxjs';
import { of } from 'rxjs';

import * as reviewActions from './../actions/review.actions';
import { HttpClient } from '@angular/common/http';
import { Review } from '../models/review.model';
export type Action = reviewActions.All;

/**
 * Side Effects for the Review Module.
 *
 * @export
 * @class ReviewEffects
 */

@Injectable()
export class ReviewEffects {
  constructor(private http$: HttpClient, private actions$: Actions) {}

  @Effect()
  loadReviews: Observable<Action> = this.actions$.pipe(
    ofType(reviewActions.LOAD_REVIEWS),
    map((action: reviewActions.LoadReviews) => action.payload),
    mergeMap(payload => this.http$.get(`/reviews/${payload}/all`)),
    map((reviews: Review[]) => new reviewActions.LoadReviewsSuccess(reviews))
  );

  @Effect()
  reviewVote: Observable<Action> = this.actions$.pipe(
    ofType(reviewActions.REVIEW_VOTE),
    map((action: reviewActions.ReviewVote) => action.payload),
    mergeMap(payload =>
      of(
        this.http$.post(`/reviews/${payload.bookId}/all/${payload.review.$key}`, {
          upVote: payload.review.upVote + (payload.up ? 1 : 0),
          downVote: payload.review.downVote + (payload.down ? 1 : 0)
        })
      )
    ),
    map(() => new reviewActions.ReviewVoteSuccess()),
    catchError(err =>
      of(new reviewActions.ReviewVoteFail({ error: err.message }))
    )
  );

  @Effect()
  addReview = this.actions$.pipe(
    ofType(reviewActions.ADD_REVIEW),
    map((action: reviewActions.AddReview) => action.payload),
    mergeMap(payload =>
      this.http$.post(`/reviews/${payload.bookId}/all`, payload.review)
    ),
    map(() => new reviewActions.AddReviewSuccess()),
    catchError(err =>
      of(new reviewActions.AddReviewFail({ error: err.message }))
    )
  );

  @Effect()
  removeReview = this.actions$.pipe(
    ofType(reviewActions.REMOVE_REVIEW),
    map((action: reviewActions.RemoveReview) => action.payload),
    mergeMap(payload =>
      this.http$.delete(`/reviews/${payload.bookId}/all/${payload.reviewId}`)
    ),
    map(() => new reviewActions.RemoveReviewSuccess()),
    catchError(err =>
      of(new reviewActions.RemoveReviewFail({ error: err.message }))
    )
  );
}
