import { ResponseModel } from "./response.model";

export class ReadStatusResponseModel extends ResponseModel {
    override data: ReadStatusData = new ReadStatusData(); 
}

export class ReadStatusData {
    parameters: ReadStatusParameters = new ReadStatusParameters();
    files: any;
}

export class ReadStatusParameters {
    'history.file.name': string;
    'max.history.date.analysis': string;
    'min.history.date.analysis': string;
    'analysis.complete.date': string;
    'forecast.file.name': string;
    'max.history.date.forecast': string;
    'min.history.date.forecast': string;
    'forecast.complete.date': string;
    'forecast.date.from': string;
    'forecast.date.to': string;
    'ref.scenario': string;
    menu: any;
    reports: any;
    templates: any;
    user: any;
}

//Notepad ++ ma response and url chhe.