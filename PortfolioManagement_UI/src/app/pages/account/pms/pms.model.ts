
import { PagingSortingModel } from '../../../models/pagingsorting.model';

export class PmsModel   {
	public id: number = 0;
	public name: string = '';
    public isActive?: boolean ;
	public type: string ='';
}

export class PmsAddModel {
}
export class PmsListModel {
}
export class PmsEditModel extends PmsAddModel {
	public pms: PmsModel = new PmsModel();
}

export class PmsGridModel {
	public pmss: PmsModel[] = [];
	public totalRecords: number = 0;
}

export class PmsParameterModel extends PagingSortingModel {
	public id: number = 0;
	public name: string = '';
}
