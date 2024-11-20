import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ResponseModel } from '../models/response.model';
import { Router } from '@angular/router';
import { SpinnerService } from '../components/spinner/spinner.service';
import { ToastService } from './toast.service';
import { SessionService } from './session.service';

@Injectable()
export class HttpService {
	public requestObj: any = {};

	constructor(private http: HttpClient, private spinner: SpinnerService, private router: Router, private toasterService: ToastService, private sessionService: SessionService) { }


	public get(url: string, hideSpinner: boolean = true): Observable<any> {
		const httpOptions = {
			headers: new HttpHeaders({ 'Content-Type': 'application/json' })
		};
		try {
			if (!hideSpinner) {
				this.spinner.show();
			}
			return this.http.get<ResponseModel>(url, httpOptions).pipe(
				map((response: ResponseModel) => {
					if (!hideSpinner) {
						this.spinner.hide()
					}
					if (response.status == 200) {
						return response.data;
					}
					else if (response.status == 401) {
						this.handleUnauthorizedError(response.status);
					}
					else {
						this.router.navigateByUrl('login');
						return null;
					}
				}),
				catchError(async (error) => this.handleErrors(error, hideSpinner))
			);
		}
		catch (error: any) {
			this.handleErrors(error, hideSpinner);
			return of(null);
		}
	}

	public post(url: string, data?: any, hideSpinner: boolean = true): Observable<any> {
		const httpOptions = {
			headers: new HttpHeaders({ 'Content-Type': 'application/json' })
		};
		try {
			if (!hideSpinner) {
				this.spinner.show();
			}
			return this.http.post<ResponseModel>(url, data, httpOptions).pipe(
				map((response: ResponseModel) => {
					if (!hideSpinner) {
						this.spinner.hide()
					}
					if (response.status == 200 || response.status == 200) {
						return response.data;
					}
					else if (response.status == 401) {
						this.handleUnauthorizedError(response.status);
					}
					else {
						return response;
					}
				}),
				catchError(async (error) => this.handleErrors(error, hideSpinner))
			);
		}
		catch (error: any) {
			this.handleErrors(error, hideSpinner);
			return of(error);
		}
	}

	public getUnsecure(url: string, hideSpinner?: boolean): Observable<any> {
		const httpOptions = {
			headers: new HttpHeaders({ 'Content-Type': 'application/json' })
		};
		if (!hideSpinner) {
			this.spinner.show();
		}
		try {
			return this.http.get(url, httpOptions).pipe(
				map((response: any) => {
					if (!hideSpinner) {
						this.spinner.hide()
					}
					return response;
				}),
				catchError(async (error) => this.handleErrors(error, hideSpinner))
			);
		}
		catch (error: any) {
			this.handleErrors(error, hideSpinner);
			return of(null);
		}
	}

	//Used to catch API errors 
	private handleErrors(error: HttpErrorResponse, hideSpinner?: boolean) {
		console.log('Error: ', error);
		if (error.status == 401) {
			this.sessionService.logout();
			return of(error);
		}
		if (!hideSpinner) {
			this.spinner.hide()
		}
		return null;
	};

	

	private handleUnauthorizedError(error: number): Observable<any> {
		if (error == 401) {
			this.toasterService.warning("User is unauthorized!");
			this.router.navigate(["/auth/login"]);
			return of(error);
		}
		if (error == 400) {
			this.toasterService.warning("Bad request response!");
			this.router.navigate(["/auth/login"]);
			return of(error);
		}
		return of('error');
	}

}
