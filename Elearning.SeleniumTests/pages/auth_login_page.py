from __future__ import annotations

from selenium.webdriver.common.by import By
from selenium.common.exceptions import TimeoutException

from config.settings import E2E_BASE_URL
from pages.base_page import BasePage


class AuthLoginPage(BasePage):
    path = "/login"

    # IMPORTANT:
    # Error alert is rendered as <div class="alert">...</div> inside .auth-card
    error_alert = (By.CSS_SELECTOR, ".auth-card .alert")

    # Blazor validation messages typically use the "validation-message" class.
    validation_messages = (By.CSS_SELECTOR, ".auth-card .validation-message")
    user_menu = (By.CSS_SELECTOR, ".user-menu")

    def __init__(self, driver, wait, base_url: str = E2E_BASE_URL):
        super().__init__(driver, wait, base_url)

    def open_page(self) -> None:
        self.open(self.path)

    def login(self, email: str, password: str, *, open: bool = True) -> None:
        if open:
            self.open_page()

        # Uses label-based lookup (more robust than nth-child selectors).
        self.find_input_in_form_group_by_label("Email").send_keys(email)
        self.find_input_in_form_group_by_label("Mot de passe").send_keys(password)

        self.click_button_by_text("Se connecter")

    def login_success(self, email: str, password: str, *, open: bool = True) -> None:
        """
        Login and wait until the app shows the authenticated header menu.
        """
        self.login(email, password, open=open)
        self.visible(self.user_menu)

    def submit_empty(self) -> None:
        self.open_page()
        self.click_button_by_text("Se connecter")

    def read_error(self) -> str:
        return self.visible(self.error_alert).text.strip()

    def has_validation_errors(self) -> bool:
        try:
            self.wait.until(lambda _d: len(self.driver.find_elements(*self.validation_messages)) > 0)
            return True
        except TimeoutException:
            return False
