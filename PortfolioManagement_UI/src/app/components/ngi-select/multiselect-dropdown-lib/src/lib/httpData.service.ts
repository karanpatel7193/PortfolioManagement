import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { HTTPTYPE } from './ngi-select.component';
import { ServiceResponse } from './ngiselect.model';

@Injectable({
    providedIn: 'root'
  })
export class LazyLoadingService {
  
  constructor(private httpClient: HttpClient) {}
  // Http Headers
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
    
  }

    getData(urlstring:string){
        return this.httpClient.get<ServiceResponse>(urlstring,this.httpOptions);
    }
   
    
     

}
