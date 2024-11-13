import { PagingSortingModel } from "src/app/models/pagingsorting.model";

export class BrokerMainModel{
public id: number = 0;
public name:string = '';
}

export class BrokerModel extends BrokerMainModel{
    public  brokerTypeId : number = 0;
    public  buyBrokerage : number = 0;
    public  sellBrokerage : number = 0;
    public  brokerType : string = '';
	public accountId: number = 0;
}

export class BrokerAddModel {

}

export class BrokerEditModel extends BrokerAddModel {
	public broker: BrokerModel = new BrokerModel();
}
export class BrokerGridModel {
	public brokers: BrokerModel[] = [];
	public totalRecords: number = 0;
}

export class BrokerParameterModel extends PagingSortingModel {
	public id: number = 0;
	public name: string = '';
	public brokerTypeId: number = 0;
	public brokerId: number = 0;
}

