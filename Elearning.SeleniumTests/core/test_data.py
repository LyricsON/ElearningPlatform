from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timezone
from typing import Any

import pytest
import requests
import warnings
from urllib3.exceptions import InsecureRequestWarning

from config.settings import (
    E2E_ADMIN_EMAIL,
    E2E_ADMIN_PASSWORD,
    E2E_API_URL,
    E2E_USER_EMAIL,
    E2E_USER_PASSWORD,
)

warnings.filterwarnings("ignore", category=InsecureRequestWarning)


@dataclass(frozen=True)
class Credentials:
    email: str
    password: str


def admin_credentials() -> Credentials:
    """
    Uses env vars if provided, otherwise falls back to the API seed user.
    """
    return Credentials(
        email=E2E_ADMIN_EMAIL or "admin@elearning.local",
        password=E2E_ADMIN_PASSWORD or "Admin123!",
    )


def instructor_credentials() -> Credentials:
    """
    Instructor seed user (used for creating test data via API).
    """
    return Credentials(
        email="instructor@elearning.local",
        password="Instructor123!",
    )


def require_env_user_credentials() -> Credentials:
    if not E2E_USER_EMAIL or not E2E_USER_PASSWORD:
        pytest.skip("Set E2E_USER_EMAIL and E2E_USER_PASSWORD to run this test.")
    return Credentials(E2E_USER_EMAIL, E2E_USER_PASSWORD)


def user_credentials() -> Credentials:
    """
    Regular UI user (Student) for end-to-end flows.

    Priority:
    1) If E2E_USER_EMAIL / E2E_USER_PASSWORD are set, use them.
    2) Otherwise, auto-register a new Student user via the API.
    """
    global _CACHED_USER
    if _CACHED_USER is not None:
        return _CACHED_USER

    if E2E_USER_EMAIL and E2E_USER_PASSWORD:
        _CACHED_USER = Credentials(E2E_USER_EMAIL, E2E_USER_PASSWORD)
        return _CACHED_USER

    api_url = E2E_API_URL
    if not api_url:
        pytest.skip("E2E_API_URL not set; cannot auto-register a test user.")

    stamp = datetime.now(timezone.utc).strftime("%Y%m%d%H%M%S")
    email = f"e2e.student.{stamp}@example.test"
    password = "Student123!"

    payload = {
        "firstName": "E2E",
        "lastName": "Student",
        "email": email,
        "role": "Student",
        "password": password,
        "confirmPassword": password,
    }

    try:
        resp = requests.post(
            f"{api_url}/api/auth/register",
            json=payload,
            timeout=10,
            verify=False,
        )
        if resp.status_code not in (200, 201):
            raise RuntimeError(f"Register failed ({resp.status_code}): {resp.text}")
    except Exception as exc:
        pytest.skip(f"Could not auto-register test user: {exc}")

    _CACHED_USER = Credentials(email=email, password=password)
    return _CACHED_USER


def api_login(api_url: str, creds: Credentials) -> str:
    resp = requests.post(
        f"{api_url}/api/auth/login",
        json={"email": creds.email, "password": creds.password},
        timeout=10,
        verify=False,  # local dev certs
    )
    if resp.status_code != 200:
        raise RuntimeError(f"API login failed ({resp.status_code}): {resp.text}")
    data = resp.json()
    token = data.get("token")
    if not token:
        raise RuntimeError("API login response missing token.")
    return token


def api_session(api_url: str, token: str | None = None) -> requests.Session:
    session = requests.Session()
    session.verify = False  # local dev certs
    session.headers.update({"Accept": "application/json"})
    if token:
        session.headers.update({"Authorization": f"Bearer {token}"})
    session.base_url = api_url  # type: ignore[attr-defined]  # simple convenience
    return session


def _post(session: requests.Session, path: str, payload: dict[str, Any]) -> dict[str, Any]:
    resp = session.post(f"{session.base_url}{path}", json=payload, timeout=10)  # type: ignore[attr-defined]
    if resp.status_code not in (200, 201):
        raise RuntimeError(f"POST {path} failed ({resp.status_code}): {resp.text}")
    return resp.json() if resp.text else {}


def _get_json(session: requests.Session, path: str) -> Any:
    resp = session.get(f"{session.base_url}{path}", timeout=10)  # type: ignore[attr-defined]
    if resp.status_code != 200:
        raise RuntimeError(f"GET {path} failed ({resp.status_code}): {resp.text}")
    return resp.json()


def ensure_minimum_course_content() -> dict[str, int]:
    """
    Creates (via API) a minimal set of data for UI tests:
    - 1 category
    - 1 course (owned by instructor)
    - 2 lessons
    - 1 quiz with 1 question

    Returns IDs: category_id, course_id, lesson1_id, lesson2_id, quiz_id
    """
    global _CACHED_CONTENT
    if _CACHED_CONTENT is not None:
        return _CACHED_CONTENT

    api_url = E2E_API_URL
    if not api_url:
        pytest.skip("E2E_API_URL not set and no default environment API URL found.")

    try:
        admin_token = api_login(api_url, admin_credentials())
        instructor_token = api_login(api_url, instructor_credentials())
    except Exception as exc:
        pytest.skip(f"API not reachable or login failed: {exc}")

    admin = api_session(api_url, admin_token)
    instructor = api_session(api_url, instructor_token)

    stamp = datetime.now(timezone.utc).strftime("%Y%m%d%H%M%S")
    category = _post(admin, "/api/coursecategories", {"name": f"E2E Category {stamp}"})
    category_id = int(category["id"])

    course = _post(
        instructor,
        "/api/courses",
        {
            "title": f"E2E Course {stamp}",
            "description": "Course created by automated tests.",
            "categoryId": category_id,
            "instructorId": 0,  # API overrides this for non-admin
            "thumbnailUrl": "",
            "difficultyLevel": "Beginner",
        },
    )
    course_id = int(course["id"])

    lesson1 = _post(
        instructor,
        "/api/lessons",
        {"courseId": course_id, "title": "E2E Lesson 1", "videoUrl": "", "durationMinutes": 5, "orderNumber": 1},
    )
    lesson2 = _post(
        instructor,
        "/api/lessons",
        {"courseId": course_id, "title": "E2E Lesson 2", "videoUrl": "", "durationMinutes": 5, "orderNumber": 2},
    )

    quiz = _post(
        instructor,
        "/api/quizzes",
        {"courseId": course_id, "title": "E2E Quiz 1", "description": "Quiz created by automated tests."},
    )
    quiz_id = int(quiz["id"])

    _post(
        instructor,
        "/api/quizquestions",
        {
            "quizId": quiz_id,
            "questionText": "E2E Question: pick A",
            "optionA": "A",
            "optionB": "B",
            "optionC": "C",
            "optionD": "D",
            "correctAnswer": "A",
        },
    )

    # Confirm course details is reachable (optional sanity)
    _get_json(instructor, f"/api/courses/{course_id}")

    _CACHED_CONTENT = {
        "category_id": category_id,
        "course_id": course_id,
        "lesson1_id": int(lesson1["id"]),
        "lesson2_id": int(lesson2["id"]),
        "quiz_id": quiz_id,
    }
    return _CACHED_CONTENT


_CACHED_CONTENT: dict[str, int] | None = None
_CACHED_USER: Credentials | None = None
