from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class LessonPage(BasePage):
    title_h2 = (By.CSS_SELECTOR, ".page-header h2")
    not_found_alert = (By.CSS_SELECTOR, ".card .alert, .alert")

    next_button = (By.XPATH, "//button[normalize-space()='Next']")
    previous_button = (By.XPATH, "//button[normalize-space()='Previous']")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_by_id(self, lesson_id: int) -> None:
        self.open(f"/lessons/{lesson_id}")

    def is_loaded(self) -> bool:
        return len(self.driver.find_elements(*self.title_h2)) > 0

    def click_next(self) -> None:
        self.clickable(self.next_button).click()
        # Wait for navigation (URL change)
        self.wait.until(lambda _d: "/lessons/" in self.driver.current_url)

    def click_previous(self) -> None:
        self.clickable(self.previous_button).click()
        self.wait.until(lambda _d: "/lessons/" in self.driver.current_url)

    def wait_not_found(self) -> str:
        return self.visible(self.not_found_alert).text.strip()

    def open_outline_item_by_text(self, text: str) -> None:
        """
        In LessonView.razor, the course outline is rendered as buttons.
        We locate an outline button by visible text (e.g. "E2E Lesson 2") and click it.
        """
        locator = self.by_xpath(f"//button[contains(@class,'btn') and contains(normalize-space(),'{text}')]")
        self.clickable(locator).click()
        self.wait.until(lambda _d: "/lessons/" in self.driver.current_url)
