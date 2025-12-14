from __future__ import annotations

import pytest

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import user_credentials
from pages.auth_login_page import AuthLoginPage
from pages.profile_page import ProfilePage


@pytest.mark.profile
class TestProfileValidation(BaseTest):
    def test_profile_page_not_implemented_yet(self):
        """
        MainLayout has a Profile link to /profile, but there is no /profile page in the Blazor project.
        When you implement it, replace this test with real profile validations.
        """
        creds = user_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        profile = ProfilePage(self.driver, self.wait, E2E_BASE_URL)
        profile.open_page()

        if profile.is_not_found():
            pytest.skip("Profile page is not implemented in the UI yet (/profile route missing).")
