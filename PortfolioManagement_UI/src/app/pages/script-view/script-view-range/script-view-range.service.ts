import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { BaseService } from 'src/app/services/base.service';
import { ScriptViewRangeChartModel, ScriptViewRangeModel } from './script-view-range.model';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewRangeService {

    constructor(private httpBase: HttpService) { }

    public getForRange(id: number): Observable<ScriptViewRangeChartModel> {
        return this.httpBase.get('scriptView/range/get/' + id).pipe(
            map((response: ScriptViewRangeChartModel) => {
                return response;
            }),
        );
    }
}
