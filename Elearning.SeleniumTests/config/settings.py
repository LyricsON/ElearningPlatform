from __future__ import annotations

import os

from config.environments import ENVIRONMENTS


def _env_bool(name: str, default: bool) -> bool:
    raw = os.getenv(name)
    if raw is None:
        return default
    return raw.strip().lower() in {"1", "true", "yes", "y", "on"}


def _env_str(name: str, default: str) -> str:
    raw = os.getenv(name)
    return default if raw is None or not raw.strip() else raw.strip()


# Select environment (local by default).
E2E_ENV: str = _env_str("E2E_ENV", "local")
_env_defaults = ENVIRONMENTS.get(E2E_ENV, ENVIRONMENTS["local"])

# Base URL of the running Blazor app.
# Also supports legacy env var BASE_URL for convenience.
E2E_BASE_URL: str = _env_str("E2E_BASE_URL", _env_str("BASE_URL", _env_defaults["base_url"])).rstrip("/")

# API base URL (used for optional API checks / test data setup).
E2E_API_URL: str = _env_str("E2E_API_URL", _env_defaults["api_url"]).rstrip("/")

# Browser settings
E2E_BROWSER: str = _env_str("E2E_BROWSER", _env_str("BROWSER", "chrome"))
E2E_HEADLESS: bool = _env_bool("E2E_HEADLESS", _env_bool("HEADLESS", False))

# Timeouts
E2E_TIMEOUT_SECONDS: int = int(_env_str("E2E_TIMEOUT_SECONDS", _env_str("DEFAULT_TIMEOUT_SECONDS", "10")))
E2E_PAGE_LOAD_TIMEOUT_SECONDS: int = int(_env_str("E2E_PAGE_LOAD_TIMEOUT_SECONDS", _env_str("PAGE_LOAD_TIMEOUT_SECONDS", "30")))
E2E_API_MAX_RESPONSE_MS: int = int(_env_str("E2E_API_MAX_RESPONSE_MS", "5000"))

# Artifacts
E2E_ARTIFACTS_DIR: str = _env_str("E2E_ARTIFACTS_DIR", "artifacts")
E2E_SCREENSHOT_ON_FAIL: bool = _env_bool("E2E_SCREENSHOT_ON_FAIL", True)

# Credentials (if not provided, tests that require them will skip)
E2E_USER_EMAIL: str | None = os.getenv("E2E_USER_EMAIL")
E2E_USER_PASSWORD: str | None = os.getenv("E2E_USER_PASSWORD")
E2E_ADMIN_EMAIL: str | None = os.getenv("E2E_ADMIN_EMAIL")
E2E_ADMIN_PASSWORD: str | None = os.getenv("E2E_ADMIN_PASSWORD")
