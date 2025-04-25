# DLS E-Commerce Web Application

A full-stack e-commerce web application built with ASP.NET Core Web API backend and Angular frontend.

## Overview

This application provides a simplified e-commerce platform with the following features:

- Product browsing and filtering by category
- Shopping cart functionality
- User authentication and authorization
- Admin panel for product and category management

## Technology Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core with SQL Server
- Clean architecture (Controllers, Services, Repositories)
- JWT authentication
- AutoMapper for DTO mapping

### Frontend

- Angular 19.2
- Angular Material UI components
- Angular Reactive Forms with validation
- NgRx for state management
- Angular Guards for route protection
- HTTP Interceptors for JWT token handling

## Project Structure

### Backend

The backend follows a layered architecture:

- **DLS.API**: API controllers and configuration
- **DLS.Application**: Business logic, services, and DTOs
- **DLS.Domain**: Domain entities and interfaces
- **DLS.Infrastructure**: Data access, repositories, and external services

### Frontend

The Angular frontend is organized by feature modules:

- **auth**: Login/Register components and authentication logic
- **product**: Product listing and details
- **cart**: Shopping cart functionality
- **admin**: Admin dashboard for product/category management
- **core**: Shared services, guards, and interceptors

## API Endpoints

### Authentication

- POST `/api/authentication/register` - Register a new user
- POST `/api/authentication/login` - Login and receive JWT token

### Products

- GET `/api/products` - Get all products
- GET `/api/products/{id}` - Get product by ID
- GET `/api/products/category/{categoryId}` - Filter products by category
- POST `/api/products` - Create new product (Admin only)
- PUT `/api/products/{id}` - Update product (Admin only)
- DELETE `/api/products/{id}` - Delete product (Admin only)

### Categories

- GET `/api/categories` - Get all categories
- GET `/api/categories/{id}` - Get category by ID
- POST `/api/categories` - Create new category (Admin only)
- PUT `/api/categories/{id}` - Update category (Admin only)
- DELETE `/api/categories/{id}` - Delete category (Admin only)

### Cart

- GET `/api/cart` - Get current user's cart
- POST `/api/cart/add` - Add item to cart
- PUT `/api/cart/update` - Update cart item quantity
- DELETE `/api/cart/remove/{id}` - Remove item from cart

## Setup Instructions

### Prerequisites

- .NET SDK 8.0 or later
- Node.js 18 or later
- SQL Server (or replace connection string for SQLite)

### Backend Setup

1. Navigate to the `BackEnd` directory
2. Update the connection string in `DLS.API/appsettings.json` if needed
3. Run the following commands:
   ```
   dotnet restore
   dotnet build
   dotnet run --project DLS.API
   ```
4. The API will be available at `https://localhost:7080` and `http://localhost:5173`

### Frontend Setup

1. Navigate to the `FrontEnd` directory
2. Run the following commands:
   ```
   npm install
   npm start
   ```
3. The application will be available at `http://localhost:4200`

## Features Implemented

- ✅ Clean architecture with separation of concerns
- ✅ Entity Framework Core with SQL Server
- ✅ JWT authentication with role-based authorization
- ✅ CRUD operations for products and categories
- ✅ Shopping cart functionality
- ✅ Data validation using FluentValidation
- ✅ Records and Mappester
- ✅ Angular routing with Auth Guards
- ✅ Reactive Forms with validation
- ✅ HTTP Interceptors for JWT token
