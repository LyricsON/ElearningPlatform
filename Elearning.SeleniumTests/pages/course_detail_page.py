from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class CourseDetailPage(BasePage):
    course_hero = (By.CSS_SELECTOR, ".course-hero")
    # Button text is "S'inscrire maintenant" when user is NOT enrolled.
    # We match on "inscrire" to avoid apostrophe/encoding issues.
    enroll_button = (
        By.XPATH,
        "//button[contains(@class,'btn') and "
        "(contains(normalize-space(),'inscrire') or contains(normalize-space(),'Inscrire'))]",
    )
    continue_button = (By.XPATH, "//button[contains(@class,'btn') and contains(normalize-space(),'Continuer le cours')]")
    lesson_open_buttons = (By.CSS_SELECTOR, ".lesson-list button.btn.btn-outline")
    start_quiz_buttons = (By.XPATH, "//button[contains(@class,'btn') and contains(normalize-space(),'Commencer')]")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_by_id(self, course_id: int) -> None:
        self.open(f"/courses/{course_id}")
        self.visible(self.course_hero)

    def enroll(self) -> None:
        self.clickable(self.enroll_button).click()
        # Either stays on course page (logged in) or redirects to login (anonymous).
        self.wait.until(lambda _d: "/login" in self.driver.current_url or "/courses/" in self.driver.current_url)

    def continue_learning(self) -> None:
        self.clickable(self.continue_button).click()
        self.wait.until(EC.url_contains("/lessons/"))

    def open_first_lesson(self) -> None:
        buttons = self.driver.find_elements(*self.lesson_open_buttons)
        if not buttons:
            raise AssertionError("No lessons found in course detail page.")
        buttons[0].click()
        self.wait.until(EC.url_contains("/lessons/"))

    def start_first_quiz(self) -> None:
        self.clickable(self.start_quiz_buttons).click()
        self.wait.until(EC.url_contains("/quizzes/"))
