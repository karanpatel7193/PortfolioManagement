import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ScriptViewChartService } from './script-view-chart.service';
import { ActivatedRoute } from '@angular/router';
import { ScriptViewChartModel, ScriptViewParameterModel, ScriptViewPriceModel} from './script-view-chart.model';
import { bottom, right } from '@popperjs/core';

@Component({
    selector: 'app-script-view-chart',
    templateUrl: './script-view-chart.component.html',
    styleUrls: ['./script-view-chart.component.scss']
})
export class ScriptViewChartComponent implements OnInit {
    @Input() id!: number;
    public scriptviewChartModel: ScriptViewChartModel = new ScriptViewChartModel();
    public scriptviewParameterModel: ScriptViewParameterModel = new ScriptViewParameterModel();
    selectedRange: string = '1M';
    chartOption: any;

    constructor(private scriptviewChartService: ScriptViewChartService, private route: ActivatedRoute) {}

    ngOnInit(): void {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if(changes['id'] && changes['id'].currentValue){
            this.setDateRange('1M')
            this.getScriptData();
        }
    }
    public setDateRange(range: string) {
        this.selectedRange = range;
        const today = new Date();
        this.scriptviewParameterModel.fromDate = new Date(today);
        this.scriptviewParameterModel.toDate = new Date(today);
    
        switch (range) {
            case '1D':
                this.scriptviewParameterModel.fromDate.setDate(today.getDate());
                break;
            case '1W':
                this.scriptviewParameterModel.fromDate.setDate(today.getDate() - 7);
                break;
            case '1M':
                this.scriptviewParameterModel.fromDate.setMonth(today.getMonth() - 1);
                break;
            case '1Y':
                this.scriptviewParameterModel.fromDate.setFullYear(today.getFullYear() - 1);
                break;
            }
        this.getScriptData();
    }
    
    public getScriptData() {
        this.scriptviewParameterModel.scriptId = this.id ;
        this.scriptviewChartService.getForChart(this.scriptviewParameterModel).subscribe((data) => {
            this.scriptviewChartModel = data;
            this.updateChartOptions(this.scriptviewChartModel);  
        });
    }   

    public updateChartOptions(scriptviewChartModel: ScriptViewChartModel) {
    
        const scriptName = this.scriptviewChartModel.script.name;
    
        let minPrice = Math.floor(Math.min(...scriptviewChartModel.prices.map(price => price.low)));
        let maxPrice = Math.ceil(Math.max(...scriptviewChartModel.prices.map(price => price.high)));
    
        this.chartOption = {
            tooltip: {
                trigger: 'axis',
                className: 'custom-tooltip',
                formatter: (params: any) => {
                    const candlestickData = params[0].data;
                    const volumeData = this.scriptviewChartModel.volumeSeriesData[params[0].dataIndex];
                    return `
                        <div>
                            <strong>Open:</strong> ${candlestickData[1]} <br>
                            <strong>Close:</strong> ${candlestickData[2]} <br>
                            <strong>Low:</strong> ${candlestickData[4]} <br>
                            <strong>High:</strong> ${candlestickData[3]} <br>
                            <strong>Volume:</strong> ${volumeData}
                        </div>
                    `;
                },
                axisPointer: {
                    type: 'cross',
                    label: {
                        backgroundColor: '#6a7985'
                    }
                }
            },
            xAxis: [{
                    type: 'time',
                    data: this.scriptviewChartModel.dates,
                    boundaryGap: true,
                },
                {
                    type: 'category',
                    gridIndex: 1,
                    data: this.scriptviewChartModel.dates,
                    splitLine: { show: false },
                    axisLabel: { show: false },
                    axisTick: { show: false },
                    // axisLine: { lineStyle: { color: '#777' } },
                    axisLine: { show: false },  
                    axisPointer: {
                        type: 'shadow',
                        label: { show: true },
                        triggerTooltip: true,
                        handle: {
                        show: false,
                        margin : 50,
                        color: '#B80C00',
                        },
                    },
                },
            ],
            yAxis: [
                {
                    scale: true,
                    splitLine: { show: true },
                    axisLine: { show: false },
                    axisTick: { show: false },
                },
                {
                    scale: true,
                    gridIndex: 1,
                    splitNumber: 2,
                    min: minPrice,
                    max: maxPrice,
                    axisLabel: { show: false },
                    axisLine: { show: false },
                    axisTick: { show: false },
                    splitLine: { show: false },
                },
            ],
            grid: [
                {
                    left: 40,
                    right: 40,
                    top: 20,
                    bottom: 40,
                    height: 350,
                },
                {
                    left: 60,
                    right: 60,
                    height: 40,
                    top: 330,
                },
            ],
            series: [
                {
                    name: 'Volume',
                    type: 'bar',
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    emphasis: {
                      itemStyle: {
                        color: '#140',
                      },
                    },
                    barWidth: 30, 
                    data: scriptviewChartModel.volumeSeriesData,
                },
                {
                    name: '',
                    type: 'candlestick',
                    dimensions: ['date', 'open', 'close', 'highest', 'lowest'],
                    data: this.scriptviewChartModel.candelSeriesData,
                    itemStyle: {
                        color: '#47b262', // positive
                        color0: '#eb5454', // negative
                        borderColor: '#47b262',
                        borderColor0: '#eb5454',
                        borderColorDoji: null, 
                        borderWidth: 1
                    },
                    barWidth: 23, 
                },
            ],
            dataZoom: [
                {
                    type: 'inside',
                    start: 1,
                    end: 100
                }
            ]
        };
    }
}
