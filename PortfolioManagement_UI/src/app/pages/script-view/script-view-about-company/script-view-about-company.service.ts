import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ScriptViewAboutCompanyModel } from './script-view-about-company.model';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewAboutCompanyService {

  constructor(private http: HttpService) { }

  public getRecord(id: number): Observable<ScriptViewAboutCompanyModel> {
    return this.http.get('scriptView/aboutCompany/get/'+id).pipe(
        map((response: ScriptViewAboutCompanyModel) => {
            return response;
        }),
    );
}
}
