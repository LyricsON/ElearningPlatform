# GAP Analysis vs Guideline (before → after)

## Initial repo state (before this work)

Found projects:
- `Elearning.Api` (ASP.NET Core Web API)
- `Elearning.Blazor` (Blazor UI)

Initial testing gaps:
- No backend unit test project
- No backend integration test project
- No formal static testing evidence
- No documented test case catalog (technique/level)
- No traceability matrix
- No execution reporting folder structure

## What is implemented now

### A) Static testing
- Tool-based static analysis evidence:
  - `Elearning.TestQualityDeliverables/static/static_analysis_report.md`
  - `Elearning.TestQualityDeliverables/static/dotnet_build_release.log`
  - `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_api.log`
  - `Elearning.TestQualityDeliverables/static/dotnet_vulnerable_blazor.log`
- Code review evidence:
  - `Elearning.TestQualityDeliverables/static/code_review_evidence.md`

### B/E) Test levels (Unit / Integration / System)
- Unit (backend): `Elearning.DotNetTests/Elearning.Api.UnitTests/`
  - Execution: `Elearning.TestQualityDeliverables/reports/unit_tests.trx`
- Integration (backend API): `Elearning.DotNetTests/Elearning.Api.IntegrationTests/`
  - Execution: `Elearning.TestQualityDeliverables/reports/integration_tests.trx`
- System (E2E UI): `Elearning.SeleniumTests/`
  - Requires running UI/API locally; evidence saved under `Elearning.SeleniumTests/artifacts/`

### D) Testing techniques
- Technique mapped per test case in:
  - `Elearning.TestQualityDeliverables/testcases/test_cases.md`

### F) Deliverables + traceability
- Folder structure: `Elearning.TestQualityDeliverables/`
- Requirements list: `Elearning.TestQualityDeliverables/traceability/requirements.md`
- Traceability matrix: `Elearning.TestQualityDeliverables/traceability/traceability_matrix.md`
- Execution summary: `Elearning.TestQualityDeliverables/reports/execution_summary.md`
- Final report: `Elearning.TestQualityDeliverables/reports/final_test_report.md`
- Compliance checklist: `Elearning.TestQualityDeliverables/reports/compliance_checklist.md`

## Remaining items (if you want to go further)

- Run Selenium E2E suite and attach screenshots/log evidence for key scenarios in `Elearning.TestQualityDeliverables/evidence/`.
- Extend non-functional coverage (optional): more API response-time checks, simple accessibility checks, multi-browser smoke.

