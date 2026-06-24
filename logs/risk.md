# Project Review Suggestions - 2026-06-24 10:46:49

## Scope
This log captures prioritized improvement suggestions for code quality, performance, and security.

## Suggestions

### 1. High - Startup crash risk from duplicate IDs in JSON
- File reference: `Repositories/JsonTodoRepository.cs` (`ToDictionary(todo => todo.Id)`).
- Risk: duplicated `id` values can throw during startup and prevent the API from running.
- Recommendation: validate duplicates before indexing; return a controlled, descriptive startup error.

### 2. High - Repository exposes mutable internal state
- File reference: `Repositories/JsonTodoRepository.cs` (`GetAll`, `GetByUserId`).
- Risk: callers may mutate internal collections via cast paths, causing data consistency issues.
- Recommendation: return immutable snapshots (e.g., immutable collections) or defensive copies.

### 3. Medium - No explicit authentication strategy
- File reference: `Program.cs` and `Controllers/TodosController.cs`.
- Risk: all endpoints are publicly readable; possible information exposure depending on business intent.
- Recommendation: define policy.
  - Public API: add throttling and response limits.
  - Private API: add authentication + authorization policies.

### 4. Medium - AllowedHosts is too open
- File reference: `appsettings.json` (`"AllowedHosts": "*"`).
- Risk: relaxed host filtering in production scenarios.
- Recommendation: restrict allowed hosts by environment and deployment domain.

### 5. Medium - No pagination/rate limiting on bulk endpoint
- File reference: `Controllers/TodosController.cs` (`GET /api/todos`).
- Risk: unrestricted full-dataset responses can degrade performance under load.
- Recommendation: add pagination (`page`, `pageSize`, max cap), rate limiting, and response compression.

### 6. Medium - Per-request allocations on completed queries
- File reference: `Repositories/JsonTodoRepository.cs` (`Where(...).ToArray()`).
- Risk: extra allocations and GC pressure at higher request volumes.
- Recommendation: precompute/cached completed indexes by user during initialization.

### 7. Low - Data file copied on every build
- File reference: `StatusTasks.WebApi.csproj` (`CopyToOutputDirectory` = `Always`).
- Risk: unnecessary build I/O.
- Recommendation: use `PreserveNewest` unless forced copy is required.

### 8. Low - Error contract could be standardized
- File reference: `Middleware/GlobalExceptionMiddleware.cs`.
- Risk: custom format is valid but less interoperable with standard tools.
- Recommendation: adopt RFC7807 `ProblemDetails` while keeping `traceId`.

### 9. Quality gap - Missing integration tests for HTTP pipeline
- File reference: `StatusTasks.WebApi.Tests/*`.
- Risk: unit tests validate service/repository logic but not full request pipeline behavior.
- Recommendation: add integration tests with `WebApplicationFactory` for status codes, contracts, and middleware.

## Suggested Priority Plan
1. Protect startup from duplicate IDs and make repository returns immutable.
2. Add pagination and rate limiting on `/api/todos`.
3. Decide public/private API policy and apply auth strategy.
4. Standardize errors with `ProblemDetails`.
5. Expand test suite with integration tests.
