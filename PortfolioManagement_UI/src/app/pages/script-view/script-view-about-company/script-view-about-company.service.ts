import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { map, Observable } from 'rxjs';
import { ScriptViewAboutCompanyModel } from './script-view-about-company.model';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewAboutCompanyService {

  constructor(private httpBase: BaseService) { }

  public getRecord(id: number): Observable<ScriptViewAboutCompanyModel> {
    return this.httpBase.get('scriptView/aboutCompany/get/'+id).pipe(
        map((response: ScriptViewAboutCompanyModel) => {
            return response;
        }),
    );
}
}
