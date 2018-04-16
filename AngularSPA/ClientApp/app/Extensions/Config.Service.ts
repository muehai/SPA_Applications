﻿import { Injectable } from '@angular/core'

@Injectable()
export class ConfigService {
    _apiUrl: string;

    constructor() {
        this._apiUrl = 'http://localhost:5080/api/';
    }

    getapiUrl() {
        return this._apiUrl;
    }

    getHost() {
        return this._apiUrl.replace('api/', '');
    }
}