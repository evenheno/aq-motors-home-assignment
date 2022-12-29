import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Logger } from 'src/class/logger';

const logger = new Logger('APIService');

@Injectable({
    providedIn: 'root'
})

export class APIService {

    private _baseURL: string;

    public constructor(private _http: HttpClient) {
        this._baseURL = 'http://localhost:5032';
    }

    private _resolveURL(path: string) {
        return new URL(path, this._baseURL).toString();
    }

    public async get<T>(path: string, params?: HttpParams) {
        const url = this._resolveURL(path);
        logger.action('Executing GET Request...', { url: url, params: params });
        return new Promise<T>((resolve, reject) => {
            const method = this._http.get(url, { params: params });
            method.subscribe({
                next: (data) => {
                    logger.success('GET Request executed successfully', data)
                    resolve(data as T);
                },
                error: (error) => {
                    reject(error);
                }
            });
        });
    }

}
