import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { HttpService } from '../../../services/http.service';
import { PmsGridModel, PmsModel, PmsParameterModel } from './pms.model';

@Injectable()
export class PmsService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<PmsModel> {
		return this.http.get('account/pms/getRecord/' + id).pipe(
			map((response: PmsModel) => {
				return response;
			})
		)
	}

	public getForGrid(pmsParameter: PmsParameterModel): Observable<PmsGridModel> {
		return this.http.post('account/Pms/getGridData', pmsParameter).pipe(
			map((response: PmsGridModel) => {
				return response;
			}),
		);
	}

	public save(pms: PmsModel): Observable<number> {
		if (pms.id === 0)
			return this.http.post('account/pms/insert', pms).pipe(
				map((response: number) => {
					return response;
				}),
			);
		else
			return this.http.post('account/pms/update', pms).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}

	public delete(id: number): Observable<void> {
		return this.http.post('account/pms/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}
}
