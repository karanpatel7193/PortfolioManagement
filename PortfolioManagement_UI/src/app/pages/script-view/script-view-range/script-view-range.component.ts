import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ScriptViewRangeChartModel, ScriptViewRangeModel } from './script-view-range.model';
import { ScriptViewRangeService } from './script-view-range.service';
import { ScriptViewChartService } from '../script-view-chart/script-view-chart.service';
import { ScriptViewChartModel, ScriptViewParameterModel } from '../script-view-chart/script-view-chart.model';
import * as echarts from 'echarts';
import { bottom } from '@popperjs/core';

@Component({
    selector: 'app-script-view-range',
    templateUrl: './script-view-range.component.html',
})
export class ScriptViewRangeComponent implements OnInit, OnChanges {
    @Input() id!: number;
    public scriptViewRangeChartModel: ScriptViewRangeChartModel = new ScriptViewRangeChartModel();
    public scriptViewChartModel: ScriptViewChartModel = new ScriptViewChartModel();
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
        let myChart = echarts.init(document.getElementById('chartContainer') as HTMLElement);
    
        const categories = scriptViewRangeChartModel.timeSeries; 
        const priceData = scriptViewRangeChartModel.priceSeriesData; 
        const volumeData = scriptViewRangeChartModel.volumeSeriesData;
    
        const minPrice = Math.floor(Math.min(...priceData));
        const maxPrice = Math.ceil(Math.max(...priceData));
        const minVolume = Math.floor(Math.min(...volumeData));
        const maxVolume = Math.ceil(Math.max(...volumeData));
    
        let option = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'cross',
                    label: {
                        backgroundColor: '#283b56'
                    }
                },
                formatter: (params: any) => {
                    const timeIndex = params[0].dataIndex;
                    const price = priceData[timeIndex];
                    const volume = volumeData[timeIndex];
                    return `
                        <div>
                            <strong>Price:</strong> ${price} <br>
                            <strong>Volume:</strong> ${volume}
                        </div>
                    `;
                }
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: categories 
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    name: 'Price',
                    min: minPrice,
                    max: maxPrice,
                    axisLabel: {
                        formatter: '{value}'
                    },
                    position: 'left',
                },
                {
                    type: 'value',
                    name: 'Volume',
                    min: minVolume,
                    max: maxVolume,
                    axisLabel: {
                        formatter: function (value: any) {
                            if (value >= 10000000) { // 1 crore or more
                                return (value / 10000000).toFixed(1) + 'Cr'; 
                            } else if (value >= 100000) { // 1 lakh or more
                                return (value / 100000).toFixed(1) + 'L'; 
                            } else if (value >= 1000) { // 1 thousand or more
                                return (value / 1000).toFixed(1) + 'k'; 
                            }
                            return value; // Less than 1 thousand
                        }
                    },
                    position: 'right',
                    show: true
                }
            ],
            series: [
                {
                    name: 'Price',
                    type: 'line',
                    yAxisIndex: 0,
                    data: priceData,
                    lineStyle: {
                        color: '#5470C6',
                        width: 3
                    },
                    areaStyle: {
                        color: 'rgba(84,112,198, 0.3)'
                    }
                },
                {
                    name: 'Volume',
                    type: 'bar',
                    yAxisIndex: 1,
                    data: volumeData,
                    itemStyle: {
                        color: '#91CC75',
                    },
                    barWidth: 10 
                }
            ],
            grid: {
                top: '13px',    
                bottom: '25px', 
                left: '10%',    
                right: '11%'    
            },
        };
    
        myChart.setOption(option);
    }
}