from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import instructor_credentials
from pages.auth_login_page import AuthLoginPage
from pages.base_page import BasePage
from pages.nav_bar import NavBar


@pytest.mark.auth
class TestAuthPositiveNegative(BaseTest):
    def test_invalid_login_shows_error_message(self):
        login = AuthLoginPage(self.driver, self.wait, E2E_BASE_URL)
        login.login(email="invalid@example.com", password="wrong-password")

        message = login.read_error()
        assert "Identifiants invalides" in message

    def test_login_empty_fields_shows_validation(self):
        login = AuthLoginPage(self.driver, self.wait, E2E_BASE_URL)
        login.submit_empty()
        assert login.has_validation_errors(), "Expected validation messages to appear."

    def test_valid_login_and_logout(self):
        creds = instructor_credentials()

        login = AuthLoginPage(self.driver, self.wait, E2E_BASE_URL)
        login.login(email=creds.email, password=creds.password)

        page = BasePage(self.driver, self.wait, E2E_BASE_URL)
        page.visible((By.CSS_SELECTOR, ".user-menu"))

        nav = NavBar(self.driver, self.wait, E2E_BASE_URL)
        nav.logout()

