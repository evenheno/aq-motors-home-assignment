import { Component, Input } from '@angular/core';
import { EnSignalType, IDTSine, IDTState, ISignal } from 'src/dto';

@Component({
  selector: 'app-signals-viewer',
  templateUrl: './signals-viewer.component.html',
  styleUrls: ['./signals-viewer.component.scss']
})
export class SignalsViewerComponent {

  @Input() public data?: Array<ISignal>;

  asSine = (signal: ISignal) => signal as IDTSine;
  asState = (signal: ISignal) => signal as IDTState;
  isSine = (type: number) => type === EnSignalType.Sine;
  isState = (type: number) => type === EnSignalType.State;

}
