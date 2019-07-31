import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { finalize, tap } from 'rxjs/operators';
import { MessageService } from '../services/message.service';


@Injectable()
export class NotifyInterceptor implements HttpInterceptor {
    constructor(private messenger: MessageService) { }


    intercept(req: HttpRequest<any>, next: HttpHandler) {


        let notify = this.shellNotify(req);
        if (notify)
            this.messenger.add('', 'processing');

        // extend server response observable with logging
        return next.handle(req)
            .pipe(
                tap(
                    // Succeeds when there is a response; ignore other events
                    event => {
                        if (event instanceof HttpResponse && notify) {
                            this.messenger.add(event.body.message, 'success');
                        }
                    },
                    // Operation failed; error is an HttpErrorResponse
                    error => {
                        if (notify) this.messenger.add(error.error.message, 'error');
                    }
                ),
                // Log when response observable either completes or errors
                finalize(() => {
                })
            );
    }

    shellNotify(req) {
        if (req.method == 'POST') {
            // check exceptions --
            if (req.urlWithParams.indexOf('GetTransactions') != -1) return false;
            return true;
        }
        return false;
    }
}
