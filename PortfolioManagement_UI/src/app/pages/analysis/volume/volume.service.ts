import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BaseService } from 'src/app/services/base.service';
import { VolumeGridModel, VolumeParameterModel } from './volume.model';

@Injectable({
  providedIn: 'root'
})
export class VolumeService {

  constructor(private httpBase: BaseService) { }

  public getForVolume(): Observable<VolumeGridModel> {
    return this.httpBase.post('analysis/volume/getForVolume',null).pipe(
        map((response: VolumeGridModel) => {
            return response;
        }),
    );
}
}
