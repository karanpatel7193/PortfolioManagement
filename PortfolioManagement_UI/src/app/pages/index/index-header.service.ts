import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';
import { HeaderGridModel, HeaderModel } from './index-header.model';

@Injectable({
  providedIn: 'root'
})
export class IndexHeaderService {

  constructor(private httpBase: BaseService,private http: HttpService) { }


public getForGrid(): Observable<HeaderGridModel> {
		return this.http.post('index/header/getForMarquee').pipe(
			map((response: HeaderGridModel) => {
				return response;
			})
		)
	}
	public getForIndex(): Observable<HeaderModel> {
		return this.http.post('index/header/getForIndex').pipe(
			map((response: HeaderModel) => {
				return response;
			})
		)
	}

}
