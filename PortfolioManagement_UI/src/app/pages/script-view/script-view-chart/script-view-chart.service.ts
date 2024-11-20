import { Injectable } from '@angular/core';
import { ScriptViewChartModel, ScriptViewParameterModel } from './script-view-chart.model';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewChartService {

  constructor(private http: HttpService) { }

  public getForChart(scriptViewParameterModel: ScriptViewParameterModel): Observable<ScriptViewChartModel> {
    return this.http.post('scriptView/chart/getForChart',scriptViewParameterModel).pipe(
        map((response: ScriptViewChartModel) => {
            return response;
        }),
    );
    }
}
