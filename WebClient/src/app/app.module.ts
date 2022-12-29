import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './components/app/app.component';
import { SignalsViewerComponent } from './components/signals-viewer/signals-viewer.component';
import { PipeTimestampString } from './pipes/timestamp-string.pipe';

@NgModule({
  declarations: [
    AppComponent,
    SignalsViewerComponent,
    PipeTimestampString
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }