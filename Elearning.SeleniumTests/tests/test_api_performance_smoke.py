from __future__ import annotations

import time

import pytest
import requests

from config.settings import E2E_API_MAX_RESPONSE_MS, E2E_API_URL


@pytest.mark.api
@pytest.mark.perf
def test_swagger_response_time_is_reasonable():
    """
    Simple performance smoke check (non-functional).

    - Uses a generous default threshold to avoid flakiness.
    - Override threshold via E2E_API_MAX_RESPONSE_MS.
    """
    url = f"{E2E_API_URL}/swagger/v1/swagger.json"

    start = time.perf_counter()
    resp = requests.get(url, timeout=10, verify=False)
    elapsed_ms = (time.perf_counter() - start) * 1000

    resp.raise_for_status()
    assert elapsed_ms <= E2E_API_MAX_RESPONSE_MS, f"Response time {elapsed_ms:.0f}ms > {E2E_API_MAX_RESPONSE_MS}ms"

