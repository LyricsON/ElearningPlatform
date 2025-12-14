from __future__ import annotations

import pytest
from selenium.webdriver.support import expected_conditions as EC

from config.settings import E2E_BASE_URL
from core.base_test import BaseTest
from core.test_data import ensure_minimum_course_content, user_credentials
from pages.auth_login_page import AuthLoginPage
from pages.course_detail_page import CourseDetailPage
from pages.courses_list_page import CoursesListPage


@pytest.mark.courses
class TestCoursesBrowseEnroll(BaseTest):
    @pytest.fixture(scope="class")
    def seeded(self):
        return ensure_minimum_course_content()

    def test_courses_list_loads(self):
        courses = CoursesListPage(self.driver, self.wait, E2E_BASE_URL)
        courses.open_page()

    def test_course_detail_and_enroll_redirects_when_anonymous(self, seeded):
        course_id = seeded["course_id"]

        detail = CourseDetailPage(self.driver, self.wait, E2E_BASE_URL)
        detail.open_by_id(course_id)
        detail.enroll()

        assert "/login" in self.driver.current_url
        assert f"returnUrl=/courses/{course_id}" in self.driver.current_url

    def test_course_detail_enroll_when_logged_in(self, seeded):
        course_id = seeded["course_id"]
        creds = user_credentials()

        # Login first (stable), then enroll from the course page.
        AuthLoginPage(self.driver, self.wait, E2E_BASE_URL).login_success(creds.email, creds.password)

        detail = CourseDetailPage(self.driver, self.wait, E2E_BASE_URL)
        detail.open_by_id(course_id)

        # Enroll now as authenticated user
        detail.enroll()
        self.wait.until(EC.presence_of_element_located(detail.continue_button))
