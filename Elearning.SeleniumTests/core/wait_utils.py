from __future__ import annotations

from selenium.webdriver.remote.webdriver import WebDriver
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait


def visible(wait: WebDriverWait, locator):
    return wait.until(EC.visibility_of_element_located(locator))


def present(wait: WebDriverWait, locator):
    return wait.until(EC.presence_of_element_located(locator))


def clickable(wait: WebDriverWait, locator):
    return wait.until(EC.element_to_be_clickable(locator))


def url_contains(wait: WebDriverWait, text: str):
    return wait.until(EC.url_contains(text))


def document_ready(driver: WebDriver, wait: WebDriverWait):
    """
    Wait until document.readyState == 'complete'.
    Useful when navigating between pages.
    """
    wait.until(lambda _d: driver.execute_script("return document.readyState") == "complete")

