from __future__ import annotations

from urllib.parse import urlparse

from selenium.webdriver.common.by import By

from core import wait_utils


class BasePage:
    """
    Very small base page object with helpers.
    Prefer stable selectors (data-testid). If the app doesn't have them yet,
    we use robust CSS/XPath and keep them in ONE place here.
    """

    def __init__(self, driver, wait, base_url: str):
        self.driver = driver
        self.wait = wait
        self.base_url = base_url.rstrip("/")

    def open(self, path: str) -> None:
        self.driver.get(f"{self.base_url}{path}")
        wait_utils.document_ready(self.driver, self.wait)

    def current_path(self) -> str:
        return urlparse(self.driver.current_url).path

    def by_css(self, selector: str):
        return (By.CSS_SELECTOR, selector)

    def by_xpath(self, xpath: str):
        return (By.XPATH, xpath)

    def visible(self, locator):
        return wait_utils.visible(self.wait, locator)

    def clickable(self, locator):
        return wait_utils.clickable(self.wait, locator)

    def present(self, locator):
        return wait_utils.present(self.wait, locator)

    def find_input_in_form_group_by_label(self, label_text: str):
        """
        Blazor forms often look like:
          <div class="form-group">
            <label>Email</label>
            <input ... />
          </div>

        This finds the input/textarea/select within the same form-group as the label.
        Update this helper once if the UI changes.
        """
        xpath = (
            "//div[contains(@class,'form-group')"
            f" and .//label[normalize-space()='{label_text}']]"
            "//input | "
            "//div[contains(@class,'form-group')"
            f" and .//label[normalize-space()='{label_text}']]"
            "//textarea | "
            "//div[contains(@class,'form-group')"
            f" and .//label[normalize-space()='{label_text}']]"
            "//select"
        )
        return self.visible(self.by_xpath(xpath))

    def click_button_by_text(self, text: str):
        locator = self.by_xpath(f"//button[normalize-space()='{text}']")
        self.clickable(locator).click()

