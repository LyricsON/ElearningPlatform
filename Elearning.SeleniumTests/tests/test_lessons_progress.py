from __future__ import annotations

import pytest
from selenium.webdriver.common.by import By

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import ensure_minimum_course_content, user_credentials
from pages.auth_login_page import AuthLoginPage
from pages.lesson_page import LessonPage


@pytest.mark.lessons
class TestLessonsProgress(BaseTest):
    @pytest.fixture(scope="class")
    def seeded(self):
        return ensure_minimum_course_content()

    def test_open_lesson_and_navigate_next(self, seeded):
        creds = user_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        lesson = LessonPage(self.driver, self.wait, E2E_BASE_URL)
        lesson.open_by_id(seeded["lesson1_id"])
        self.wait.until(lambda _d: lesson.is_loaded())

        # More stable than relying on the Next button state:
        # click the outline item for "E2E Lesson 2" (created by test data setup).
        lesson.open_outline_item_by_text("E2E Lesson 2")
        self.wait.until(lambda _d: f"/lessons/{seeded['lesson2_id']}" in self.driver.current_url)

    def test_invalid_lesson_id_shows_not_found(self):
        creds = user_credentials()
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        lesson = LessonPage(self.driver, self.wait, E2E_BASE_URL)
        lesson.open_by_id(999999)

        message = lesson.wait_not_found()
        assert "Lesson not found" in message
