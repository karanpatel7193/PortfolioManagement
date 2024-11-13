import { Injectable, TemplateRef } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
	toasts: any[] = [];
    toastConfigs: any = {
        autohide: true,
        delay: 10000,
        ariaLive: 'polite',
        animation: true
    };

	show(header: string, textOrTpl: string | TemplateRef<any>, options: any) {
        const toastConfigs = { ...this.toastConfigs, ...options };
		this.toasts.push({header, textOrTpl, ...toastConfigs });
	}

	success(message: string | TemplateRef<any>, header: string = '') {
        if (header == '')
            header = "Success!"
        let options: any = { classname: 'success' }
        this.show(header, message, options);
    }

	info(message: string | TemplateRef<any>, header: string = '') {
        if (header == '')
            header = "Info!"
        let options: any = { classname: 'info' }
        this.show(header, message, options);
	}

	warning(message: string | TemplateRef<any>, header: string = '') {
        if (header == '')
            header = "Warning!"
        let options: any = { classname: 'warning' }
        this.show(header, message, options);
	}

	error(message: string | TemplateRef<any>, header: string = '') {
        if (header == '') {
            header = "Error!";
        }
        let options: any = { classname: 'error' }
        this.show(header, message, options);
	}

	remove(toast: any) {
		this.toasts = this.toasts.filter((t) => t !== toast);
	}

	clear() {
		this.toasts.splice(0, this.toasts.length);
	}
}