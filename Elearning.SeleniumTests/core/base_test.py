from __future__ import annotations

import pytest


class BaseTest:
    """
    Base class for tests.
    Tests can inherit from this and use:
      - self.driver
      - self.wait
    """

    driver = None
    wait = None

    @pytest.fixture(autouse=True)
    def _inject_fixtures(self, driver, wait):
        self.driver = driver
        self.wait = wait
