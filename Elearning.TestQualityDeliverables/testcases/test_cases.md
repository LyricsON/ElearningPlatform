# Test Cases Catalog

Notes:
- Columns `Actual Results`, `Status`, and `Evidence link` are meant to be updated after execution.
- Automated backend results are backed by TRX files in `Elearning.TestQualityDeliverables/reports/`.
- UI E2E tests require the Blazor app to be running locally.

| TestCaseID | RequirementID | Scenario | Level | Type | Technique | Preconditions | Test Data | Steps | Expected Results | Actual Results | Status | Evidence link |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| TC-001 | REQ-001 | Login returns JWT token | Integration | Functional | Black-box (equivalence partitioning) | API running in test host | New Instructor email/password | 1) Register 2) Login | 200 OK + token + role | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-002 | REQ-002 | Register rejects invalid role | Integration | Functional | Black-box (equivalence partitioning) | API running in test host | Role=`Admin` | 1) POST register | 400 Bad Request | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-003 | REQ-014 | Anonymous cannot access protected endpoints | Integration | Security | Black-box (access control) | API running in test host | None | 1) GET protected endpoint | 401/403 as designed | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-004 | REQ-011 | Admin can create category | Integration | Functional | Black-box (happy path) | API running in test host; admin token available | Seed Admin (`admin@elearning.local`) | 1) Login as Admin 2) POST category | 201/200 + created entity | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-005 | REQ-011 | Non-admin cannot create category | Integration | Security | Black-box (access control) | API running in test host; instructor token available | Instructor user | 1) Login as Instructor 2) POST category | 403 Forbidden | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-006 | REQ-003 | List categories works for anonymous | Integration | Functional | Black-box (happy path) | API running in test host | None | 1) GET categories | 200 OK + array | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-007 | REQ-004 | List courses works for anonymous | Integration | Functional | Black-box (happy path) | API running in test host | None | 1) GET courses | 200 OK + array | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/integration_tests.trx` |
| TC-008 | REQ-002 | Register DTO validation (email required) | Unit | Functional | White-box (validation attributes) | None | Empty email | 1) Validate DTO | Validation fails | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/unit_tests.trx` |
| TC-009 | REQ-002 | Register DTO validation (password min length) | Unit | Functional | White-box (boundary values) | None | Password length < 6 | 1) Validate DTO | Validation fails | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/unit_tests.trx` |
| TC-010 | REQ-001 | Login rejects unknown user | Unit | Security | White-box (negative path) | None (controller unit test) | Unknown email | 1) Call Login | 401 Unauthorized | Passed (TRX) | Pass | `Elearning.TestQualityDeliverables/reports/unit_tests.trx` |
| TC-011 | REQ-001 | UI: Home page loads | System | Functional | Black-box (smoke) | UI running at `E2E_BASE_URL` | None | 1) Open `/` | Home hero visible | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-012 | REQ-001 | UI: Invalid login shows error message | System | Functional | Black-box (equivalence partitioning) | UI running; login page exists | Invalid email/pass | 1) Open `/login` 2) Submit invalid creds | Error displayed | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-013 | REQ-004 | UI: Navigate to Courses from navbar | System | Functional | Black-box (smoke) | UI running | None | 1) Open `/` 2) Click Courses | Courses page loads | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-014 | REQ-005 | UI: Anonymous enroll redirects to login | System | Security | Black-box (access control) | UI running; course details page exists | Any course id | 1) Open course detail 2) Click Enroll | Redirect to login | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-015 | REQ-009 | UI: Take quiz happy path (seeded data) | System | Functional | Black-box (happy path) | UI running; seeded quiz exists | Seeded course/quiz/question | 1) Login 2) Start quiz 3) Submit | Score shown | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-016 | REQ-014 | UI: Admin page access requires Admin role | System | Security | Black-box (role-based access) | UI running; admin routes exist | Admin vs Instructor | 1) Login 2) Open `/admin` | Allowed only for Admin | Not executed | Not Executed | `Elearning.SeleniumTests/artifacts/` |
| TC-017 | REQ-001 | API: Swagger is reachable | System | Non-functional | Black-box (availability) | API running locally | None | 1) GET `/swagger/v1/swagger.json` | 200 OK | Not executed | Not Executed | (N/A) |
| TC-018 | REQ-004 | API performance smoke (response time) | System | Non-functional | Black-box (response time threshold) | API running locally | None | 1) GET `/swagger/v1/swagger.json` and measure | Response under threshold | Not executed | Not Executed | (N/A) |

