import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';
import { HttpService } from '../../../services/http.service';
import { AccountModel,  AccountParameterModel,AccountGridModel, AccountListModel, AccountAddModel, AccountEditModel } from './account.model';
import { from } from 'rxjs';

@Injectable()
export class AccountService {
	constructor(private http: HttpService) {
	}

	public getRecord(id: number): Observable<AccountModel> {
		return this.http.get('master/account/getRecord/' + id).pipe(
			map((response: AccountModel) => {
				return response;
			})
		)
	}

	public getForLOV(accountParameter: AccountParameterModel): Observable<AccountListModel> {
		return this.http.post('master/account/getLovValue',accountParameter ).pipe(
			map((response: AccountListModel) => response)
        );
    }
	public getForAdd(accountParameterModel: AccountParameterModel ): Observable<AccountAddModel> {
        return this.http.post('master/account/getAddData',accountParameterModel).pipe(
            map((response: AccountAddModel) => response)
        );
    }

    public getForEdit(accountParameterModel: AccountParameterModel): Observable<AccountEditModel> {
        return this.http.post('master/account/getEditData',accountParameterModel).pipe(
            map((response: AccountEditModel) => response)
        );
    }


	public getForGrid(accountParameter: AccountParameterModel): Observable<AccountGridModel> {
		return this.http.post('master/account/getGridData', accountParameter).pipe(
			map((response: AccountGridModel) => response)
        );
    }

	public save(account: AccountModel): Observable<number> {
		if (account.id === 0)
			return this.http.post('master/account/insert', account).pipe(
				map((response: number) => response)
			);
		else
			return this.http.post('master/account/update', account).pipe(
				map((response: number) => {
					return response;
				}),
			);
	}

	public delete(id: number): Observable<void> {
		return this.http.post('master/account/delete/' + id, null).pipe(
			map((response: void) => {
				return response;
			}),
		);
	}
}
