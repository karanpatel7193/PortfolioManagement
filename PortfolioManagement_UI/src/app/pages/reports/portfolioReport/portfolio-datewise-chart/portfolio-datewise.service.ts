import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { PortfolioDatewiseModel, PortfolioDatewiseParameterModel, PortfolioDatewiseReportModel } from './portfolio-datewise.model';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class PortfolioDatewiseService {

    constructor(private http: HttpService) { }

    public getPortfolioDatewiseReport(portfolioDatewiseParameterModel: PortfolioDatewiseParameterModel): Observable<PortfolioDatewiseReportModel[]> {
    return this.http.post('portfolio/portfolioDateWise/getPortfolioDatewiseReport', portfolioDatewiseParameterModel).pipe(
        map((response: PortfolioDatewiseReportModel[]) => {
            return response;
        })
    );
}
}
