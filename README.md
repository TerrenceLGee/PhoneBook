# PhoneBook
A console based contact management application built with C# and .NET.

## Features
- **Contact Management**: Add, view, update, and delete contacts
- **Email Integration**: Send emails directly from the application
- **Data Persistence**: SQL Server database with Entity Framework Core
- **Logging**: Comprehensive logging with Serilog
- **Error Handling**: Robust error handling with custom Result pattern

## Technologies Used
- **Framework**: .NET 9.0
- **Database**: SQL Server with Entity Framework Core
- **Email**: MailKit for SMTP email sending
- **Phone Number**: libphonenumber-csharp for validation of entered phone numbers 
- **Logging**: Serilog with file and console output
- **Console Interface**: Spectre.Console
- **Error Handling**: Custom Result pattern for functional error handling

  ## Project Structure

```
PhoneBook/
├── PhoneBook.Core/           # Core domain models and results
├── PhoneBook.DataAccess/     # Data access layer with repositories
├── PhoneBook.Domain/         # Business logic and services
└── PhoneBook.Presentation/   # User interface layer/Application entry point
```

## Architecture

This application follows Clean Architecture principles:

- **Core Layer**: Contains domain models and business rules
- **Data Access Layer**: Handles database operations and repositories
- **Domain Layer**: Contains business logic and services
- **Presentation Layer**: Handles user interface and user interactions

### Key Design Patterns Used

- **Repository Pattern**: For data access abstraction
- **Dependency Injection**: For loose coupling and testability
- **Result Pattern**: For functional error handling
- **Factory Pattern**: For object creation
- **Extension Methods**: For code reusability

## Error Handling

The application uses a custom Result pattern for comprehensive error handling:

- All operations return `Result<T>` or `Result` objects
- Errors are logged using Serilog
- User-friendly error messages are displayed
- Detailed error information is logged for debugging

## Logging

Comprehensive logging is implemented using Serilog:

- **File Logging**: Daily rotating log files in the `Logs/` directory
- **Console Logging**: Real-time logging to console during development
- **Structured Logging**: Consistent log format with timestamps and levels
- **Exception Logging**: Detailed exception information for debugging

### Entity Framework Commands

Due to the structure of the application, Entity Framework commands must be run from the `PhoneBook.DataAccess` directory:

```
# Navigate to the DataAccess project
cd PhoneBook.DataAccess

# Create a new migration
dotnet ef migrations add MigrationName

# Update the database
dotnet ef database update

# Remove the last migration
dotnet ef migrations remove
```

This is necessary because the `PhoneBookContextFactory` is located in the DataAccess project, which EF Core uses to create the DbContext at design time.

## Installation

1. **Clone the repository**
   ```
   git clone https://github.com/yourusername/phonebook-application.git
   cd phonebook-application
   ```

2. **Restore dependencies**
   ```
   dotnet restore
   ```

3. **Configure the database**
   - Update the connection string in `appsettings.json`
   - Navigate to the DataAccess project directory and run database migrations:
   ```
   cd PhoneBook.DataAccess
   dotnet ef database update
   ```
   **Note**: Entity Framework commands must be run from the `PhoneBook.DataAccess` directory since this is where the `PhoneBookContextFactory` is located.

4. **Configure email settings**
   - Update the `EmailSettings` section in `appsettings.json`:
   ```json
   {
     "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": 587,
       "SenderEmail": "your-email@gmail.com",
       "SenderName": "Your Name",
       "AppPassword": "your-app-password"
     }
   }
   ```

5. **Run the application**
   ```
   dotnet run
   ```

## Configuration

### Database Configuration
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PhoneBookDb": "Server=(localdb)\\mssqllocaldb;Database=PhoneBookDb;Trusted_Connection=true;"
  }
}
```

### Email Configuration
For Gmail, you'll need to:
1. Enable 2-factor authentication
2. Generate an app password
3. Use the app password in the configuration

### Logging Configuration
Logs are automatically saved to the `Logs/` directory with daily rotation. The logging level can be adjusted in the `LoggingSetup()` method.

## Usage

The application provides a console-based menu system with the following options:

1. **Add Contact**: Create a new contact with name, email, and phone number
2. **Update Contact**: Modify existing contact information
3. **Delete Contact**: Remove a contact from the database
4. **View Contact By Id** View information for an individual contact by id
5. **View Contact By Name** View information for an individual contact by name
6. **View Contacts By Category** View contacts filtered by category (i.e. General, Family, Friends, Work)
7. **View All Contacts** View all contacts regardless of category
8. **Send Email**: Send an email to a contact
9. **Exit**: Close the application


## Troubleshooting

### Common Issues

**Database Connection Issues**
- Ensure SQL Server is running
- Verify the connection string is correct
- Check if the database exists
- **Important**: Run Entity Framework commands from the `PhoneBook.DataAccess` directory, not the main project directory

**Email Sending Issues**
- Verify SMTP settings are correct
- Ensure app password is generated for Gmail
- Check firewall settings

**Application Startup Issues**
- Check the logs in the `Logs/` directory
- Verify all dependencies are installed
- Ensure .NET 9.0 is installed
   
## Acknowledgments

- [The C# Academy](https://www.thecsharpacademy.com/)


## Contact

Terrence Gee - mrgee1978@proton.me

Project Link: [https://github.com/yourusername/phonebook-application](https://github.com/yourusername/phonebook-application)

