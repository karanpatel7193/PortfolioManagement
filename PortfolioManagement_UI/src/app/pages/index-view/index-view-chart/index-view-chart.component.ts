import { Component, OnInit } from '@angular/core';
import { IndexViewChartModel, IndexViewParameterModel } from './index-view-chart.model';
import { IndexViewChartService } from './index-view-chart.service';

@Component({
    selector: 'app-index-view-chart',
    templateUrl: './index-view-chart.component.html'
})
export class IndexViewChartComponent implements OnInit {
    public indexViewChartModel: IndexViewChartModel = new IndexViewChartModel();
    public indexViewParameterModel: IndexViewParameterModel = new IndexViewParameterModel();
    niftyChartOption: any;
    sensexChartOption: any;
    difiChartOption: any;

    constructor(private indexViewChartService: IndexViewChartService) { }

    ngOnInit(): void {
        this.getScriptData();
    }

    public getScriptData() {
        this.indexViewChartService.getForChart(this.indexViewParameterModel).subscribe((data) => {
            this.indexViewChartModel = data;
            this.updateChartOptions(this.indexViewChartModel);  
        });
    }

    public updateChartOptions(indexViewChartModel: IndexViewChartModel) {
        this.niftyChartOption = {
            xAxis: [
                {
                    type: 'category',
                    data: indexViewChartModel.dates, 
                    name: 'Dates',
                },
            ],
            yAxis: {
                type: 'value',
            },
            series: [
                {
                    name: 'niftyCandlestick', 
                    data: indexViewChartModel.niftySeriesData,
                    type: 'candlestick',
                    smooth: true 
                },
            ]
        },
        this.sensexChartOption = {
            xAxis: [
                {
                    type: 'category',
                    data: indexViewChartModel.dates, 
                    name: 'Dates',
                },
            ],
            yAxis: {
                type: 'value',
            },
            series: [
                {
                    name: 'sensexCandlestick', 
                    data: indexViewChartModel.sensexSeriesData,
                    type: 'candlestick',
                    smooth: true 
                },
            ]
        },
        this.difiChartOption = {
            xAxis: {
                type: 'category',
                data: indexViewChartModel.diiSeriesData
            },
            yAxis: {
                type: 'value',
            },
            series: [
                {
                    data: indexViewChartModel.fiiSeriesData,
                    type: 'line',
                }
            ]
        }
    }
}
