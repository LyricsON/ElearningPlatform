from __future__ import annotations

import pytest
import requests
import warnings
from urllib3.exceptions import InsecureRequestWarning

from config.settings import E2E_API_URL

warnings.filterwarnings("ignore", category=InsecureRequestWarning)


@pytest.mark.api
class TestApiHealthContract:
    def test_swagger_is_available(self):
        try:
            resp = requests.get(f"{E2E_API_URL}/swagger/v1/swagger.json", timeout=10, verify=False)
        except Exception as exc:
            pytest.skip(f"API not reachable: {exc}")

        assert resp.status_code == 200
        data = resp.json()

        paths = data.get("paths", {})
        assert any("/api/Auth/login" in p or "/api/auth/login" in p for p in paths.keys())

    def test_public_endpoints_work_and_protected_requires_auth(self):
        try:
            cats = requests.get(f"{E2E_API_URL}/api/coursecategories", timeout=10, verify=False)
        except Exception as exc:
            pytest.skip(f"API not reachable: {exc}")

        assert cats.status_code == 200

        enrollments = requests.get(f"{E2E_API_URL}/api/enrollments", timeout=10, verify=False)
        assert enrollments.status_code in {401, 403}
