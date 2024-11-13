import { PagingSortingModel } from "src/app/models/pagingsorting.model";
import { BrokerMainModel } from "../broker/broker.model";

export class AccountMainModel {
  public id: number = 0;
  public name: string = '';
}

export class AccountModel extends AccountMainModel {
  public brokers: AccountBrokerSelectEntity[] = [];
  }

export class AccountBrokerSelectEntity extends BrokerMainModel {
  public isSelected: boolean = false;
}

export class AccountAddModel {
  public brockers :AccountBrokerSelectEntity[]=[];
}

export class AccountEditModel {
  public account: AccountModel = new AccountModel();
}

export class AccountGridModel {
  public accounts: AccountModel[] = [];
  public totalRecords: number = 0;
}

export class AccountListModel extends AccountGridModel {
  public brokers: BrokerMainModel[]=[];
}

export class AccountParameterModel extends PagingSortingModel {
  public id: number = 0;
  public brokerId: number = 0;
  public name: any;
}
