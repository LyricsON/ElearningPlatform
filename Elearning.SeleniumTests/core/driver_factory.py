from __future__ import annotations

import os

from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from webdriver_manager.chrome import ChromeDriverManager

from config.settings import (
    E2E_BROWSER,
    E2E_HEADLESS,
    E2E_PAGE_LOAD_TIMEOUT_SECONDS,
)


def create_driver() -> webdriver.Chrome:
    """
    Create and return a Selenium WebDriver instance.

    - Chrome only (simple + beginner-friendly).
    - No implicit waits (we use explicit waits only).
    """
    if E2E_BROWSER.strip().lower() != "chrome":
        raise ValueError(f"Unsupported browser: {E2E_BROWSER!r}. Use E2E_BROWSER=chrome.")

    options = webdriver.ChromeOptions()

    if E2E_HEADLESS:
        # "new" headless mode is the current recommended Chrome option.
        options.add_argument("--headless=new")

    options.add_argument("--window-size=1920,1080")
    options.add_argument("--disable-notifications")
    options.add_argument("--disable-infobars")
    options.add_argument("--disable-dev-shm-usage")

    # Helpful for local https dev certificates (common in ASP.NET/Blazor).
    options.add_argument("--ignore-certificate-errors")
    options.add_argument("--allow-insecure-localhost")

    # webdriver-manager downloads a matching chromedriver automatically.
    # In some environments, network can be flaky; we try online first and then fallback to cached driver only.
    os.environ.setdefault("WDM_TIMEOUT", "20")
    driver_path = None
    try:
        driver_path = ChromeDriverManager().install()
    except Exception:
        os.environ["WDM_LOCAL"] = "1"  # use cached driver only (no network)
        driver_path = ChromeDriverManager().install()

    service = Service(driver_path)
    driver = webdriver.Chrome(service=service, options=options)

    # Explicit waits only: keep implicit wait at 0.
    driver.implicitly_wait(0)
    driver.set_page_load_timeout(E2E_PAGE_LOAD_TIMEOUT_SECONDS)

    return driver
