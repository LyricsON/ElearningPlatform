# Requirements List (derived from code)

Derived by scanning:
- `Elearning.Blazor/Pages` routes (`@page`)
- `Elearning.Api/Controllers` endpoints + `[Authorize]` policies
- Seed users in `Elearning.Api/Data/SeedData.cs`

## UI routes (Blazor)

- `/` Home
- `/login` Login
- `/register` Register
- `/courses` Courses list (search/filter/sort)
- `/courses/{id:int}` Course details + enroll + open lessons + start quiz
- `/my-courses` My courses (requires login)
- `/lessons/{id:int}` Lesson view + outline + next/previous navigation
- `/quizzes` Quizzes list
- `/quizzes/{id:int}` Quiz take + submit (requires login)
- `/my-results` Quiz results (requires login)
- Instructor:
  - `/instructor/courses`
  - `/instructor/add-course`
  - `/instructor/edit-course/{CourseId:int}`
  - `/instructor/manage/{CourseId:int}`
- Admin:
  - `/admin`
  - `/admin/users`
  - `/admin/categories`
  - `/admin/courses`

## API features/endpoints (ASP.NET Core Web API)

Auth:
- REQ-001: User can login with email/password (`POST /api/auth/login`).
- REQ-002: User can register as Student or Instructor (`POST /api/auth/register`) with validation.

Catalog:
- REQ-003: Anyone can list categories (`GET /api/coursecategories`).
- REQ-004: Anyone can list courses and view course details (`GET /api/courses`, `GET /api/courses/{id}`).

Enrollment:
- REQ-005: Authenticated users can enroll in courses and track progress (`/api/enrollments`).

Lessons:
- REQ-006: Authenticated users can view lessons (`/api/lessons`).
- REQ-007: Instructors/Admin can create/update/delete lessons (course ownership enforced).

Quizzes:
- REQ-008: Authenticated users can view quizzes and questions (`/api/quizzes`, `/api/quizquestions`).
- REQ-009: Authenticated users can submit quiz answers and get a score (`POST /api/quizresults/submit`).
- REQ-010: Instructors/Admin can create/update/delete quizzes and questions (ownership enforced).

Administration:
- REQ-011: Admin can manage categories (`POST/PUT/DELETE /api/coursecategories`).
- REQ-012: Admin can manage users and roles (`/api/users`).
- REQ-013: Admin can delete enrollments and quiz results (`DELETE /api/enrollments/{id}`, `DELETE /api/quizresults/{id}`).

## Roles and access control

Derived from `[Authorize(Roles=...)]` in controllers and role checks in Blazor pages.

- REQ-014: Role-based access is enforced:
  - Admin-only endpoints (`/api/users`, category CRUD, delete enrollment/result)
  - Instructor/Admin content creation endpoints (courses/lessons/quizzes/questions)
  - Student restrictions on viewing other users' enrollments/results

