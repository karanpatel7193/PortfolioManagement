import { Injectable } from '@angular/core';
//  import { BaseService } from '../../../services/base.service';
import { catchError, map, tap } from 'rxjs/operators';
import {
    UserModel, UserMainModel, UserParameterModel, UserGridModel,
    UserAddModel, UserEditModel, UserListModel, UserLoginModel
} from './user.model';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/services/base.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root' // This makes the service available application-wide
  })
export class UserService {
    // constructor(private httpBase: HttpClient) {
    constructor(private httpBase: BaseService) {
    }

    public getRecord(id: number): Observable<UserModel> {
        return this.httpBase.get('account/user/getRecord/' + id).pipe(
            map((response: UserModel) => {
                return response;
            }),
        );
    }

    public getForLOV(userParameter: UserParameterModel): Observable<UserMainModel[]> {
        return this.httpBase.post('account/user/getLovValue', userParameter).pipe(
            map((response: UserMainModel[]) => {
                return response;
            }),
        );
    }

    public getAddMode(userParameter: UserParameterModel): Observable<UserAddModel> {
        return this.httpBase.post('account/user/getAddMode', userParameter).pipe(
            map((response: UserAddModel) => {
                return response;
            }),
        );
    }

    public getEditMode(userParameter: UserParameterModel): Observable<UserEditModel> {
        return this.httpBase.post('account/user/getEditMode', userParameter).pipe(
            map((response: UserEditModel) => {
                return response;
            })
        );
    }

    public getForGrid(userParameter: UserParameterModel): Observable<UserGridModel> {
        return this.httpBase.post('account/user/getGridData', userParameter).pipe(
            map((response: UserGridModel) => {
                return response;
            }),
        );
    }

    public getListMode(userParameter: UserParameterModel): Observable<UserListModel> {
        return this.httpBase.post('account/user/getListMode', userParameter).pipe(
            map((response: UserListModel) => {
                return response;
            }),
        );
    }

    public Registration(user: UserModel): Observable<number> {
        return this.httpBase.post('account/user/registration', user).pipe(
            map((response: number) => {
                return response;
            }),
        );
    }

    public save(user: UserModel): Observable<number> {
        if (user.id === 0)
            return this.httpBase.post('account/user/insert', user).pipe(
                map((response: number) => {
                    return response;
                }),
            );
        else
            return this.httpBase.post('account/user/update', user).pipe(
                map((response: number) => {
                    return response;
                }),
            );
    }

    public delete(id: number): Observable<void> {
        return this.httpBase.post('account/user/delete/' + id, null).pipe(
            map((response: void) => {
                return response;
            })
        );
    }

    public validateLogin(user: UserModel): Observable<UserLoginModel> {
        return this.httpBase.post('account/user/validateLogin', user).pipe(
            map((response: UserLoginModel) => {
                return response;
            }),
        );
    }

    public resetPassword(user: UserModel): Observable<boolean> {
        return this.httpBase.post('account/user/resetPassword', user).pipe(
            map((response: boolean) => {
                return response;
            }),
        );
    }

    public updatePassord(user: UserModel): Observable<string> {
        return this.httpBase.post('account/user/updatePassword', user).pipe(
            map((response: string) => {
                return response;
            }),
        );
    }
}
