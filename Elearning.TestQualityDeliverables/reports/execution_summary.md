# Execution Summary

## Latest backend automation run (local)

Artifacts:
- Unit: `Elearning.TestQualityDeliverables/reports/unit_tests.trx`
- Integration: `Elearning.TestQualityDeliverables/reports/integration_tests.trx`

Summary (from latest TRX generation):
- Unit tests: 4 passed, 0 failed
- Integration tests: 6 passed, 0 failed

## UI automation run (Selenium)

UI E2E requires you to start:
- Blazor UI at `https://localhost:7051`
- API at `https://localhost:7075`

Then run:
- `cd Elearning.SeleniumTests`
- `python -m pytest` (or `python -m pytest -m smoke`)

Evidence on failure:
- Screenshots + HTML dumps go to `Elearning.SeleniumTests/artifacts/` by default.

