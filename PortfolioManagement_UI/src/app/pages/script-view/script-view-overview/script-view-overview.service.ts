import { Injectable } from '@angular/core';
import { ScriptViewOverviewModel } from './script-view-overview.model';
import { BaseService } from 'src/app/services/base.service';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewOverviewService {

  constructor(private httpBase: BaseService,private http: HttpService) { }


  public getRecord(id: number): Observable<ScriptViewOverviewModel> {
		return this.http.get('scriptView/Overview/get/' + id).pipe(
			map((response: ScriptViewOverviewModel) => {
				return response;
			})
		)
	}
}
