import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IndexFiidiiChartService } from './index-fiidii-chart.service';
import { IndexFiiDiiChartModel, IndexFiiDiiParameterModel } from './index-fiidii-chart.model';
import * as echarts from 'echarts';


@Component({
  selector: 'app-index-fiidii-chart',
  templateUrl: './index-fiidii-chart.component.html',
  styleUrls: ['./index-fiidii-chart.component.scss']
})
export class IndexFiidiiChartComponent implements OnInit {
  public indexFiiDiiChartModel: IndexFiiDiiChartModel = new IndexFiiDiiChartModel();
  public indexFiiDiiParameterModel: IndexFiiDiiParameterModel = new IndexFiiDiiParameterModel();
  public niftyChart: any; 
  public selectedRange: string = '1W';
  @ViewChild('chartContainer', { static: false }) chartContainer!: ElementRef;

  constructor(private indexFiidiiChartService: IndexFiidiiChartService) { }

  ngOnInit(): void {
    this.getChartData();
  }

  public getChartData() {
    this.indexFiidiiChartService.getForChart(this.indexFiiDiiParameterModel).subscribe((data) => {
      this.indexFiiDiiChartModel.dates = data.dates;
      this.indexFiiDiiChartModel.fiiSeriesData = data.fiiSeriesData;
      this.indexFiiDiiChartModel.diiSeriesData = data.diiSeriesData;
      this.indexFiiDiiChartModel.sensexSeriesData = data.sensexSeriesData;
      this.indexFiiDiiChartModel.niftySeriesData = data.niftySeriesData;

      this.updateChartOptions();
    });
  }

  updateChartOptions(): void {
    const colors = ['#5470C6', '#91CC75','#000000'];

    const fiiData = this.indexFiiDiiChartModel.fiiSeriesData;
    const diiData = this.indexFiiDiiChartModel.diiSeriesData;
  
    this.niftyChart = {
      color: colors,
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross'
        }
      },
      grid: {
      top: '15%', 
      right: '10%',
      bottom: '10%',
      left: '10%',
    },
      toolbox: {
        feature: {
        }
      },
      legend: {
        data: ['FII', 'DII', 'Nifty'],
      },
    xAxis: [
  {
    type: 'category',
    axisTick: {
      alignWithLabel: true
    },
    data: this.indexFiiDiiChartModel.dates.map(date => new Date(date).toLocaleDateString()), 
    axisLabel: {
      interval: 0, 
      rotate: 45, 
      formatter: (value: any) => {
        return value; 
      }
    }
  }
],
      yAxis: [
        {
          type: 'value',
          name: 'FII & DII',
          position: 'left',
          alignTicks: true,
          axisLine: {
            show: true,
            lineStyle: {
              color: colors[0]
            }
          },
          axisLabel: {
            formatter: '{value}'
          },
          min: -5000, 
          max: 5000,
          interval: 1000, 
          scale: true
        },
        {
          type: 'value',
          name: 'Nifty',
          position: 'right',
          axisLine: {
            show: true,
            lineStyle: {
              color: colors[2]
            }
          },
          axisLabel: {
            formatter: '{value}'
          },
          min: -3000, 
          max: 30000, 
          interval: 3000, 
        }
      ],
      series: [
        {
          name: 'FII',
          type: 'bar',
          data: fiiData,
          color: colors[0],
          itemStyle: {
            normal: {
              color: (params: any) => (params.value >= 0 ? colors[0] : '#5470C6'), 
            }
          }
        },
        {
          name: 'DII',
          type: 'bar',
          yAxisIndex: 0,
          data: diiData,
          color: colors[1],
          itemStyle: {
            normal: {
              color: (params: any) => (params.value >= 0 ? colors[1] : '#91CC75'), 
            }
          }
        },
        {
          name: 'Nifty',
          type: 'line',
          yAxisIndex: 1,
          data: this.indexFiiDiiChartModel.niftySeriesData,
          color: colors[2],

        }
      ]
    };
  }

  setDateRange(range: string): void {
    this.selectedRange = range;
    const today = new Date();
    if (range === '1W') {
      this.indexFiiDiiParameterModel.todayDate = new Date(today.setDate(today.getDate() - 7));
    } else if (range === '1M') {
      this.indexFiiDiiParameterModel.todayDate = new Date(today.setMonth(today.getMonth() - 1));
    } else {
      this.indexFiiDiiParameterModel.todayDate = new Date();
    }
    this.getChartData();
  }
  
}

