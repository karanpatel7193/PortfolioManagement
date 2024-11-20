export class VolumeModel {
    public scriptId: number = 0;
    public scriptName: string = '';
    public volume: number = 0;
    public weekVolumeAverage: number = 0;
    public monthVolumeAverage: number = 0;
    public weekVolumePercentage: number = 0;
    public monthVolumePercentage: number = 0;
    public newsCount: number = 0; 
}

export class VolumeGridModel {
    public volumes: VolumeModel[] = [];
}
export class VolumeParameterModel {
    public dateTime: Date = new Date(0);
  }
