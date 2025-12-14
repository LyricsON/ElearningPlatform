from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class CoursesListPage(BasePage):
    path = "/courses"

    title_h2 = (By.CSS_SELECTOR, ".page-header h2")
    search_input = (By.CSS_SELECTOR, "input.input-control[placeholder^='Rechercher un cours']")
    course_cards = (By.CSS_SELECTOR, ".course-grid .course-card")
    first_view_button = (By.CSS_SELECTOR, ".course-grid .course-card .card-actions button.btn.btn-primary")
    empty_message = (By.CSS_SELECTOR, ".card p.muted")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_page(self) -> None:
        self.open(self.path)
        self.visible(self.title_h2)

    def search(self, text: str) -> None:
        el = self.visible(self.search_input)
        el.clear()
        el.send_keys(text)

    def has_any_courses(self) -> bool:
        return len(self.driver.find_elements(*self.course_cards)) > 0

    def open_first_course(self) -> None:
        self.clickable(self.first_view_button).click()
        self.wait.until(EC.url_contains("/courses/"))

