import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IndexChartService } from './index-chart.service';
import { IndexChartGridModel, IndexChartModel, IndexChartParameterModel } from './index-chart.model';

@Component({
  selector: 'app-index-chart',
  templateUrl: './index-chart.component.html',
  styleUrls: ['./index-chart.component.scss']
})
export class IndexChartComponent implements OnInit {
  public indexChartModel: IndexChartModel = new IndexChartModel();
  public indexChartGridModel: IndexChartGridModel = new IndexChartGridModel();
  public indexChartParameterModel: IndexChartParameterModel = new IndexChartParameterModel();
  @ViewChild('chartContainer', { static: false }) chartContainer!: ElementRef;

  selectedRange: string = '';
  indexChart: any;

  constructor(private indexChartService: IndexChartService) { }

  ngOnInit(): void {
    this.selectedRange = '1W'; 
    this.getScriptData(this.selectedRange);
  }

  public getScriptData(range: string): void {
    this.indexChartParameterModel.dateRange = range;
    this.indexChartService.getForIndexChart(this.indexChartParameterModel).subscribe((data) => {
      this.indexChartGridModel.dates = data.dates;
      this.indexChartGridModel.niftySeriesData = data.niftySeriesData;
      this.indexChartGridModel.sensexSeriesData = data.sensexSeriesData;

      this.updateChartOptions();
    });
  }

  updateChartOptions(): void {
    const colors = ['blue', 'green'];
    const niftyMin = Math.min(...this.indexChartGridModel.niftySeriesData);
    const niftyMax = Math.max(...this.indexChartGridModel.niftySeriesData);
    const sensexMin = Math.min(...this.indexChartGridModel.sensexSeriesData);
    const sensexMax = Math.max(...this.indexChartGridModel.sensexSeriesData);

    // const formattedDates = this.indexChartGridModel.dates.map(date => {
    //   const formattedDate = new Date(date);
    //   if (this.selectedRange === '1D') {
    //     return formattedDate.toLocaleString(); // Includes both date and time
    //   } else {
    //     return formattedDate.toLocaleDateString(); // Only the date
    //   }
    // });

    this.indexChart = {
      color: colors,
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross'
        }
      },
      grid: {
        right: '6%',
        left: '6%',
        top: '7%',
        bottom: '5%'
      },
      legend: {
        data: ['Nifty', 'Sensex']
      },
      xAxis: [
        {
          type: 'category',
          axisTick: {
            alignWithLabel: true
          },
          data: this.indexChartGridModel.dates // Use the formatted dates
        }
      ],
      yAxis: [
        {
          type: 'value',
          name: 'Nifty',
          min: niftyMin,
          max: niftyMax,
          position: 'left',
          alignTicks: false,
          axisLine: {
            show: true,
            lineStyle: {
              color: colors[0]
            }
          },
          axisLabel: {
            formatter: (value: number) => value.toFixed(2)
          }
        },
        {
          type: 'value',
          name: 'Sensex',
          min: sensexMin,
          max: sensexMax,
          position: 'right',
          alignTicks: false,
          axisLine: {
            show: true,
            lineStyle: {
              color: colors[1]
            }
          },
          axisLabel: {
            formatter: (value: number) => value.toFixed(2)
          }
        }
      ],
      series: [
        {
          name: 'Nifty',
          type: 'line',
          data: this.indexChartGridModel.niftySeriesData,
        },
        {
          name: 'Sensex',
          type: 'line',
          yAxisIndex: 1,
          data: this.indexChartGridModel.sensexSeriesData,
        }
      ],
      dataZoom: [{ type: 'inside' }]
    };
  }

  setDateRange(range: string): void {
    this.selectedRange = range;
    // Pass the selected range to the backend via getScriptData
    this.getScriptData(range);
  }
}
