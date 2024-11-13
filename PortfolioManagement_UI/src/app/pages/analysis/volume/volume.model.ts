export class VolumeModel {
    scriptId: number = 0;
    scriptName: string = '';
    volume: number = 0;
    weekAverageVolume: number = 0;
    monthAverageVolume: number = 0;
    weekPercentage: number = 0;
    monthPercentage: number = 0;
    newsCount: number = 0; 
}

export class VolumeGridModel {
    volumes: VolumeModel[] = [];
}
export class VolumeParameterModel {
    dateTime: Date = new Date(0);
  }
