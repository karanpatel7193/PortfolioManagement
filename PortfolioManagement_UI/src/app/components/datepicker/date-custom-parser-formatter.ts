import { Component, Injectable } from '@angular/core';
import {
	NgbDateAdapter,
	NgbDateParserFormatter,
	NgbDateStruct,
} from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class CustomAdapter extends NgbDateAdapter<string> {
	readonly DELIMITER = '/';
    constructor(private maskController: MaskController) {
        super();
    }

	fromModel(value: string | null): NgbDateStruct | null {
		if (value) {
            if(this.maskController.mask == 'MM-yyyy') {
                const date = value.split('-');
                return {
                    day: 1,
                    month: getMonthNumber(date[0]) + 1,
                    year: parseInt(date[1], 10),
                };
            } else {
			const date = value.split(this.DELIMITER);
                return {
                    day: parseInt(date[1], 10),
                    month: parseInt(date[0], 10),
                    year: parseInt(date[2], 10),
                };
            }
		}
		return null;
	}

	toModel(date: NgbDateStruct | null): string | null {
        if (date) {
            if (this.maskController.mask == 'MM-yyyy') {
                return getMonthName(date.month - 1) + '-' + date.year;
            } else {
                return padNumber(date.month) + this.DELIMITER + padNumber(date.day) + this.DELIMITER + date.year;
            }
        } else {
            return null;
        }
	}
}

/**
 * This Service handles how the date is rendered and parsed from keyboard i.e. in the bound input field.
 */
@Injectable()
export class CustomDateParserFormatter extends NgbDateParserFormatter {
	readonly DELIMITER = '/';
    constructor(private maskController: MaskController) {
        super();
    }

	parse(value: string): NgbDateStruct | null {
		if (value) {
            if(this.maskController.mask == 'MM-yyyy') {
                const date = value.split('-');
                return {
                    day: 1,
                    month: getMonthNumber(date[0])+ 1,
                    year: parseInt(date[1], 10),
                };
            } else {
			const date = value.split(this.DELIMITER);
                return {
                    day: parseInt(date[1], 10),
                    month: parseInt(date[0], 10),
                    year: parseInt(date[2], 10),
                };
            }
		}
		return null;
	}

	format(date: NgbDateStruct | null): string {
		if (date) {
            if (this.maskController.mask == 'MM-yyyy') {
                return getMonthName(date.month - 1) + '-' + date.year;
            } else {
                return padNumber(date.month) + this.DELIMITER + padNumber(date.day) + this.DELIMITER + date.year;
            }
        } else {
            return '';
        }
	}
}

export function padNumber(value: number) {
  if (value) {
    return `0${value}`.slice(-2);
  } else {
    return "";
  }
}

export function getMonthName(value: number) {
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    return monthNames[value];
}

export function getMonthNumber(value: string) {
    const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    return monthNames.indexOf(value);
}

@Injectable({
    providedIn: 'root',
})
export class MaskController {
    mask: string = "MM/dd/yyyy";

    public setMask(mask: string) {
      this.mask = mask;
    }
}