import { Component } from "@angular/core";

export enum EnColor {
    Default = '',
    Red = 'red',
    Green = 'lime',
    Cyan = 'cyan',
    Yellow = 'yellow'
}

export class Logger {

    private _moduleName: string;

    public constructor(moduleName: string) {
        this._moduleName = moduleName;
    }

    public log(message: string, color: EnColor = EnColor.Default, ...data: any) {
        console.log(`%c${this._moduleName}: ${message}`, `color:${color.toString()}`, ...data);
    }

    public success(message: string, ...data: any) {
        this.log(message, EnColor.Green, ...data);
    }

    public action(message: string, ...data: any){
        this.log(`${message}...`, EnColor.Yellow, ...data);
    }

    public info(message: string, ...data: any){
        this.log(`${message}`, EnColor.Cyan, ...data);
    }

    public instance(name: string, ...data: any){
        this.log(`Instance Created`, EnColor.Cyan, ...data);
    }

    public error(description: string, ...data: any){
        this.log(`ERROR: ${description}`, EnColor.Red, ...data);
    }
}