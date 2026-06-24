# Agent Instructions: Web API Development (To-Do Service)

## Role & Objective
You are an expert backend developer. Your task is to create a robust, production-ready RESTful Web API project that integrates a provided JSON file as its initial data source and exposes specific endpoints for managing To-Do items.
---

## Task Requirements

### 1. Data Source Configuration
* The API must read its data from the file named `todos.json`, which is located inside the `Resources` folder at the root of the project (`/Resources/todos.json`).
* Ensure that when the Web API application starts, this file is loaded or targeted as the primary data store (mock database) for all related requests.

### 2. API Endpoints to Implement/Update
Implement or update the following RESTful endpoints to interact with the data from `todos.json`. Ensure they follow standard HTTP methods and URL conventions:

* **GET `/api/todos`**
    * Retrieve all To-Do items.
* **GET `/api/users/{userId}/todos`**
    * Retrieve all To-Do items belonging to a specific `userId`.
* **GET `/api/users/{userId}/todos/completed`**
    * Retrieve only the *completed* To-Do items for a specific `userId`.
* **GET `/api/todos/{id}`**
    * Retrieve a single, specific To-Do item by its unique identifier.

### 3. Code Documentation
* **Inline Documentation:** Generate comprehensive code documentation for all newly created or updated service methods, controllers, and repository operations.
* **Standards:** Use the standard documentation format appropriate for the project's language (e.g., XML documentation comments `///` for C#, JSDoc for JavaScript/TypeScript, or Docstrings for Python).
* **Metadata:** The documentation must clearly describe the purpose of the operation, its parameters (including types and validations), return values, and potential exceptions or HTTP status codes thrown.
---

## Technical Constraints & Best Practices

### 🛡️ Validations & Error Handling
* **Input Validation:** Validate route parameters (e.g., ensure `userId` and `id` are of the correct data type and format before processing).
* **Resource Availability:** Return a `404 Not Found` with a clean error message if a requested `userId` or specific To-Do `id` does not exist.
* **Global Error Handling:** Implement a global exception handling middleware or interceptor to catch unhandled errors, returning a standard JSON error response (e.g., `500 Internal Server Error`) without leaking stack traces.

### ⚡ Performance & Data Management
* **Efficient I/O:** Read and parse the JSON file asynchronously to avoid blocking the main thread.
* **In-Memory Caching/State:** Load the JSON data into an optimized in-memory data structure (or a lightweight repository pattern) at startup to avoid redundant disk I/O on subsequent requests.
* **Query Optimization:** Use efficient filtering methods (e.g., LINQ, array methods, or indexing counterparts) to look up items by `userId` or `id`.

### 📐 Architecture & Clean Code
* **Separation of Concerns:** Separate controllers/routers, business logic (services), and data access layers.
* **REST Standards:** Use proper HTTP status codes (`200 OK`, `400 Bad Request`, `404 Not Found`, `500 Internal Server Error`).
* **Type Safety:** Define strong DTOs (Data Transfer Objects) or Interfaces representing the To-Do item schema based on the JSON structure.


### 4. Add Technical Documentation Context
  - Check if is necessary to generate new requests, controllers, components based on extensions.md recommendations and questions. .
- Reference current solution architecture patterns from the codebase
- Apply zero-warning build policy and scoped guidelines from CLAUDE.md

### 5. Generate Implementation Artifacts
- Create new code and patterns
- Produce C# code templates for application services, entities, and controllers based on the provided JSON structure and requirements
- Follow established patterns from similar components in the workspace
- Ensure all generated code compiles with zero warnings

### 6. Save Analysis Results (MANDATORY - Each Run Creates New File)
- **ALWAYS CREATE** a NEW timestamped markdown file in `/logs/` directory
- **Format:** `generation-{YYYYMMDD-HHmmss}.md` (with current date/time)
- **Each analysis run must generate a unique file** even if analyzing the same ticket multiple times
- Example: `generation-H20-64-20260508-143045.md` (different timestamp each time)
- Structure with clear sections for review and implementation
- **Note:** This allows tracking analysis changes over time for the same ticket

## Quality Validations

### Script Execution Validation
- Check for character encoding issues (no ???, âœ, or similar artifacts)
- Validate param() block is at script beginning if modifying scripts
- Retry with adjusted ticket number format if initial attempt fails

### Code Generation Standards
- Apply zero-warning policy: all generated code must compile cleanly

### Large Output Strategy
When approaching response limits:
1. **Create Overview First**: Generate summary of all items to be implemented
2. **Prioritize by Impact**: Focus on high-priority specified behavior items first
3. **Generate in Phases**: Create foundational templates before detailed implementations
4. **Reference Existing Patterns**: Link to similar components rather than recreating code
5. **Use File Links**: Reference existing files with line numbers instead of duplicating code

## TODO Handling
- When encountering TODO comments in analysis, attempt implementation using workspace patterns
- If unable to implement, document specific references to required actions
- Link to similar implementations in the codebase for guidance
- Mark unresolved TODOs with clear next-step instructions


Follow this workflow systematically to ensure consistent, high-quality analysis and template generation for all Jira ticket implementations.
