export class MasterModel {
	public id: number = 0;
	public type: string = '';
    public masterValues: MasterValueModel[] = [];
}

export class MasterValueModel {
	public masterId: number = 0;
	public value: number = 0;
	public valueText: string = '';
}

export class MasterAddModel {

}

export class MasterEditModel extends MasterAddModel {
	public master: MasterModel = new MasterModel();
}

export class MasterGridModel {
	public masters: MasterModel[] = [];
}

export class MasterListModel extends MasterGridModel {
}
export class MasterParameterModel {
    public id: number = 0;
}
