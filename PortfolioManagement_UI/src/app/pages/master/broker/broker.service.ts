import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { HttpService } from '../../../services/http.service';
import { BrokerModel, BrokerMainModel, BrokerParameterModel,BrokerGridModel } from './broker.model';

@Injectable()
export class BrokerService {
	constructor(private http: HttpService,) {
	}

	public getRecord(id: number): Observable<BrokerModel> {
		return this.http.get('master/broker/getRecord/' + id).pipe(
			map((response: BrokerModel) => {
				return response;
			})
		)
	}

	public getForLOV(brokerParameter: BrokerParameterModel): Observable<BrokerMainModel[]> {
		return this.http.post('master/broker/getLovValue', brokerParameter).pipe(
			map((response: BrokerMainModel[]) => {
				return response;
			}),
		);
	}

	public getForGrid(brokerParameter: BrokerParameterModel): Observable<BrokerGridModel> {
		return this.http.post('master/broker/getGridData', brokerParameter).pipe(
			map((response: BrokerGridModel) => {
				return response;
			}),
		);
	}

	public save(broker: BrokerModel, mode: string): Observable<number> {
		if (mode === 'Add') {
			return this.http.post('master/broker/insert', broker).pipe(
				map((response: number) => {
					return response;
				}),
			);
		} else {
			return this.http.post('master/broker/update', broker).pipe(
				map((response: number) => {
					return response;
				}),
			);
		}
	}
	

	

	public delete(id: number): Observable<void> {
		return this.http.post('master/broker/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			})
		);
	}
}
