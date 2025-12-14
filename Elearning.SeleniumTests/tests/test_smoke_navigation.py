from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from pages.base_page import BasePage
from pages.nav_bar import NavBar


@pytest.mark.smoke
class TestSmokeNavigation(BaseTest):
    def test_home_page_loads(self):
        page = BasePage(self.driver, self.wait, E2E_BASE_URL)
        page.open("/")
        page.visible((By.CSS_SELECTOR, "div.hero"))  # Home page main section

    def test_nav_to_courses_page(self):
        page = BasePage(self.driver, self.wait, E2E_BASE_URL)
        page.open("/")

        nav = NavBar(self.driver, self.wait, E2E_BASE_URL)
        nav.click_nav("/courses")

        page.visible((By.CSS_SELECTOR, ".page-header h2"))

