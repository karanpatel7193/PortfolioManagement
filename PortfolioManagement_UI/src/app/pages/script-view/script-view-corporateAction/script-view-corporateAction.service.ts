import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';
import { ScriptViewCorporateActionModel } from './script-view-corporateAction.model';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewCorporateActionService {

  constructor(private http: HttpService) { }


  public getRecordBonus(id: number): Observable<ScriptViewCorporateActionModel[]> {
		return this.http.get('scriptView/CorporateAction/bonus/' + id).pipe(
			map((response: ScriptViewCorporateActionModel[]) => {
				return response;
			})
		)
	}
	public getRecordSplit(id: number): Observable<ScriptViewCorporateActionModel[]> {
		return this.http.get('scriptView/CorporateAction/split/' + id).pipe(
			map((response: ScriptViewCorporateActionModel[]) => {
				return response;
			})
		)
	}
	
}
