import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map} from 'rxjs/operators';
import { ResponseModel } from '../models/response.model';
import { ToastService } from './toast.service';

@Injectable({
    providedIn:"root"
})
export class BaseService {
    userId: any;

    public apiUrl: string = "";

    constructor(private http: HttpClient, private toaster: ToastService) { }

    get(url: any): Observable<any> {
        const httpOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };

        return this.http.get<ResponseModel>(this.apiUrl + url, httpOptions).pipe(
            map((response: ResponseModel) => {
                if (response.status == 200) {
                    return response.data;
                }
                else {
                    this.toaster.error("Service: " + url, 'Service Error');
                    throw new Error();
                }
            })
        );
    }

    post(url: any, data: any): Observable<any> {
        if (data != null) {
            data.user_id = this.userId;
        }

        const httpOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };

        return this.http.post<ResponseModel>(this.apiUrl + url, data, httpOptions).pipe(
            map((response: ResponseModel) => {
                if (response.status == 200) {
                    return response.data;
                }
                else {
                    this.toaster.error("Service: " + url, 'Service Error');
                    throw new Error();
                }
            })
        );
    }

    /**
    * Handle Http operation that failed.
    * Let the app continue.
    * @param operation - name of the operation that failed
    * @param result - optional value to return as the observable result
    */
    public handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

            // TODO: send the error to remote logging infrastructure
            console.log(error); // log to console instead

            // TODO: better job of transforming error for user consumption
            // this.log(`${operation} failed: ${error.message}`);

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }

    private handleErrors(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            console.log('An error occurred:', error.error.message);
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            console.log(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
        }
        // return an ErrorObservable with a user-facing error message
        this.toaster.error('Something bad happened; please try again later.');
        return null;
    };
}