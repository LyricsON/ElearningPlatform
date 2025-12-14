from __future__ import annotations

import re
from datetime import datetime, timezone
from pathlib import Path

import pytest

from config.settings import E2E_ARTIFACTS_DIR, E2E_SCREENSHOT_ON_FAIL


def _safe_filename(name: str) -> str:
    name = re.sub(r"[^a-zA-Z0-9_.-]+", "_", name)
    return name.strip("._") or "test"


@pytest.hookimpl(hookwrapper=True)
def pytest_runtest_makereport(item: pytest.Item, call: pytest.CallInfo):
    outcome = yield
    report = outcome.get_result()

    if not E2E_SCREENSHOT_ON_FAIL:
        return

    if report.when != "call" or not report.failed:
        return

    driver = getattr(item, "_driver", None)
    if driver is None:
        return

    artifacts_dir = Path(E2E_ARTIFACTS_DIR) / "screenshots"
    artifacts_dir.mkdir(parents=True, exist_ok=True)

    timestamp = datetime.now(timezone.utc).strftime("%Y%m%d_%H%M%S")
    test_name = _safe_filename(report.nodeid)

    png_path = artifacts_dir / f"{timestamp}_{test_name}.png"
    html_path = artifacts_dir / f"{timestamp}_{test_name}.html"

    try:
        driver.save_screenshot(str(png_path))
    except Exception:
        pass

    try:
        html_path.write_text(driver.page_source, encoding="utf-8")
    except Exception:
        pass
