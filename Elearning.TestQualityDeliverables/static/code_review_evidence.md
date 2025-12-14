# Code Review Evidence (manual)

Scope reviewed (read-only review; no production code changes):
- `Elearning.Api/Controllers/*.cs`
- `Elearning.Api/Dtos/*.cs`
- `Elearning.Api/Data/SeedData.cs`
- `Elearning.Blazor/Pages/**/*.razor`
- `Elearning.Blazor/Services/*.cs`

## Review checklist (what was checked)

- API endpoints have clear routes and status codes
- Auth/roles are enforced via `[Authorize]` and per-endpoint checks
- DTO validation attributes exist and match UI expectations
- Error handling: meaningful messages without leaking secrets
- UI routing: pages exist for main flows and guard unauthorized access
- UI selectors: assess testability (data-testid availability)

## Findings (issues and recommendations)

### CR-001: Lack of stable UI selectors for automation
- Evidence: UI uses classes and structure-based selectors; no `data-testid` attributes found.
- Risk: UI E2E tests can become flaky if layout/classes change.
- Recommendation: add `data-testid` attributes to key elements (documented in `Elearning.SeleniumTests/README.md`).
- Fix: Not applied (guideline constraint: do not change production code).

### CR-002: Debug logging in production controllers/services
- Evidence: `Elearning.Api/Controllers/UsersController.cs` prints authentication claims to console.
- Risk: logs could leak sensitive token/claim info in production environments.
- Recommendation: use structured logging with proper log levels and remove debug prints before production.
- Fix: Not applied (guideline constraint: do not change production code).

### CR-003: Blazor navigation includes `/profile` but route not found
- Evidence: layout references `/profile`, but no `@page "/profile"` exists in `Elearning.Blazor/Pages`.
- Risk: broken navigation / 404 route.
- Recommendation: implement Profile page or remove link; current system test marks it as skipped until implemented.
- Fix: Not applied (guideline constraint: do not change production code).

### CR-004: Mixed language strings in UI can complicate selector strategy
- Evidence: some labels are French ("Mot de passe", "Se connecter") and others English.
- Risk: label-based selectors must match exact visible text.
- Recommendation: standardize labels or add `data-testid`.
- Fix: Not applied.

## Conclusion

The system is testable with a simple POM approach, but adding stable `data-testid` selectors would significantly improve reliability.

