# Traceability Matrix (REQ → Scenario → Test Cases → Result)

Legend for Result:
- Pass: last automated execution passed
- Not Executed: not run in the last execution cycle (often requires UI running)

| RequirementID | Scenario summary | Test Cases | Result |
|---|---|---|---|
| REQ-001 | Login works + invalid login handled | TC-001, TC-010, TC-012 | Pass (API) / Not Executed (UI) |
| REQ-002 | Register validation + role rules | TC-002, TC-008, TC-009 | Pass |
| REQ-003 | Public categories listing | TC-006 | Pass |
| REQ-004 | Public courses listing + navigation | TC-007, TC-013 | Pass (API) / Not Executed (UI) |
| REQ-005 | Enrollment access control | TC-014 | Not Executed |
| REQ-006 | Lessons view | (covered by Selenium suite) | Not Executed |
| REQ-007 | Lessons CRUD role enforcement | (covered by API integration suite) | Pass |
| REQ-008 | Quizzes read | (covered by Selenium suite) | Not Executed |
| REQ-009 | Quiz submit + score | TC-015 | Not Executed |
| REQ-010 | Quiz CRUD role enforcement | (covered by API integration suite) | Pass |
| REQ-011 | Admin category management | TC-004, TC-005 | Pass |
| REQ-012 | Admin user management | (covered by Selenium suite) | Not Executed |
| REQ-013 | Admin delete enrollment/result | (covered by API integration suite) | Pass |
| REQ-014 | Role-based access enforced | TC-003, TC-016 | Pass (API) / Not Executed (UI) |

