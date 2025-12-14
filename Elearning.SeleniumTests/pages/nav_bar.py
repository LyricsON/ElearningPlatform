from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC

from pages.base_page import BasePage


class NavBar(BasePage):
    """
    The top header + nav bar are in Shared/MainLayout.razor.
    """

    user_menu = (By.CSS_SELECTOR, ".user-menu")
    user_dropdown = (By.CSS_SELECTOR, ".user-dropdown")
    logout_button = (By.CSS_SELECTOR, "button.dropdown-logout")

    def click_nav(self, href: str) -> None:
        locator = (By.CSS_SELECTOR, f"nav.nav-bar a[href='{href}']")
        self.clickable(locator).click()
        self.wait.until(EC.url_contains(href))

    def click_header_link(self, href: str) -> None:
        locator = (By.CSS_SELECTOR, f"header.top-header a[href='{href}']")
        self.clickable(locator).click()
        self.wait.until(EC.url_contains(href))

    def open_user_menu(self) -> None:
        self.clickable(self.user_menu).click()
        self.visible(self.user_dropdown)

    def logout(self) -> None:
        self.open_user_menu()
        self.clickable(self.logout_button).click()
        self.wait.until(EC.url_contains("/login"))

