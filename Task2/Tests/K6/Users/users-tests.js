import { group } from "k6";
import { UsersTests } from "./users-tests-methods.js";

const tests = new UsersTests();

export default function () {
	group("Create test user", function () {
		tests.UsersCreateTestUser();
	});
	
	group("Subscribe user", function () {
		tests.UsersSubscribeUser();
	});

	group("Change user email", function () {
		tests.UsersChangeUserEmail();
	});

	group("Delete user", function () {
		tests.UsersDeleteUser();
	});
}