export enum EnSignalType {
    Sine = 0,
    State = 1
}
export interface ISignal {
    signalType: EnSignalType
}