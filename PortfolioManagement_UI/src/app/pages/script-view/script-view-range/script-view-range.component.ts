import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScriptViewRangeChartModel, ScriptViewRangeModel } from './script-view-range.model';
import { ScriptViewRangeService } from './script-view-range.service';
import { ScriptViewChartService } from '../script-view-chart/script-view-chart.service';
import { ScriptViewParameterModel } from '../script-view-chart/script-view-chart.model';
import { color } from 'echarts';

@Component({
    selector: 'app-script-view-range',
    templateUrl: './script-view-range.component.html',
})
export class ScriptViewRangeComponent implements OnInit, OnChanges {
    @Input() id!: number;
    public scriptViewRangeChartModel: ScriptViewRangeChartModel = new ScriptViewRangeChartModel();
    public scriptviewParameterModel: ScriptViewParameterModel = new ScriptViewParameterModel();
    public myColor: string = '';
    
    lineChartOption: any;
    public scriptViewRangeModel: ScriptViewRangeModel = new ScriptViewRangeModel();
    constructor(private scriptviewRangeService: ScriptViewRangeService, private route: ActivatedRoute, private scriptViewChartService: ScriptViewChartService) { }

    ngOnInit(): void {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['id'] && changes['id'].currentValue) {
            this.getScriptData();        
        }
    }

    public getScriptData() {
		this.scriptviewRangeService.getForRange(this.id).subscribe((data) => {
            this.scriptViewRangeChartModel = data;
            this.myColor = this.scriptViewRangeChartModel.script.priceChange > 0 ? 'green' : 'red';
			this.updateChartOptions(this.scriptViewRangeChartModel);
		});
	}
    
    public updateChartOptions(scriptViewRangeChartModel: ScriptViewRangeChartModel) {
        
        const maxPrice = Math.ceil(Math.max(...scriptViewRangeChartModel.priceSeriesData));
        const minPrice = Math.floor(Math.min(...scriptViewRangeChartModel.priceSeriesData));
    
        this.lineChartOption = {
            xAxis : [
                {
                    type: 'time',
                    boundaryGap:false             
                }
            ],
            yAxis : [
                {
                    type : 'value',
                    min: minPrice,
                    max: maxPrice
                }
            ],
            tooltip: {
                trigger: 'axis',
            },
            dataZoom: [
                {
                    type: 'inside', 
                    start: 0,
                    end: 100 
                }
            ],
            grid: {
                left: 40,
                top: 10,
                right: 20,
                bottom: 20
            },
            series: [
                {
                    name:'PriceSeries',
                    type: 'line',
                    itemStyle: {normal: {areaStyle: {color: this.myColor}}, color: this.myColor},
                    lineStyle: {color: this.myColor},
                    data: scriptViewRangeChartModel.timeSeries,
                }
            ]
        };
    }
}