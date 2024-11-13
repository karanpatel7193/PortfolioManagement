import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PortfolioDatewiseModel, PortfolioDatewiseParameterModel, PortfolioDatewiseReportModel } from './portfolio-datewise.model';
import { PortfolioDatewiseService } from './portfolio-datewise.service';

@Component({
  selector: 'app-portfolio-datewise-chart',
  templateUrl: './portfolio-datewise-chart.component.html',
  styleUrls: ['./portfolio-datewise-chart.component.scss']
})
export class PortfolioDatewiseChartComponent implements OnInit{
    public portfolioDatewiseReportModel: PortfolioDatewiseReportModel[] = [];
    public portfolioDatewiseParameterModel: PortfolioDatewiseParameterModel = new PortfolioDatewiseParameterModel();
    
    public investmentAmountseries: number[] = [];
    public marketValueseries: number[] = [];
    public timeSeries: string[] = [];
    lineChartOption: any;
   
    @Input() accountId: number = 0;
    @Input() brokerId: number = 0;
    public myColor: string = '';

    constructor(private portfolioDatewiseService: PortfolioDatewiseService) {
        var today = new Date();
        this.portfolioDatewiseParameterModel.fromDate = new Date(today.setMonth(today.getMonth() - 1)); 
    }

    ngOnInit(): void {
    }
    ngOnChanges(changes: SimpleChanges): void {
        if (changes['accountId'] || changes['brokerId']) {
            this.timeSeries = [];
            this.investmentAmountseries = [];
            this.marketValueseries = [];
            this.getPortfolioDatewiseChart();
        }
    }
    
    public onDateChange() {
        this.marketValueseries = [];
        this.investmentAmountseries = [];
        this.timeSeries = [];
        this.getPortfolioDatewiseChart();
    }

    public getPortfolioDatewiseChart() {
        if(!this.portfolioDatewiseParameterModel.fromDate){
            var today = new Date();
            this.portfolioDatewiseParameterModel.fromDate = new Date(today.setMonth(today.getMonth() - 1)); 
        }

        this.portfolioDatewiseParameterModel.accountId = this.accountId;
        this.portfolioDatewiseParameterModel.brokerId = this.brokerId;
        this.portfolioDatewiseService.getPortfolioDatewiseReport(this.portfolioDatewiseParameterModel).subscribe((data) => {
            var reportData = data[0]; 

            this.timeSeries = [];
            this.investmentAmountseries = [];
            this.marketValueseries = [];

            this.timeSeries = reportData.timeSeries;                    
            this.investmentAmountseries = reportData.investmentSeries;  
            this.marketValueseries = reportData.marketValueSeries;   

            if (this.marketValueseries.length > 0 && this.investmentAmountseries.length > 0) {
                const lastInvestmentAmount = this.investmentAmountseries[this.investmentAmountseries.length - 1];
                const lastMarketValue = this.marketValueseries[this.marketValueseries.length - 1];
            
                if (lastMarketValue > lastInvestmentAmount) {
                    this.myColor = 'rgba(0, 255, 0, 0.5)';  
                } else {
                    this.myColor = 'rgba(255, 0, 0, 0.5)';  
                }
            }
            this.updateChartOptions();  
        });
    }

    public updateChartOptions() {           
        const maxInvestmentAmount = Math.ceil(Math.max(...this.investmentAmountseries));
        const minInvestmentAmount = Math.floor(Math.min(...this.investmentAmountseries));
        const maxMarketValue = Math.ceil(Math.max(...this.marketValueseries));
        const minMarketValue = Math.floor(Math.min(...this.marketValueseries));
    
        const maxAmount = Math.max(maxInvestmentAmount, maxMarketValue) + 1000;
        const minAmount = Math.min(minInvestmentAmount, minMarketValue) - 1000;
    
        this.lineChartOption = {
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: this.timeSeries, 
                    axisTick: { show: false },
                    axisLine: { lineStyle: { color: '#777' } },
                    axisLabel: { show: true },
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    min: minAmount,  
                    max: maxAmount,  
                }
            ],
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'cross',
                    label: {
                        backgroundColor: '#6a7985'
                    }
                }
            },
            dataZoom: [
                {
                    type: 'inside', 
                    start: 0,
                    end: 100,
                }
            ],
            grid: {
                left: 65,
                top: 10,
                right: 35,
                bottom: 20,
            },
            series: [
                {
                    name: 'Investment Amount',
                    type: 'line',
                    itemStyle: {
                        normal: {
                            areaStyle: {
                                color: 'yellow',  
                            },
                        },
                    },
                    lineStyle: {
                        color: 'yellow',
                    },
                    data: this.investmentAmountseries,  
                },
                {
                    name: 'Market Value',
                    type: 'line',
                    itemStyle: {normal: {areaStyle: {color: this.myColor}}, color: this.myColor},
                    lineStyle: {color: this.myColor},
                    data: this.marketValueseries,  
                }
            ]
        };
    }
}
