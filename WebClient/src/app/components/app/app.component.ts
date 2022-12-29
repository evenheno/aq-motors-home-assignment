import { Component } from '@angular/core';
import { Logger } from 'src/class/logger';
import { ISignal } from 'src/dto';
import { SignalsService } from 'src/app/services/signals.service';

const logger = new Logger('AppComponent');

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  /** Stores array of signals fetched from signal service */
  public signals?: Array<ISignal>;

  /**Stores timestamp of the latest data update */
  public lastUpdate?: number;

  public constructor(private _signalService: SignalsService) {
    logger.instance('AppComponent');
  }

  public async ngOnInit() {
    //Subscribe to 'onSignalsReceived' to receive data updates
    this._signalService.onSignalsReceived.subscribe((signals) => {
      //Store the local array with the updated data
      this.signals = signals;
      //Update the last time data updated
      this.lastUpdate = Date.now();
    });
  }

}
