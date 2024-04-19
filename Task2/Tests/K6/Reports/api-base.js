import http from 'k6/http';

const apiBaseUrl = __ENV.URL || 'http://localhost:5019';

let accessToken;
let header;
let baseUrl;

export class ApiBase {
    constructor() {
        this.accessToken = "";
        this.baseUrl = apiBaseUrl;
        this.header = {
            Authorization: `Bearer ${this.accessToken}`,
            'Content-Type': 'application/json'
        };
    }

    SetAccessToken() {
        this.accessToken = getAccessToken();
        this.header.Authorization = `Bearer ${this.accessToken}`;
    }
}

