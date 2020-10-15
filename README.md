# Payment Gateway Challenge

Welcome to my solution to the .NET payment gateway challenge. The solution is split into 4 projects.

1. The PaymentGateway project is the API layer.
2. The PaymentGateway.Domain project is the domain layer, containing the core business logic and domain objects/interfaces.
3. The PaymentGateway.Data project is the data layer containing the Entity Framework related classes and repositories for data access.
4. The PaymentGateway.Tests project is the unit testing project for the solution. I have aimed to achieve > 80% unit test coverage for the core business logic contained in the PaymentGateway.Domain project, as well as the validators and controllers in the PaymentGateway project. I have not written unit tests for the  PaymentGateway.Data project.

## Features

- Fully functional Payment Gateway built for Development environment.
- Mock acquiring bank implementation.
- ASP .NET Core Identity integration with two seeded test accounts.
- JSON Web Token authentication.
- Entity Framework Core for data storage (Sqlite provider for development).
- Containerisation with Docker and Docker Compose.
- Application logging with Serilog.
- PCI Compliant card number masking when retrieving payment details.
- Model validation using Fluent Validation.
- Swagger / OpenAPI documentation.
- GitHub Actions setup for Continuous Integration.

## Assumptions

The following assumptions have been made whilst developing the Payment Gateway application.

- The application has been setup to only run in Development.
- Card numbers can be represented as a string.
- Money can be represented as a decimal.
- Currency is represented by an enum (only GBP = 0, USD = 1 and EUR = 2 are included).
- All payments are processed by the same acquiring bank.
- Users can only access details for their own payments.
- It acceptable to store full card details in the database.
- Password validation is sufficient for logging in (ignoring email verification/multi factor authentication).
- It is acceptable for multiple users to use the same card details.
- All DateTimes are expressed in UTC.

#### Mocking the acquiring bank

- I have assumed it is sufficient to mock the acquiring bank from within the PaymentGateway.Domain project and not as a separate API. I have created a `FakeAcquiringBank` class for this purpose.
- The `FakeAcquiringBank` implementation will fail a payment if the amount is greater than 500 in any currency (for testing). 
- The Acquiring Bank is responsible for forming its own `IAcquiringBankRequest`.
- The `ProcessPaymentAsync` method on `IAcquiringBank` can throw any exception and all of them will be handled in the same way. 

## Running the application

The application has three core pieces of functionality:

1. Requesting a JSON web token from the API
2. Processing a payment using the API.
3. Retrieving a payment's details using the API.

In order to run the application locally you will either require the .NET Core 3.1 SDK or Docker installed on your machine. The application is currently only setup to run using HTTP (not HTTPS) and in the Development environment.

To run the application using the dotnet CLI run `dotnet run --environment=Development` from the PaymentGateway/PaymentGateway directory. The application should now be running on **http://localhost:5000**. You should also have a Sqlite database called **payment-gateway.db** in the PaymentGateway/PaymentGateway directory which will store our data. EF Core migrations are run automatically on application startup in Development.

To run the application using Docker Compose you can run `docker-compose up` from the root of the repository. The application should now be running on **http://localhost:5000**.

There are two test users with usernames **Test** and **Test2**. They both have the same password **Password123!**. They can be used to test the functionality of the API.

Once the application is up and running, the Swagger documentation can be found at **http://localhost:5000/swagger**. You can use these pages to test your requests.

#### Obtaining a JSON Web Token for authentication

In order to generate a JSON web token for either test user, use the `POST /api/account/login` endpoint with the credentials for either account:

```
    {
        "username": "test",
        "password": "Password123!"
    }
```

If successful, you should receive the following response with a different `token` and `expires` value:

```
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
        "expires": "2020-10-16T23:02:59.8671789"
    }
```

The `token` property can be used to authenticate requests to other endpoints, and the `expires` property indicates when this token will no longer be valid.

**Note that the following two requests will require the following HTTP Header** 
`Authorization: Bearer {YOUR TOKEN}`.

#### Processing a payment

In order to process a payment with the API, use the `POST /api/payments` endpoint with the following example request body. Note that all request properties must be supplied and additional validations (that the card number is valid for example) are applied on each property using Fluent Validation.
   
```
{
    "cardNumber": "1111222233334444",
    "cvv": 123,
    "currency": 1,
    "amount": 12.00,
    "cardExpiryYear": 2021,
    "cardExpiryMonth": 12
}
```

If successful, you should receive the following response with a different `paymentId`:

```
{
    "success": true,
    "paymentId": "3b95476d-af21-4f2b-a6c5-45c9ed3ee530"
}
```

The `success` property indicates whether the payment was successful and the `paymentId` can be used with the `GET /api/payments/{paymentId}` endpoint to retrieve the details of the payment. Note that if you use an amount greater than 500, the acquiring bank will fail the payment and `success` will be `false`.

#### Retrieving a payment's details

 In order to retrieve details of a previous payment use the `GET /api/payments/{paymentId}` endpoint, supplying the `paymentId` returned in the previous step. For example `GET /api/payments/3b95476d-af21-4f2b-a6c5-45c9ed3ee530`. If authenticated as the same user who created the payment you should get a response containing the details of the payment like below:

```
{
    "paymentId": "3b95476d-af21-4f2b-a6c5-45c9ed3ee530",
    "amount": 12.0,
    "currency": 1,
    "paymentCreatedAt": "2020-10-15T20:02:59.8671789",
    "success": true,
    "cardDetails": {
        "maskedCardNumber": "111122XXXXXX4444",
        "cvv": 123,
        "expiryMonth": 12,
        "expiryYear": 2021
    }
}
```

If you request details for a payment that was not created by the user you are authenticated as you will get a 403 Forbidden response and if the payment doesn't exist you will get a 404 Not Found response.

## Future development / Areas for improvement

- Setup the use of HTTPS to enable secure communications for users supplying card details.
- Extend the current ASP .NET Identity integration, i.e. to allow for user registration, modification etc.
- If used in Production, the application would have to be PCI DSS compliant and would need to have a greater focus on the security of card/payment details.
- Use a different data provider with Entity Framework, rather than Sqlite. It has some restrictions on performing migrations with foreign key constraints which caused me some issues. Using a SQL Server instance running in a separate docker container would be a better solution for the Development environment.
- Normalize my database structure to have separate tables for *Payments* and *Cards*, with a table to link *Cards* to *Users*.
- Use a library like AutoMapper for mapping classes rather than doing it manually.
- Improve unit test coverage for the PaymentGateway project to cover Startup.cs, Program.cs and the Extensions directory.
- Add unit tests for PaymentGateway.Data project (Need to learn how to unit test when using EF Core).
- Add an integration test project.
