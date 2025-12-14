from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import admin_credentials, instructor_credentials
from pages.admin_page import AdminPage
from pages.auth_login_page import AuthLoginPage
from pages.base_page import BasePage


@pytest.mark.admin
class TestRoleBasedAccess(BaseTest):
    def test_anonymous_cannot_access_admin_users(self):
        admin = AdminPage(self.driver, self.wait, E2E_BASE_URL)
        admin.open_users()

        # UsersManagement.razor redirects non-admins to "/"
        home = BasePage(self.driver, self.wait, E2E_BASE_URL)
        home.visible((By.CSS_SELECTOR, "div.hero"))

    def test_instructor_cannot_access_admin_users(self):
        creds = instructor_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        admin = AdminPage(self.driver, self.wait, E2E_BASE_URL)
        admin.open_users()

        home = BasePage(self.driver, self.wait, E2E_BASE_URL)
        home.visible((By.CSS_SELECTOR, "div.hero"))

    def test_admin_can_access_admin_pages(self):
        creds = admin_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        admin = AdminPage(self.driver, self.wait, E2E_BASE_URL)
        admin.open_users()
        assert "Gestion des utilisateurs" in admin.header_text()

        admin.open_categories()
        assert "Gestion des cat" in admin.header_text()

        admin.open_courses()
        assert "Gestion des cours" in admin.header_text()
