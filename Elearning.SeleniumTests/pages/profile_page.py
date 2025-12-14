from __future__ import annotations

from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class ProfilePage(BasePage):
    """
    MainLayout links to /profile, but there is currently no @page "/profile" found in Elearning.Blazor/Pages.
    This page object is kept for future implementation.
    """

    path = "/profile"
    not_found_alert = (By.CSS_SELECTOR, "p[role='alert']")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_page(self) -> None:
        self.open(self.path)

    def is_not_found(self) -> bool:
        return len(self.driver.find_elements(*self.not_found_alert)) > 0

