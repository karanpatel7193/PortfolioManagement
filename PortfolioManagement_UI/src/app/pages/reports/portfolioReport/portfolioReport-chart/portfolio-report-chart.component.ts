import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { PortfolioDonutChartModel} from '../portfolio-report.model';
import { bottom } from '@popperjs/core';


@Component({
    selector: 'app-portfoilo-report-chart',
    templateUrl: './portfolio-report-chart.component.html',
    styleUrls: ['./portfolio-report-chart.component.scss']
})
export class PortfolioReportChartComponent implements OnInit, OnChanges {
    @Input() chartData: PortfolioDonutChartModel[] = [];
    chartOption: any;

    constructor() { }

    ngOnInit(): void {
        this.updateChartOptions()
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes['chartData']) {
        this.updateChartOptions();
    }
    }

    public updateChartOptions() {
        const totalValue = this.chartData.reduce((sum, item) => sum + item.value, 0);
        this.chartOption = {
            tooltip: {
                trigger: 'item'
            },
            title: { 
                show: false 
            },
            legend: {
                orient: 'horizontal',
                left: 'left',
                top:230,
                formatter: (name: string) => {
                    const item = this.chartData.find(data => data.name === name);
                    const percentage = ((item!.value / totalValue) * 100).toFixed(2);
                    return `${name} (${percentage}%)`;
                }
            },
            series: [
                {
                    type: 'pie',
                    radius: ['50%', '25%'],
                    center: ['50%', '100'],
                    avoidLabelOverlap: false,
                    label: { 
                        show: false,
                        position: 'center'
                    },
                    data: this.chartData
                }
            ],
            grid: {
                left: 65,
                top: 10,
                right: 35,
                bottom: 20,
            },
            emphasis: {
                label: {
                    show: false,
                }
            },
            labelLine: {
                show: false
            },
        }
        };
    }

    

