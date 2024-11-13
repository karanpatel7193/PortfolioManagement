import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { ScriptViewChartModel, ScriptViewParameterModel } from './script-view-chart.model';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewChartService {

  constructor(private http: BaseService) { }

  public getForChart(scriptViewParameterModel: ScriptViewParameterModel): Observable<ScriptViewChartModel> {
    return this.http.post('scriptView/chart/getForChart',scriptViewParameterModel).pipe(
        map((response: ScriptViewChartModel) => {
            return response;
        }),
    );
    }
}
