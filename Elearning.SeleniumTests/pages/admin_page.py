from __future__ import annotations

from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class AdminPage(BasePage):
    users_path = "/admin/users"
    categories_path = "/admin/categories"
    courses_path = "/admin/courses"

    header_h1 = (By.CSS_SELECTOR, ".page-header h1, .page-header h2")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_users(self) -> None:
        self.open(self.users_path)

    def open_categories(self) -> None:
        self.open(self.categories_path)

    def open_courses(self) -> None:
        self.open(self.courses_path)

    def header_text(self) -> str:
        return self.visible(self.header_h1).text.strip()

