import { Observable } from "rxjs";
import { Router } from "@angular/router";

export class BaseService {

    router: Router;

    errorCheck(error: any) {
        console.error("error");
        console.error("status: " + error.status);
        console.error(error);
        if (error.status == 401) {
            this.router.navigateByUrl('/login');
        }
        return Observable.throw(error.json().error || 'Server error')
    }

    checkLoad(load: any) {
        console.log("Post payload: " + JSON.stringify(load));
    }

}
