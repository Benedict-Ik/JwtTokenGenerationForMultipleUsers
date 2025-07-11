# Jwt Token Generation For Multiple Users

## Overview
Exposes authentication-related API endpoints, specifically for registering users and logging them in via JWTs.

## Endpoints
- **POST /api/users/register:** Registers a new user with a provided username and password.
- **POST /api/users/login:** Authenticates an existing user and returns a signed JWT on success.

## Dependencies
1. **Uses IUserService:**
- Handles core user operations like creating users and validating credentials.
- Encapsulates database access and password hashing internally.

  
2. **Uses JwtService:**
- Generates JWT tokens that contain user identity claims.
- Provides a secure mechanism for authenticated users to access protected APIs.

  
3. **Request Models:**
- RegisterRequest: Carries username and password for account creation.
- LoginRequest: Carries username and password for authentication.