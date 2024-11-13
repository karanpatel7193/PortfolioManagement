export class MyException {
	status : number;
	body : any;
	constructor(status : number, body : any) {
		this.status = status;
		this.body = body;
	}
	
}

export class ServiceResponse{
	data: any[]=[];
	status:number=0;
	errorId:any;
	exceptions:any;
}
