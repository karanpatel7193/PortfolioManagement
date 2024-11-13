import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    constructor(private http: HttpClient) { }

    public getProfileData(): Observable<any> {
        return this.http.post<any>('user/user-profile/', null).pipe(
            map((response: any) => {
                return response;
            })
        )
    }
    public updateAvatar(data: any): Observable<any> {
        return this.http.post<any>('user/profile_update_avatar/', data).pipe(
            map((response: any) => {
                return response;
            })
        )
    }
}
