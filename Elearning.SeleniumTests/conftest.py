from __future__ import annotations

import pytest
from selenium.webdriver.support.ui import WebDriverWait

from config.settings import E2E_TIMEOUT_SECONDS
from core.driver_factory import create_driver

pytest_plugins = ["core.screenshot_on_fail"]


@pytest.fixture
def driver(request):
    """
    WebDriver fixture shared by all tests.
    Quits the browser after each test.
    """
    web_driver = create_driver()
    request.node._driver = web_driver
    yield web_driver
    web_driver.quit()


@pytest.fixture
def wait(driver):
    """
    Explicit wait helper fixture (explicit waits only; no time.sleep / implicit waits).
    """
    return WebDriverWait(driver, E2E_TIMEOUT_SECONDS)
