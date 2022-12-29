import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'timestampString'
})
export class PipeTimestampString implements PipeTransform {
    transform(value: number): string {
        console.log(value)
        return new Date(value).toLocaleString();
    }
}