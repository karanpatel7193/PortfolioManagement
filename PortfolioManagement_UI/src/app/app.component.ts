import { Component, TemplateRef } from '@angular/core';
import { ToastService } from './services/toast.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent {
	title = 'PortfolioManagementWeb';

	constructor(public toastService: ToastService) {
	}

	isTemplate(toast: any) {
		return toast.textOrTpl instanceof TemplateRef;
	}
}
