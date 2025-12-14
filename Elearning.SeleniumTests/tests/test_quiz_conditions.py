from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import ensure_minimum_course_content, user_credentials
from pages.auth_login_page import AuthLoginPage
from pages.quiz_page import QuizPage


@pytest.mark.quiz
class TestQuizConditions(BaseTest):
    @pytest.fixture(scope="class")
    def seeded(self):
        return ensure_minimum_course_content()

    def test_quiz_requires_login(self, seeded):
        quiz = QuizPage(self.driver, self.wait, E2E_BASE_URL)
        quiz.open_by_id(seeded["quiz_id"])
        quiz.wait_login_required()

    def test_take_quiz_happy_path(self, seeded):
        creds = user_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        quiz = QuizPage(self.driver, self.wait, E2E_BASE_URL)
        quiz.open_by_id(seeded["quiz_id"])
        self.wait.until(lambda _d: quiz.has_questions())

        quiz.answer_first_question_a()
        quiz.submit()

        # Score card appears after successful submission
        assert len(self.driver.find_elements(By.XPATH, "//h5[contains(.,'obtenu')]")) > 0
