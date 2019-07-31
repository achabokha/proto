"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/* "Barrel" of Http Interceptors */
var http_1 = require("@angular/common/http");
var logging_interceptor_1 = require("./logging-interceptor");
var notify_interceptor_1 = require("./notify-interceptor");
//import { AuthInterceptor } from './auth-interceptor';
//import { CachingInterceptor } from './caching-interceptor';
//import { EnsureHttpsInterceptor } from './ensure-https-interceptor';
//import { NoopInterceptor } from './noop-interceptor';
//import { TrimNameInterceptor } from './trim-name-interceptor';
//import { UploadInterceptor } from './upload-interceptor';
/** Http interceptor providers in outside-in order */
exports.httpInterceptorProviders = [
    { provide: http_1.HTTP_INTERCEPTORS, useClass: logging_interceptor_1.LoggingInterceptor, multi: true },
    { provide: http_1.HTTP_INTERCEPTORS, useClass: notify_interceptor_1.NotifyInterceptor, multi: true },
];
exports.httpInterceptorProvidersProd = [
    //{ provide: HTTP_INTERCEPTORS, useClass: LoggingInterceptor, multi: true },
    { provide: http_1.HTTP_INTERCEPTORS, useClass: notify_interceptor_1.NotifyInterceptor, multi: true },
];
//# sourceMappingURL=index.js.map