import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import {
    NgbDate,
    NgbDateAdapter,
    NgbDateParserFormatter,
    NgbDateStruct
} from '@ng-bootstrap/ng-bootstrap';
import { DatePickerMode, DayOfMonth } from 'src/app/enum';
import { CustomAdapter, CustomDateParserFormatter, MaskController } from './date-custom-parser-formatter';

@Component({
    selector: 'app-datepicker',
    templateUrl: './datepicker.component.html',
    styleUrls: ['./datepicker.component.scss'],
    providers: [
        { provide: NgbDateAdapter, useClass: CustomAdapter },
        { provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter },
    ],
})
export class DatepickerComponent implements OnInit, OnChanges {
    public dateModel: string = '';
    public dateView: DatePickerMode = DatePickerMode.Date;
    public dateValue: any;
    public minDate!: NgbDateStruct;
    public maxDate!: NgbDateStruct;

    @Input() value?: Date;
    @Input() minValue?: string;
    @Input() maxValue?: string;
    @Input() minDateValue?: Date;
    @Input() maxDateValue?: Date;
    @Input() viewMode: DatePickerMode = DatePickerMode.Month;
    @Input() dayOfMonth: DayOfMonth = DayOfMonth.Start;
    @Input() toggleMonth: boolean = false;
    @Input() datepickerName: string = '';
    @Input() placeholder: string = 'mm/dd/yyyy';
    @Input() format: string = 'MM/dd/yyyy';
    
    @Output() onDateChange = new EventEmitter<Date>();

    constructor(private dateAdapter: NgbDateAdapter<string>, private dateFormatter: NgbDateParserFormatter, private maskController: MaskController) { }

    ngOnInit(): void {
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes['value'] != null) {
            if (this.value) {
                this.dateModel = this.dateFormatter.format(new NgbDate(this.value.getFullYear(), this.value.getMonth() + 1, this.value.getDate()));
                this.dateValue = { year: this.value.getFullYear(), month: this.value.getMonth() + 1 }
            }
        }
        if (changes['format'] != null) {
            this.maskController.setMask(this.format);
        }
        if (changes['minValue'] != null) {
            this.minDate = this.parseMinMaxValue(this.minValue);
        }
        if (changes['maxValue'] != null) {
            this.maxDate = this.parseMinMaxValue(this.maxValue);
        }
        if (changes['minDateValue'] != null) {
            this.minDate = this.parseMinMaxDateValue(this.minDateValue);
        }
        if (changes['maxDateValue'] != null) {
            this.maxDate = this.parseMinMaxDateValue(this.maxDateValue);
        }
    }
    public parseMinMaxDateValue(date?: Date): NgbDateStruct {
        if (date) {
            return { month: date.getMonth() + 1, day: date.getDate() + 1, year: date.getFullYear() }
        } else {
            return { month: 1, day: 1, year: 2001 }
        }
    }
    public parseMinMaxValue(date?: string): NgbDateStruct {
        if (date) {
            if (this.viewMode == DatePickerMode.Date) {
                let dateValue = date.split('-');
                return {
                    month: parseInt(dateValue[2], 10),
                    day: parseInt(dateValue[1], 10),
                    year: parseInt(dateValue[0], 10),
                }
            } else {
                let dateValue = date.split('-');
                return {
                    month: parseInt(dateValue[1], 10),
                    day: 1,
                    year: parseInt(dateValue[0], 10),
                }
            }
        } else {
            return { month: 1, day: 1, year: 2001 }
        }
    }

    public onChange(event: any) {
        if (event.current) {
            if (this.viewMode == DatePickerMode.Date) {
                let dateValue: NgbDateStruct | null = this.dateFormatter.parse(this.dateModel);
                if (dateValue != null)
                    this.value = new Date(dateValue.year, dateValue.month - 1, dateValue.day);

                this.onDateChange.emit(this.value);
            }
            else {
                if (this.dayOfMonth == DayOfMonth.Start) {
                    this.value = new Date(event.next.year, event.next.month - 1, 1);
                }
                else {
                    this.value = new Date(event.next.year, event.next.month, 0);
                }

                if (!this.toggleMonth)
                    this.onDateChange.emit(this.value);
            }
        }
    }

    public onDateSelected() {
        if (this.value)
            this.onDateChange.emit(this.value);
    }

    public preventKeys(event: any) {
        var charCode = event.keyCode;
        if (charCode == 8 || charCode == 46 || charCode == 37 || charCode == 39)
           return true;
        return false;
    }
}
