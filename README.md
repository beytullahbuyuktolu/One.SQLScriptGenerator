# SQL Script Generator

A powerful Windows Forms application for generating SQL scripts from database tables. This tool helps database administrators and developers create SQL scripts quickly and efficiently.

## Key Features

### Database Connection
- Connect to SQL Server databases
- Load and display available databases
- Support for multiple database selection

### Script Generation Types
1. **Create Table (Empty)**
   - Generates SQL scripts to create empty tables
   - Includes all table structure and constraints
   - Perfect for creating new tables with existing structure

2. **Create Table with Data**
   - Creates tables with data from source tables
   - Generates INSERT statements for all records
   - Maintains all table structure and relationships

3. **Create Table with Modified Company Code**
   - Special feature for company-specific data
   - Allows changing company codes in data
   - Useful for data migration between companies

### User Interface
- Clean and intuitive Windows Forms interface
- Multiple selection support for databases and tables
- Real-time connection status display
- Error handling and user feedback
- Configuration management through appsettings.json

## Installation

1. Clone the repository:
```bash
git clone [repository-url]
```

2. Open the solution in Visual Studio
3. Build the solution
4. Run the application

## Configuration

The application uses an appsettings.json file for configuration:
- Server connection details
- Database connection string
- Other application settings

## Usage Steps

1. Launch the application
2. Configure database connection in appsettings.json
3. Connect to the SQL Server
4. Select databases from the list
5. Choose the desired script type
6. Generate and review the SQL script
7. Copy or save the generated script

## Technical Details

- **Framework**: .NET Framework
- **Technologies Used**:
  - C# Windows Forms
  - SQL Server Integration
  - JSON Configuration Management
  - Async/Await Pattern for Database Operations

- **Core Components**:
  - DatabaseService: Handles database connections and operations
  - ScriptGeneratorService: Generates different types of SQL scripts
  - MainForm: Main user interface

## Requirements

- Windows operating system
- .NET Framework installed
- SQL Server instance
- Visual Studio (for development)

## Contributing

If you would like to contribute to the project:
1. Fork the repository
2. Add new features
3. Submit a pull request

## Contact

If you have any questions about the project, please open an issue on the GitHub Issues page.
