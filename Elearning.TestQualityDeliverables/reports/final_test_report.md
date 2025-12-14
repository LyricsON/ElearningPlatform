# Final Test Report (Test & Quality)

## Scope

Projects:
- `Elearning.Api` (ASP.NET Core Web API)
- `Elearning.Blazor` (Blazor UI)

Test assets created:
- Backend unit tests: `Elearning.DotNetTests/Elearning.Api.UnitTests/`
- Backend integration tests: `Elearning.DotNetTests/Elearning.Api.IntegrationTests/`
- UI E2E tests (Selenium): `Elearning.SeleniumTests/`

## Static Testing (mandatory)

Static analysis (tool-based):
- `Elearning.TestQualityDeliverables/static/static_analysis_report.md`
- `Elearning.TestQualityDeliverables/static/dotnet_build_release.log`
- `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_api.log`
- `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_blazor.log`

Code review evidence (manual):
- `Elearning.TestQualityDeliverables/static/code_review_evidence.md`

## Functional Testing

Backend (automated):
- Unit tests: DTO validation + controller negative paths
- Integration tests: register/login, role-based access, catalog endpoints

Frontend (automated, Selenium):
- Smoke navigation, login positive/negative, course browsing/enrollment, lessons/quizzes (as implemented in `Elearning.SeleniumTests/tests/`)

## Non-Functional Testing (selected)

Chosen categories:
- Basic security: dependency vulnerability scan + role-based access checks (API + UI)
- Performance (simple): recommended API response-time smoke check (see TC-018 in `testcases/test_cases.md`)

## Known limitations / recommendations

- UI selectors: add `data-testid` attributes for stability (recommended in `Elearning.SeleniumTests/README.md`; no production code changes were made).
- UI E2E execution requires the app to be running locally (Selenium cannot start your .NET apps automatically here).

## AI usage disclosure

Automation code and documentation in:
- `Elearning.SeleniumTests/`
- `Elearning.DotNetTests/`
- `Elearning.TestQualityDeliverables/`

were generated/assembled with AI assistance (structure, starter tests, and documentation templates), then validated locally by running `dotnet test` for Unit/Integration tests and ensuring Selenium suite is runnable with explicit waits and screenshot-on-failure.

