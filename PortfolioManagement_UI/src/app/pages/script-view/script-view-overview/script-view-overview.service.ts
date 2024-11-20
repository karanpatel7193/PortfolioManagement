import { Injectable } from '@angular/core';
import { ScriptViewOverviewModel } from './script-view-overview.model';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewOverviewService {

  constructor(private http: HttpService) { }


  public getRecord(id: number): Observable<ScriptViewOverviewModel> {
		return this.http.get('scriptView/Overview/get/' + id).pipe(
			map((response: ScriptViewOverviewModel) => {
				return response;
			})
		)
	}
}
