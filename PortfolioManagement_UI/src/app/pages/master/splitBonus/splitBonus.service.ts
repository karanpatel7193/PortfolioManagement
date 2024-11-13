import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { HttpService } from '../../../services/http.service';
import { SplitModel, SplitParameterModel, SplitGridModel } from './splitBonus.model';
import { ScriptParameterModel, ScriptMainModel } from '../script/script.model';

@Injectable()
export class SplitService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<SplitModel> {
		return this.http.get('master/splitBonus/getRecord/' + id).pipe(
			map((response: SplitModel) => {
				return response;
			})
		)
	}

	public getForLOV(scriptParameter: ScriptParameterModel): Observable<ScriptMainModel[]> {
		return this.http.post('master/script/getLovValue', scriptParameter).pipe(
			map((response: ScriptMainModel[]) => {
				return response;
			}),
		);
	}

	public getForGrid(splitParameter: SplitParameterModel): Observable<SplitGridModel> {
		return this.http.post('master/splitBonus/getGridData', splitParameter).pipe(
			map((response: SplitGridModel) => {
				return response;
			}),
		);
	}

	public save(split: SplitModel): Observable<number> {
		if (split.id === 0)
			return this.http.post('master/splitBonus/insert', split).pipe(
				map((response: number) => {
					return response;
				}),
			);
		else
			return this.http.post('master/splitBonus/update', split).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}

	public delete(id: number): Observable<void> {
		return this.http.post('master/splitBonus/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}
	
	public splitBonusApply(id:number,isApply: boolean): Observable<number> {
			return this.http.post(`master/splitBonus/apply?id=${id}&IsApply=${isApply}`).pipe(
				map((response: number) => {
					return response;
				}),
			);
		}
	
}
