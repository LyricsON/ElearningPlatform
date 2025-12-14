from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class QuizPage(BasePage):
    login_required_card = (By.CSS_SELECTOR, ".card a.btn.btn-primary[href^='/login']")
    submit_button = (By.XPATH, "//button[contains(@class,'btn') and contains(normalize-space(),'Soumettre')]")
    score_card = (By.XPATH, "//div[contains(@class,'card')][.//h5[contains(normalize-space(),'obtenu')]]")
    any_question = (By.CSS_SELECTOR, ".quiz-question")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_by_id(self, quiz_id: int) -> None:
        self.open(f"/quizzes/{quiz_id}")

    def wait_login_required(self) -> None:
        self.visible(self.login_required_card)

    def answer_first_question_a(self) -> None:
        # Select first radio input (option A) for the first question.
        locator = (By.CSS_SELECTOR, ".quiz-question input[type='radio'][value='A']")
        self.clickable(locator).click()

    def submit(self) -> None:
        self.clickable(self.submit_button).click()
        self.wait.until(lambda _d: len(self.driver.find_elements(*self.score_card)) > 0 or "/login" in self.driver.current_url)

    def has_questions(self) -> bool:
        return len(self.driver.find_elements(*self.any_question)) > 0

