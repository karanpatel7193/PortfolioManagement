import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PortfolioReportModel, PortfolioScriptModel } from '../portfolio-report.model';

@Component({
    selector: 'app-portfolio-report-summary',
    templateUrl: './portfolio-report-summary.component.html',
    styleUrls: ['./portfolio-report-summary.component.scss']
})
export class PortfolioReportSummaryComponent implements OnInit, OnChanges {
    @Input() portfolioReportModels: PortfolioReportModel = new PortfolioReportModel ();
    maxGainer: PortfolioScriptModel | null = null;
    minGainer: PortfolioScriptModel | null = null;
    constructor() { }

    ngOnChanges(changes: SimpleChanges): void {
    if(changes['portfolioReportModels']){
        this.findMaxGainer();
        this.findMinGainer();
    }
    }

    ngOnInit(): void {
        this.findMaxGainer();
        this.findMinGainer();
    }

    private findMaxGainer(): void {
        if (this.portfolioReportModels?.scripts.length) {
            this.maxGainer = this.portfolioReportModels.scripts.reduce((prev, current) => {
                return (prev.overallGLAmount > current.overallGLAmount) ? prev : current;
            });
        }
    }

    private findMinGainer(): void {
        if (this.portfolioReportModels?.scripts.length) {
            this.minGainer = this.portfolioReportModels.scripts.reduce((prev, current) => {
                return (prev.overallGLAmount < current.overallGLAmount) ? prev : current;
            });
        }
    }


}
