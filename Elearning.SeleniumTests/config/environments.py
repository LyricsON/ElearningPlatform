from __future__ import annotations

"""
Central place for environment URLs.

These defaults were discovered from your .NET launchSettings.json files:
- Blazor UI: https://localhost:7051
- API:       https://localhost:7075
"""

ENVIRONMENTS: dict[str, dict[str, str]] = {
    "local": {
        "base_url": "https://localhost:7051",
        "api_url": "https://localhost:7075",
    }
}

