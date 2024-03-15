# Solution for `Task1`

## Assignment
```
Our client, the stock advisory company, provides services of stock market analyses,
stock monitoring and stock recommendation.

More and more of their clients ask for news about ARK invest ETFs funds and want to
know what CEO Cathie Wood buys and sells.

So far, their analysts go every month to https://ark-funds.com/funds/arkk/ download
the full holding in CSV, do the diff in Excel and send it via email to their clients.
The diff looks like this:

New positions:
Company name, ticker, #shares, weight(%)

Increased positions:
Company name, ticker, #shares (🔺x%), weight(%)

Reduced positions:
Company name, ticker, #shares (🔻x%), weight(%)

Your task is to work with the stock advisory company to help them automate it.
This company has been our customer for a long time. This is not a fixed scope project.
Both parties agree to cooperate and adjust plans as needed in an agile way.
Our ultimate goal is satisfied customer.
```
---
## Our solution

# Event storming

![Big picture ES](imgs/Big_picture.png)
---
![Process modeling ES](imgs/Process_modeling.png)

# User stories



## User Stories for Account Management

---
## User Registration

**As a prospective user, I want to register for an account, so that I can access restricted features.**

### Acceptance Criteria:
- Works with standard email and password combination.
- Provides password strength feedback.
- Confirms email address through verification email.

### Out of scope:
-  Registration using OAuth services like Google or Facebook.
- Two-factor authentication setup during initial registration.

### Notes:
- Ensure GDPR compliance for user data collection.

### Estimation:
Planning poker: 5 (considering standard complexity for user registration).

---
## User Login

**As a registered user, I want to be able to log in to my account, so that I can access my personal settings and data.**

### Acceptance Criteria:
- Works with email and password.
- Includes "Forgot Password" feature.

### Out of scope:
- Biometric login options.
- "Remember me" feature for device authorization.
- Supports OAuth login methods like Google or Facebook.

### Notes:
- Implement login rate-limiting to prevent brute force attacks.

### Estimation:
Planning poker: 3

---
## Change User Email

**As a logged-in user, I want to be able to change my registered email address, so that I can keep my contact information up to date.**

### Acceptance Criteria:
- Users can initiate email change from account settings.
- Verification of the new email address is required.
- Users are notified at their old email address about the change.
- Immediate session invalidation after email change for security.

### Out of scope:
- Changing other personal information at the same time as email.
- Email change via customer support.

### Notes:
- Any changes should be logged for audit purposes.
- Provide clear instructions for email verification process.

### Estimation:
Planning poker: 2 (straightforward process but requires proper security measures).

---
## Delete User Account

**As a user concerned about my privacy, I want the option to delete my account, so that all my data is removed from the system.**

### Acceptance Criteria:
- Provide a clear and easy way to initiate account deletion from settings.
- Confirm the deletion action via password.
- Inform the user about the data that will be removed and any data retention policies.

### Out of scope:
- Deleting user data that is required to be kept by law.

### Notes:
- Account deletion must comply with the right to be forgotten under GDPR.

### Estimation:
Planning poker: 8 (due to the need for careful handling of data and legal compliance).

