# Elearning.SeleniumTests (Python + Selenium + pytest)

Beginner-friendly UI + API automation project for:
- `Elearning.Blazor` (UI)
- `Elearning.Api` (API)

This project is **100% independent** and does **not** modify `Elearning.Api` or `Elearning.Blazor`.

## URLs (auto-detected from launchSettings.json)

- UI: `https://localhost:7051`
- API: `https://localhost:7075`

Override anytime via environment variables (see Configuration).

## Prerequisites

- Python **3.10+**
- Google Chrome installed
- Blazor UI + API running locally
  - UI must be reachable at `E2E_BASE_URL`
  - API must be reachable at `E2E_API_URL`

## Setup (Windows PowerShell)

From repo root:

```powershell
cd .\Elearning.SeleniumTests\
py -3.12 -m venv .venv
.\.venv\Scripts\Activate.ps1
python -m pip install --upgrade pip
pip install -r requirements.txt
```

## Run tests

Run everything:
```powershell
python -m pytest
```

## VS Code (pytest Test Explorer)

- Open folder: `Elearning.SeleniumTests` (not the repo root).
- Select interpreter: `.\Elearning.SeleniumTests\.venv\Scripts\python.exe`
- Configure tests: choose `pytest`
- Make sure `E2E_HEADLESS` is `false` (default) if you want to see Chrome.

Run only smoke tests:
```powershell
python -m pytest -m smoke
```

Run only auth tests:
```powershell
python -m pytest -m auth
```

## Configuration (env vars)

Supported env vars:

- `E2E_ENV` (default: `local`)
- `E2E_BASE_URL` (default: `https://localhost:7051`)
- `E2E_API_URL` (default: `https://localhost:7075`)
- `E2E_BROWSER` (default: `chrome`)
- `E2E_HEADLESS` (default: `false`) (set `true` in CI)
- `E2E_TIMEOUT_SECONDS` (default: `10`)
- `E2E_API_MAX_RESPONSE_MS` (default: `5000`) (used by simple perf smoke check)
- `E2E_USER_EMAIL` / `E2E_USER_PASSWORD` (optional; if not set, tests auto-register a Student user)
- `E2E_ADMIN_EMAIL` / `E2E_ADMIN_PASSWORD` (optional override for admin creds)

Example:
```powershell
$env:E2E_BASE_URL="https://localhost:7051"
$env:E2E_API_URL="https://localhost:7075"
$env:E2E_HEADLESS="false"
python -m pytest -m smoke
```

## How test data works

The API seeds these users (from `Elearning.Api/Data/SeedData.cs`):

| Role | Email | Password |
|---|---|---|
| Admin | `admin@elearning.local` | `Admin123!` |
| Instructor | `instructor@elearning.local` | `Instructor123!` |

Some UI tests need courses/lessons/quizzes data. Because the API does **not** seed those, the test suite creates a minimal dataset via the API (using `requests`) when needed:
- 1 category
- 1 course
- 2 lessons
- 1 quiz with 1 question

This is done in `core/test_data.py` (`ensure_minimum_course_content()`).

If you do not provide `E2E_USER_EMAIL`/`E2E_USER_PASSWORD`, the test suite auto-registers a new Student user via `POST /api/auth/register`.

## Test Coverage Matrix

| Test file | Markers | What it covers |
|---|---|---|
| `tests/test_smoke_navigation.py` | `smoke` | Home loads, nav to Courses |
| `tests/test_auth_positive_negative.py` | `auth` | Invalid login, empty-field validation, valid login + logout |
| `tests/test_courses_browse_enroll.py` | `courses` | Courses page loads, enroll redirect (anonymous), enroll (authenticated) |
| `tests/test_lessons_progress.py` | `lessons` | Lesson open, outline navigation, invalid lesson id |
| `tests/test_quiz_conditions.py` | `quiz` | Quiz requires login, take quiz happy path |
| `tests/test_role_based_access.py` | `admin` | Admin vs instructor vs anonymous access to admin pages |
| `tests/test_error_handling.py` | `smoke` | 404 route handling, protected UI route requires login |
| `tests/test_api_health_contract.py` | `api` | Swagger JSON availability, public vs protected endpoint behavior |
| `tests/test_api_performance_smoke.py` | `api`, `perf` | Simple API response-time smoke check |
| `tests/test_profile_validation.py` | `profile` | Placeholder (skips until `/profile` exists) |

## Selectors: current strategy + recommendation

Right now the Blazor UI does not expose stable `data-testid` hooks. The tests use:
- robust CSS selectors where possible (classes like `.auth-card`, `.course-card`)
- label-based lookup for form inputs (`pages/base_page.py::find_input_in_form_group_by_label`)

Recommended (suggestion only; do not change now if you don't want to):

- Add `data-testid` to key elements:
  - Login: `data-testid="login-email"`, `login-password`, `login-submit`, `login-error`
  - Courses: `courses-search`, `course-card`, `course-view`
  - Course details: `course-enroll`, `course-continue`, `course-lesson-open`, `course-quiz-start`
  - Quiz: `quiz-submit`, `quiz-score`
  - Admin tables: `admin-users-table`, `admin-categories-table`, `admin-courses-table`

When you add them, update page objects in `pages/` to use those attributes, and your tests become even more stable.

## Notes

- No `time.sleep()` is used. Only explicit waits (`WebDriverWait` + expected_conditions).
- On test failure, a screenshot + HTML page source are saved to `artifacts/screenshots/` (configurable via `E2E_ARTIFACTS_DIR`).
