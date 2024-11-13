import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { ScriptModel, ScriptMainModel, ScriptParameterModel, ScriptGridModel} from './script.model';
import { HttpService } from '../../../services/http.service';

@Injectable()
export class ScriptService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<ScriptModel> {
		return this.http.get('master/script/getRecord/' + id).pipe(
			map((response: ScriptModel) => {
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

	public getForGrid(scriptParameter: ScriptParameterModel): Observable<ScriptGridModel> {
		return this.http.post('master/script/getGridData', scriptParameter).pipe(
			map((response: ScriptGridModel) => {
				return response;
			}),
		);
	}

	public save(script: ScriptModel): Observable<number> {
		if (script.id === 0)
			return this.http.post('master/script/insert', script).pipe(
				map((response: number) => {
					return response;
				}),
			);
		else
			return this.http.post('master/script/update', script).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}

	public delete(id: number): Observable<void> {
		return this.http.post('master/script/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}
}
