import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserEditModel, UserModel, UserParameterModel } from 'src/app/pages/account/user/user.model';

@Injectable({
    providedIn: 'root'
})
export class ChangePasswordService {
    constructor(private http: HttpClient) { }

    public ChangePassword(userModel: UserModel): Observable<string> {
        return this.http.post('account/user/changepassword', userModel).pipe(
            map((response: any) => {
                return response;
            }),
        );
    }
}
