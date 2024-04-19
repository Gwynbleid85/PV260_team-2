import { group } from "k6";
import { ReportsTests } from "./reports-tests-methods.js";

const tests = new ReportsTests();

export default function () {

	group("Get empty ", function () {
		tests.ReportsGetEmptyTest();
	});

	group("Generate new report", function () {
		tests.ReportsPostTest();
	});

	group("Get all reports after generated one", function () {
		tests.ReportsGetTest();
	});

	group("Get current report", function () {
		tests.ReportsGetCurrentTest();
	});

	group("Get three months old report", function () {
		tests.ReportOldReportTest();
	});
}