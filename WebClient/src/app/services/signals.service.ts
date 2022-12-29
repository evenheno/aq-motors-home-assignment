import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Logger } from 'src/class/logger';
import { ISignal } from 'src/dto';
import { APIService } from './api.service';

const logger = new Logger('SignalsService');

@Injectable({
  providedIn: 'root'
})
export class SignalsService {

  /** RxJS Subject. Components subscribes it to get updated
   * data when data fetched from server */
  public onSignalsReceived: Subject<Array<ISignal>>;
  /** Start interval fetcher */
  private _intervalMs!: number;
  /** Start interval fetcher */
  private _intervalRef?: NodeJS.Timer;

  constructor(private _apiService: APIService) {
    this._intervalMs = 10000;
    this.onSignalsReceived = new Subject();
    this._startAutoFetch();
    this.fetchSignals();
  }

  /** Start interval fetcher */
  private _startAutoFetch() {
    logger.action('Starting auto fetcher', { intervalMs: this._intervalMs });
    this._intervalRef = setInterval(this.fetchSignals.bind(this), this._intervalMs);
  }

  /** Reset interval fetcher */
  private _resetAutoFetch() {
    logger.action('Resetting auto fetcher');
    clearInterval(this._intervalRef);
    this._intervalRef = setInterval(this.fetchSignals.bind(this), this._intervalMs);
  }

  /** Fetch signals from API service */
  public async fetchSignals() {
    try {
      //Call API Service to fetch signals
      logger.action('Fetching signals')
      const signals = await this._apiService.get<Array<ISignal>>('signals');

      //Raise 'onSignalsReceived' subject to trigger all subscribers
      logger.action('Raising "onSignalsReceived" event')
      this.onSignalsReceived.next(signals);

      //Reset interval if there's an active instance
      if (this._intervalRef) { this._resetAutoFetch(); }

      return signals;
    } catch (error) {
      logger.error('Failed to fetch signals from server', error);
      return;
    }
  }
}
