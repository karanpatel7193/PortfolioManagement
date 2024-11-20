import { Injectable } from '@angular/core';
import { catchError, map, tap } from 'rxjs/operators';
import {
    UserModel, UserMainModel, UserParameterModel, UserGridModel,
    UserAddModel, UserEditModel, UserListModel, UserLoginModel
} from './user.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HttpService } from 'src/app/services/http.service';

@Injectable({
    providedIn: 'root' 
  })
export class UserService {
    constructor(private http: HttpService) {
    }

    public getRecord(id: number): Observable<UserModel> {
        return this.http.get('account/user/getRecord/' + id).pipe(
            map((response: UserModel) => {
                return response;
            }),
        );
    }

    public getForLOV(userParameter: UserParameterModel): Observable<UserMainModel[]> {
        return this.http.post('account/user/getLovValue', userParameter).pipe(
            map((response: UserMainModel[]) => {
                return response;
            }),
        );
    }

    public getAddMode(userParameter: UserParameterModel): Observable<UserAddModel> {
        return this.http.post('account/user/getAddMode', userParameter).pipe(
            map((response: UserAddModel) => {
                return response;
            }),
        );
    }

    public getEditMode(userParameter: UserParameterModel): Observable<UserEditModel> {
        return this.http.post('account/user/getEditMode', userParameter).pipe(
            map((response: UserEditModel) => {
                return response;
            })
        );
    }

    public getForGrid(userParameter: UserParameterModel): Observable<UserGridModel> {
        return this.http.post('account/user/getGridData', userParameter).pipe(
            map((response: UserGridModel) => {
                return response;
            }),
        );
    }

    public getListMode(userParameter: UserParameterModel): Observable<UserListModel> {
        return this.http.post('account/user/getListMode', userParameter).pipe(
            map((response: UserListModel) => {
                return response;
            }),
        );
    }

    public Registration(user: UserModel): Observable<number> {
        return this.http.post('account/user/registration', user).pipe(
            map((response: number) => {
                return response;
            }),
        );
    }

    public save(user: UserModel): Observable<number> {
        if (user.id === 0)
            return this.http.post('account/user/insert', user).pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.http.post('account/user/update', user).pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.http.post('account/user/delete/' + id, null).pipe(
            map((response: void) => {
                return response;
            })
        );
    }

    public validateLogin(user: UserModel): Observable<UserLoginModel> {
        return this.http.post('account/user/validateLogin', user).pipe(
            map((response: UserLoginModel) => {
                return response;
            }),
        );
    }

    public resetPassword(user: UserModel): Observable<boolean> {
        return this.http.post('account/user/resetPassword', user).pipe(
            map((response: boolean) => {
                return response;
            }),
        );
    }

    public updatePassord(user: UserModel): Observable<string> {
        return this.http.post('account/user/updatePassword', user).pipe(
            map((response: string) => {
                return response;
            }),
        );
    }
}
