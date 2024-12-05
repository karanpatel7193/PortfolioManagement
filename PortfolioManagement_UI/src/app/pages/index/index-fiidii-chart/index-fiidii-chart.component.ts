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
    this.getChartData(this.selectedRange);
  }

  public getChartData(range: string) {
    this.indexFiiDiiParameterModel.dateRange = range;
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
    const colors = ['#5470C6', '#91CC75', '#000000'];
  
    const fiiData = this.indexFiiDiiChartModel.fiiSeriesData;
    const diiData = this.indexFiiDiiChartModel.diiSeriesData;
    const niftyData = this.indexFiiDiiChartModel.niftySeriesData;
  
    const niftyMin = Math.min(...niftyData) - 200;
    const niftyMax = Math.max(...niftyData) + 200;
  
    this.niftyChart = {
      color: colors,
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross',
        },
        formatter: (params: any) => {
          const [fiiPoint, diiPoint] = params;
          const fiiValue = fiiPoint?.data || 0;
          const diiValue = diiPoint?.data || 0;
          const totalDifference = (fiiValue + diiValue).toFixed(2);
  
          return `
            <div>
              <b>Date:</b> ${fiiPoint.axisValue}<br/>
              <b>FII:</b> ${fiiValue}<br/>
              <b>DII:</b> ${diiValue}<br/>
              <b>Total (FII - DII):</b> ${totalDifference}
            </div>
          `;
        },
      },
      grid: {
        top: '7%',
        right: '6%',
        bottom: '5%',
        left: '6%',
      },
      toolbox: {
        feature: {},
      },
      legend: {
        data: ['FII', 'DII', 'Nifty'],
      },
      xAxis: [
        {
          type: 'category',
          axisTick: {
            alignWithLabel: true,
          },
          data: this.indexFiiDiiChartModel.dates.map(date => new Date(date).toLocaleDateString()),
        },
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
              color: colors[0],
            },
          },
          axisLabel: {
            formatter: '{value}',
          },
          min: -5000,
          max: 5000,
          interval: 1000,
          scale: true,
        },
        {
          type: 'value',
          name: 'Nifty',
          position: 'right',
          axisLine: {
            show: true,
            lineStyle: {
              color: colors[2],
            },
          },
          axisLabel: {
            formatter: '{value}',
          },
          min: niftyMin,
          max: niftyMax,
          interval: Math.ceil((niftyMax - niftyMin) / 5),
        },
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
            },
          },
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
            },
          },
        },
        {
          name: 'Nifty',
          type: 'line',
          yAxisIndex: 1,
          data: this.indexFiiDiiChartModel.niftySeriesData,
          color: colors[2],
          symbol: 'circle',
          symbolSize: 7,
          itemStyle: {
            normal: {
              color: (params: any) => {
                if (params.dataIndex === 0) {
                  return '#00FF00';
                }
                const previousValue = this.indexFiiDiiChartModel.niftySeriesData[params.dataIndex - 1];
                const currentValue = params.value;
                return currentValue >= previousValue ? '#00FF00' : '#FF0000';
              },
            },
          },
        },
      ],
    };
  }
  
  setDateRange(range: string): void {
    this.selectedRange = range;
    this.getChartData(range);
  }
  
}

