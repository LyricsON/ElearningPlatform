from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from pages.base_page import BasePage


@pytest.mark.smoke
class TestErrorHandling(BaseTest):
    def test_unknown_route_shows_not_found(self):
        page = BasePage(self.driver, self.wait, E2E_BASE_URL)
        page.open("/this-route-should-not-exist")
        page.visible((By.CSS_SELECTOR, "p[role='alert']"))

    def test_my_courses_requires_login(self):
        page = BasePage(self.driver, self.wait, E2E_BASE_URL)
        page.open("/my-courses")
        page.visible((By.CSS_SELECTOR, "a.btn.btn-primary[href^='/login?returnUrl=/my-courses']"))

