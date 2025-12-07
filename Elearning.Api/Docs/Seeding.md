---
title: Seeding and Test Credentials
---

## Default test users

When running the API locally, the database is seeded with two users that simplify Swagger testing:

| Role       | Email                   | Password       |
|------------|-------------------------|----------------|
| Admin      | `admin@elearning.local` | `Admin123!`    |
| Instructor | `instructor@elearning.local` | `Instructor123!` |

Use `POST /api/auth/login` to retrieve a JWT, then click the `Authorize` button in Swagger and paste the token (without the `Bearer ` prefix). Once authorized, the seeded admin and instructor accounts let you exercise the protected CRUD endpoints.
