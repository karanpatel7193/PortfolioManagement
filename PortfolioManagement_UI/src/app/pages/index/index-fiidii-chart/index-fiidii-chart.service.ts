import { Injectable } from '@angular/core';
import { IndexFiiDiiChartModel, IndexFiiDiiParameterModel } from './index-fiidii-chart.model';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class IndexFiidiiChartService {

  constructor(private http: HttpService) { }

  public getForChart(indexFiiDiiParameterModel: IndexFiiDiiParameterModel): Observable<IndexFiiDiiChartModel> {
    return this.http.post('transaction/indexfiidii/getChartData',indexFiiDiiParameterModel).pipe(
        map((response: IndexFiiDiiChartModel) => {
            return response;
        }),
    );
    }
}
