import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { VolumeGridModel, VolumeParameterModel } from './volume.model';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
  providedIn: 'root'
})
export class VolumeService {

  constructor(private http: HttpService) { }

    public getForVolume(): Observable<VolumeGridModel> {
        return this.http.post('analysis/volume/getForVolume',null).pipe(
            map((response: VolumeGridModel) => {
                return response;
            }),
        );
    }
}
