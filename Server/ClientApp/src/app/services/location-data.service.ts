import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { getLocations } from './mock.locations';

@Injectable({
	providedIn: 'root'
})
export class LocationDataService {


	constructor() {

	}

	locationData(days: number): Promise<any[]> {
		return getLocations(days);
	}

}
