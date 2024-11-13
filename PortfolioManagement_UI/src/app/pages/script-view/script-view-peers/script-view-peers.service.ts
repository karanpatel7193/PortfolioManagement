import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/services/base.service';
import { ScriptViewPeersModel } from './script-view-peers.model';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScriptViewPeersService {

  constructor(private httpBase: BaseService) { }

  public getRecord(id: number): Observable<ScriptViewPeersModel[]> {
    return this.httpBase.get('scriptView/peers/get/'+id).pipe(
        map((response: ScriptViewPeersModel[]) => {
            return response;
        }),
    );
}
}
