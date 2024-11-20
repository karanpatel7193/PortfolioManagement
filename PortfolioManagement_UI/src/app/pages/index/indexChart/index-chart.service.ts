import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { IndexChartGridModel, IndexChartModel, IndexChartParameterModel} from './index-chart.model';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class IndexChartService {

  constructor(private http: HttpService) { }

  public getForIndexChart(indexChartParameterModel: IndexChartParameterModel): Observable<IndexChartGridModel> {
    return this.http.post('index/chart/getIndexChart',indexChartParameterModel).pipe(
        map((response: IndexChartGridModel) => {
            return response;
        }),
    );
    }
}
