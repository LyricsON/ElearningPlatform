# Compliance Checklist (TEST ET QUALITÉ LOGICIEL)

## A) Static testing (mandatory)

- Static analysis (tool-based): `Elearning.TestQualityDeliverables/static/static_analysis_report.md`
- Code review evidence: `Elearning.TestQualityDeliverables/static/code_review_evidence.md`

## B) Functional tests

- Requirements-based tests: `Elearning.TestQualityDeliverables/testcases/test_cases.md`
- Confirmation tests: Backend unit/integration tests + key UI smoke flows
- Regression tests: pytest markers + dotnet test suites (repeatable)
- System tests: Selenium E2E suite in `Elearning.SeleniumTests/`
- At least one Selenium automated test: `Elearning.SeleniumTests/tests/test_smoke_navigation.py`
- Backend + frontend coverage:
  - Backend: `Elearning.DotNetTests/Elearning.Api.UnitTests/`, `Elearning.DotNetTests/Elearning.Api.IntegrationTests/`
  - Frontend: `Elearning.SeleniumTests/tests/`

## C) Non-functional tests (1+)

- Basic security: dependency vulnerability scan + role-based checks
  - Evidence: `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_api.log`, `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_blazor.log`
  - Automated access-control checks: integration tests + `Elearning.SeleniumTests/tests/test_role_based_access.py`
- Performance (simple response time): documented as TC-018 in `Elearning.TestQualityDeliverables/testcases/test_cases.md` (and implemented in `Elearning.SeleniumTests/tests/` if enabled)

## D) Testing techniques

- Techniques captured per test case: `Elearning.TestQualityDeliverables/testcases/test_cases.md`

## E) Mandatory test levels

- Unit: `Elearning.DotNetTests/Elearning.Api.UnitTests/` (+ TRX in `Elearning.TestQualityDeliverables/reports/unit_tests.trx`)
- Integration: `Elearning.DotNetTests/Elearning.Api.IntegrationTests/` (+ TRX in `Elearning.TestQualityDeliverables/reports/integration_tests.trx`)
- System: `Elearning.SeleniumTests/` (pytest + Selenium)

## F) Deliverables (in a Test folder)

- Static evidence: `Elearning.TestQualityDeliverables/static/`
- Test case tables: `Elearning.TestQualityDeliverables/testcases/test_cases.md`
- Traceability:
  - Requirements: `Elearning.TestQualityDeliverables/traceability/requirements.md`
  - Matrix: `Elearning.TestQualityDeliverables/traceability/traceability_matrix.md`
- Execution reports: `Elearning.TestQualityDeliverables/reports/`
- Evidence storage: `Elearning.TestQualityDeliverables/evidence/` (UI screenshots go to `Elearning.SeleniumTests/artifacts/`)
- AI usage disclosure: `Elearning.TestQualityDeliverables/reports/final_test_report.md`

