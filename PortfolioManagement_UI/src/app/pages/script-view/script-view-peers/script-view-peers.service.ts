import { Injectable } from '@angular/core';
import { ScriptViewPeersModel } from './script-view-peers.model';
import { map, Observable } from 'rxjs';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewPeersService {

  constructor(private http: HttpService) { }

  public getRecord(id: number): Observable<ScriptViewPeersModel[]> {
    return this.http.get('scriptView/peers/get/'+id).pipe(
        map((response: ScriptViewPeersModel[]) => {
            return response;
        }),
    );
}
}
