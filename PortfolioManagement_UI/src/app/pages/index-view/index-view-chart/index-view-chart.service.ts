import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { map, Observable } from 'rxjs';
import { IndexViewChartModel, IndexViewParameterModel} from './index-view-chart.model';

@Injectable({
  providedIn: 'root'
})
export class IndexViewChartService {

  constructor(private http: BaseService) { }

  public getForChart(indexViewParameterModel: IndexViewParameterModel): Observable<IndexViewChartModel> {
    return this.http.post('IndexView/indexChart/getForIndexChart',indexViewParameterModel).pipe(
        map((response: IndexViewChartModel) => {
            return response;
        }),
    );
    }
}
