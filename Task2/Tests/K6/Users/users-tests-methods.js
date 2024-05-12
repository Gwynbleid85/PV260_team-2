import http from 'k6/http';
import { check } from "k6";
import { ApiBase } from "./api-base.js";


let usersApiUrl;
let userId;

export class UsersTests extends ApiBase {
	constructor() {
		super();
		usersApiUrl = `${this.baseUrl}/users`;
	}

	UsersCreateTestUser() {
		let res = http.post(
			usersApiUrl,
			"",
			{headers: this.header}
		);

		check(res, {
			"Response status 200":      (r) => r.status === 200,
		});

		userId = res.json().id;
		
	}

	UsersSubscribeUser() {
		const postUrl = `${usersApiUrl}/${userId}/subscription`;
		let res = http.post(
			postUrl,
			""
			,
			{headers: this.header}
		);
		
		check(res, {
			"Response status 200":      (r) => r.status === 200,
		});
		
		const getUrl = `${usersApiUrl}/${userId}`;
		res = http.get(
			getUrl,
			{headers: this.header}
		);
		
		check(res, {
			"Response status 200":      (r) => r.status === 200,
			"User is subscribed": (r) => r.json().isSubscribed === true,
		});
	}

	UsersChangeUserEmail() {
		const userEmail = 'newEmail@test.cz'
		const putUrl = `${usersApiUrl}/${userId}/email`;
		
		let res = http.put(
			putUrl,
			JSON.stringify({userEmail: userEmail}),
			{
				headers: this.header
			}
		);
		
		check(res, {
			"Response status 200":      (r) => r.status === 200,
		});
		
		const getUrl = `${usersApiUrl}/${userId}`;
		res = http.get(
			getUrl,
			{headers: this.header}
		);
		
		check(res, {
			"Response status 200": (r) => r.status === 200,
			"User has new email":  (r) => r.json().email === userEmail,
		});
	}

	UsersDeleteUser() {
		const deleteUrl = usersApiUrl + '/' + userId;
		
		let res = http.del(
			deleteUrl,
			'',
			{headers: this.header}
		);

		check(res, {
			"Response status 200":      (r) => r.status === 200,
		});
		
		const getUrl = usersApiUrl + '/' + userId;
		res = http.get(
			getUrl,
			{headers: this.header}
		);
		
		check(res, {
			"Response status 404":      (r) => r.status === 404,
		});
	}
}