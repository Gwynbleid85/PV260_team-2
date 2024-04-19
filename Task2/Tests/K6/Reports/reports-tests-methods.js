import http from 'k6/http';
import { check } from "k6";
import { ApiBase } from "./api-base.js";


let reportsApiUrl;
let reportId;

export class ReportsTests extends ApiBase {
	constructor() {
		super();
		reportsApiUrl = `${this.baseUrl}/reports`;
	}

	ReportsGetEmptyTest() {
		let res = http.get(`${reportsApiUrl}/history`);
		check(res, {
			"Response status 200": (r) => r.status === 200,
			"No reports found": (r) => r.json().length === 0
		});
	}

	ReportsPostTest() {
		let res = http.post(
			reportsApiUrl,
			"",
			{ headers: this.header }
		);

		let now = new Date();

		check(res, {
			"Response status 201": (r) => r.status === 200,
			"Create has correct year": (r) => r.json().year === now.getFullYear(),
			"Create has correct month": (r) => r.json().month === now.getMonth() +1
		});

		reportId = res.json().id;
	}

	ReportsGetTest() {
		let res = http.get(`${reportsApiUrl}/history`);
		check(res, {
			"Response status 200": (r) => r.status === 200,
			"Reports found": (r) => r.json().length > 0
		});
	}


	ReportsGetCurrentTest() {
		let res = http.get(`${reportsApiUrl}/current`);
		let now = new Date();

		check(res, {
			"Response status 200": (r) => r.status === 200,
			"Report found": (r) => r.json().id === reportId,
			"Report has correct year": (r) => r.json().year === now.getFullYear(),
			"Report has correct month": (r) => r.json().month === now.getMonth() +1
		});
	}

	ReportOldReportTest() {
		let res = http.get(`${reportsApiUrl}/three-months-old`);
		check(res, {
			// 404 is the correct status code for this test
			// Becase in db there is only report for currnet month
			"Response status 200": (r) => r.status === 404,
		});
	}


}