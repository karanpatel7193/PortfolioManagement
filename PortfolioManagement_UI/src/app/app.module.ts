import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxEchartsModule } from 'ngx-echarts';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SpinnerModule } from './components/spinner/spinner.module';
import { ApiInterceptor } from './interceptors/api.interceptor';
import { HttpService } from './services/http.service';
import { ScriptService } from './pages/master/script/script.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    NgbModule,
    AppRoutingModule,
    SpinnerModule,
    BrowserAnimationsModule,
    
    
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
  ],
  providers: [
	  { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
	  HttpService,
    ScriptService

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
