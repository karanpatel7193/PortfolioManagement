import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { MasterModel, MasterValueModel} from './master.model';
import { HttpService } from '../../../services/http.service';

@Injectable()
export class MasterChildService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<MasterModel> {
		return this.http.get('master/Master/getRecord/' + id).pipe(
			map((response: MasterModel) => {
				return response;
			})
		)
	}
	public getForGrid(): Observable<MasterModel[]> {
		return this.http.post('master/master/getGridData', null).pipe(
			map((response: MasterModel[]) => {
				return response;
			}),
		);
	}

    public save(masterModel: MasterModel, mode:string): Observable<number> {
		if (mode == 'Add')
			return this.http.post('master/master/insert', masterModel).pipe(
				map((response: number) => response)
			);
		else
			return this.http.post('master/master/update', masterModel).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}
	public delete(id: number): Observable<void> {
		return this.http.post('master/master/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}
}
