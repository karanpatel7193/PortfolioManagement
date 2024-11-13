import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ReadStatusParameters } from 'src/app/models/read-status.model';
import { CommonPageModel } from 'src/app/pages/common-page/common.page.model';
import { CommonPageService } from 'src/app/pages/common-page/common.page.service';
import { CommonService } from 'src/app/services/common.service';
import { SessionService } from 'src/app/services/session.service';
import { ToastService } from 'src/app/services/toast.service';
import { SpinnerService } from '../spinner/spinner.service';
import { ModalConfig } from '../modal/modal.config';
import { ModalComponent } from '../modal/modal.component';
import { ProjectModel } from 'src/app/models/project.model';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {
    @Input() pricingProduct: string = '';
    @Output() applyClick: EventEmitter<void> = new EventEmitter();
    @Output() qFactorsApplyClick: EventEmitter<void> = new EventEmitter();
    @Output() qFactorsResetClick: EventEmitter<void> = new EventEmitter();
    @Output() exportAccountLevelClick: EventEmitter<void> = new EventEmitter();
    @Output() downloadCECLReportClick: EventEmitter<void> = new EventEmitter();

    public commonPageModel: CommonPageModel = new CommonPageModel();
    public generateSpreadsheet: any = {};
    public downloadAllReport: any = {};
    public downloadReport: any = {};
    public downloadAscReport: any = {};
    public export: any = {};
    public exportSendFile: any = {};
    public showDownloadLoanLevelButton: boolean = false;
    public showDownloadAccountLevelButton: boolean = false;
    public applyDisabled: boolean = false;
    public resetDisabled: boolean = false;
    public qFactorsButtonsDisabled: boolean = false;
    public footerDisabled: boolean = false;
    public isMenuCollapsed: boolean = false;
    public projectList: ProjectModel[] = [];
    public selectedProject: ProjectModel = new ProjectModel();
    public generateReportUrl: string = '';

    @ViewChild('modal') private modalComponent!: ModalComponent;
    public modalConfig!: ModalConfig;
    public modelMessage: string = '';

    constructor(private router: Router, private commonPageService: CommonPageService, private toastService: ToastService, private sessionService: SessionService, private commonService: CommonService, public spinnerService: SpinnerService) {
        const currentRoute: string[] = this.router.url.split('/');
        let routeName: string = currentRoute[currentRoute.length - 1];
        this.commonPageModel = this.commonPageService.getApiData(routeName);
        if (this.commonPageModel.pageName == '') {
            //TODO: do any activity if page is not belogn to list.
        }
    }

    ngOnInit(): void {
        this.sessionService.menuCollapsedState.subscribe(data => {
            this.isMenuCollapsed = data;
        });
        this.sessionService.readStatusState.subscribe(data => {
            this.setDownloadVariables(data)
        });
        this.sessionService.projectsState.subscribe(data => {
            this.projectList = this.sessionService.projectsValue;
            this.selectedProject = this.projectList.find(v => v.id == this.sessionService.projectIdValue) || new ProjectModel();
        });
        this.sessionService.disableApplyButtonState.subscribe(data => {
            this.applyDisabled = data;
        });
    }

    public setDownloadVariables(data: ReadStatusParameters) {
        if (this.commonPageModel.downloadAllReport) {
            this.downloadAllReport = data.reports[this.commonPageModel.downloadAllReport];
            this.generateReport(this.downloadAllReport, this.commonPageModel.downloadAllReport);
        }
        if (this.commonPageModel.generateSpreadsheet) {
            if (Object.keys(this.generateSpreadsheet).length == 0)
                this.getReportParameters(this.commonPageModel.generateSpreadsheet, 'Spreadsheet');
        }
        if (this.commonPageModel.downloadReport) {
            this.downloadReport = data.reports[this.commonPageModel.downloadReport];
            this.generateReport(this.downloadReport, this.commonPageModel.downloadReport);
        }
        if (this.commonPageModel.downloadAscReport) {
            this.downloadAscReport = data.reports[this.commonPageModel.downloadAscReport];
            this.generateReport(this.downloadAscReport, this.commonPageModel.downloadAscReport);
        }
        // if (this.commonPageModel.export)
        //     this.export = data.reports[this.commonPageModel.export];
        // if (this.commonPageModel.exportSendFile)
        //     this.exportSendFile = data.reports[this.commonPageModel.exportSendFile];

        if ((this.commonPageModel.pageName === 'forecast-primary' || this.commonPageModel.pageName === 'forecast-secondary' || this.commonPageModel.pageName === 'cecl-current-estimate') && this.sessionService.currentUserValue.is_admin)
            this.showDownloadLoanLevelButton = true;
        if (this.commonPageModel.pageName.includes('account-level') && this.selectedProject.is_SFTP_Enabled)
            this.showDownloadAccountLevelButton = true;
    }

    public apply() {
        this.applyClick.emit();
    }

    public qFactorsApplyButton() {
        this.qFactorsApplyClick.emit();
    }

    public qFactorsResetButton() {
        this.qFactorsResetClick.emit();
    }

    public download(reportType: string, report: any) {
        let fileName: string = this.commonPageModel.pageName + '-report';
        if (reportType == 'downloadAscReport')
            fileName = 'asc-450-report';
        else if (reportType == 'downloadAllReport')
            fileName = 'all-reports';

        this.downloadFile(report.value, fileName);
    }

    private downloadFile(url: string, fileName: string) {
        let product: any = null;
        if (this.pricingProduct && this.pricingProduct != ''){
            product = {};
            product['product'] = [];
            product['product'].push(this.pricingProduct); 
        }
        this.commonService.downloadReport(url, fileName, product).subscribe({
            next: (response) => {
                if (response) {
                    var fakeUrl = window.URL.createObjectURL(response);
                    var a = document.createElement('a');
                    a.href = fakeUrl;
                    a.download = fileName;
                    a.click();
                    a.remove();
                }
            },
            error: (error) => {
                console.log(error);
                this.toastService.error('Error while downloading! Please contact administrator.')
            }
        });
    }

    private generateReport(report: any, url: string) {
        if (report.status == 'not defined' && this.generateReportUrl != url) {
            this.generateReportUrl = url;
            let data: any = null;
            if (url == 'download-cecl-report'){
                data = this.sessionService.getCECL_ValuationMethod;               
            }
            this.commonPageService.generateReport(url + '/', data).subscribe({
                next: (response) => {
                    this.generateReportUrl = '';
                    if (response.status == "done") {
                        report.status = response.status;
                        report.value = response.data?.files[0];
                    }
                    else if (response.status == "error")
                        this.toastService.error('Error while generating report!' + response.message[0])
                },
                error: (error) => {
                    this.generateReportUrl = '';
                    console.log(error);
                    this.toastService.error('Generate report having error! Please contact administrator.')
                }
            });
        }
    }

    private getReportParameters(url: string, type: string) {
        this.commonPageService.getDownloadData(url).subscribe({
            next: (response) => {
                if (response.status == "done") {
                    if (type == 'Spreadsheet')
                        this.generateSpreadsheet = { "status": response.status, "value": response.data.files.length > 0 ? response.data.files[0] : '' };
                }
                else if (response.status == "error")
                    this.toastService.error('Export ' + type + ' having error!' + response.message[0])
            },
            error: (error) => {
                console.log(error);
                this.toastService.error('Export ' + type + ' having error! Please contact administrator.')
            }
        });
    }

    public async exportModelShow(type: string) {
        if (type == 'Loan-Level') {
            if ((this.commonPageModel.pageName === 'forecast-primary' && this.sessionService.isForecastPrimaryLoanExportValue) || (this.commonPageModel.pageName === 'forecast-secondary' && this.sessionService.isForecastSecondaryLoanExportValue) || (this.commonPageModel.pageName === 'cecl-current-estimate' && this.sessionService.isCECLCurrentEstimateLoanExportValue)) {
                this.modelMessage = 'Export loan-level already started so please wait.';
                this.modalConfig = {
                    modalTitle: 'Export Loan-Level',
                    closeButtonLabel: 'Cancel',
                    hideDismissButton: () => true
                };
            }
            else {
                if (this.commonPageModel.pageName === 'cecl-current-estimate')
                    this.modelMessage = 'Monthly loan-level CECL forecasts for existing loans using the chosen economic scenario will be exported to the data transfer server.';
                else
                    this.modelMessage = 'Monthly loan-level forecasts for existing loans using the primary economic scenario will be exported to the data transfer server.';

                this.modalConfig = {
                    modalTitle: 'Export Loan-Level',
                    dismissButtonLabel: 'Export',
                    closeButtonLabel: 'Cancel',
                    onDismiss: () => this.exportLoanLevel()
                };
            }
        } else if (type == 'Account-Level') {
            if ((this.commonPageModel.pageName === 'forecast-account-level' && this.sessionService.isForecastAccountLevelExportValue) || (this.commonPageModel.pageName === 'stress-test-account-level' && this.sessionService.isForecastAccountLevelExportValue) || (this.commonPageModel.pageName === 'cecl-account-level' && this.sessionService.isCECLAccountLevelExportValue)) {
                this.modelMessage = 'Export account-level already started so please wait.';
                this.modalConfig = {
                    modalTitle: 'Export Account-Level',
                    closeButtonLabel: 'Cancel',
                    hideDismissButton: () => true
                };
            }
            else {
                this.modelMessage = 'All data will be exported to the data transfer server(SFTP).';

                this.modalConfig = {
                    modalTitle: 'Download All Account-Level',
                    dismissButtonLabel: 'Download',
                    closeButtonLabel: 'Cancel',
                    onDismiss: () => this.exportAccountLevel()
                };
            }
        }

        return await this.modalComponent.open();
    }

    public async exportLoanLevel(): Promise<boolean> {
        if (this.commonPageModel.export) {
            this.modelMessage = 'Loading...';
            this.modalConfig.disableDismissButton = () => true;
            this.modalConfig.disableCloseButton = () => true;
                this.resetExportLoanLevelFlag(true);
            await this.exportLoanLevelData(this.commonPageModel.export);
        
            this.modalConfig.disableDismissButton = () => false;
            this.modalConfig.disableCloseButton = () => false;
    
            return await true;
        }
        else
            return await true;
    }

    private async exportLoanLevelData(url: string): Promise<boolean> {
        return new Promise((resolve, reject) => {
            this.commonPageService.getDownloadData(url).subscribe({
                next: (response) => {
                    if (response.status == "done") {
                        this.export = { "status": response.status, "value": response.data.files.length > 0 ? response.data.files[0] : '' };

                        if (this.commonPageModel.exportSendFile) {
                            this.commonPageService.getDownloadData(this.commonPageModel.exportSendFile).subscribe({
                                next: (response) => {
                                    if (response.status == "done") {
                                        //this.export = { "status": response.status, "value": response.data.files.length > 0 ? response.data.files[0] : '' };
                                        let fileName: string = this.commonPageModel.pageName + '-loan-level';
                                        this.downloadFile(this.export.value, fileName);
                                    }
                                    else if (response.status == "error")
                                        this.toastService.error('Error while exporting loan-level. Please check the path to SFTP or contact the administrator!' + response.message[0]);

                                    this.resetExportLoanLevelFlag(false);
                                    resolve(true);
                                },
                                error: (error) => {
                                    console.log(error);
                                    this.toastService.error('Error while exporting loan-level. Please check the path to SFTP or contact the administrator.');

                                    this.resetExportLoanLevelFlag(false);
                                    resolve(true);
                                }
                            });
                        }
                        else
                            resolve(true);
                    }
                    else if (response.status == "error") {
                        this.toastService.error('Export Loan-Level having error!' + response.message[0])
                        this.resetExportLoanLevelFlag(false);
                        resolve(true);
                    }
                },
                error: (error) => {
                    console.log(error);
                    this.toastService.error('Export Loan-Level having error! Please contact administrator.')
                    this.resetExportLoanLevelFlag(false);
                    resolve(true);
                }
            });
        });
    }

    private resetExportLoanLevelFlag(value: boolean) {
        if (this.commonPageModel.pageName === 'forecast-primary')
            this.sessionService.setIsForecastPrimaryLoanExport(value);
        else if (this.commonPageModel.pageName === 'forecast-secondary')
            this.sessionService.setIsForecastSecondaryLoanExport(value);
        else if (this.commonPageModel.pageName === 'cecl-current-estimate')
            this.sessionService.setIsCECLCurrentEstimateLoanExport(value);
    }

    public async exportAccountLevel(): Promise<boolean> {
        if (this.commonPageModel.export) {
            this.toastService.info('It will take time to export. you can continue your work. System will notify you once file uploaded on SFTP.');
            return await this.exportAccountLevelData(this.commonPageModel.export);
        } else {
            return false;
        }
    }

    private async exportAccountLevelData(url: string): Promise<boolean> {
        return new Promise((resolve, reject) => {
            this.exportAccountLevelClick.emit();
            resolve(true);
        });
    }

    public downloadCECLReport(report: any) {
        this.downloadCECLReportClick.emit(report)
    }

}


