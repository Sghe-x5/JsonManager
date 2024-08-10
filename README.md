# JSON Object Manager

## Overview
This project is a C# console application designed to manage JSON data by modeling objects, handling events, and performing various data operations. The application implements core OOP principles, ensuring robust and maintainable code.

## Features
- **Class Library**: Contains classes that represent JSON objects. Each class includes methods for JSON serialization (`ToJSON`), event handling (`Updated`), and encapsulation to maintain data integrity.
- **Event Handling**: Implements an `AutoSaver` class that listens for updates and automatically saves changes to a temporary JSON file if two events occur within 15 seconds.
- **Sorting and Editing**: Users can sort object collections by specified fields and edit object data through a user-friendly console interface.
- **Menu-driven Interface**: The application provides a clear, interactive menu for file operations, data sorting, and object editing.

## Technologies
- **C#** (.NET 6.0)
- **System.Text.Json** for JSON serialization/deserialization

## Usage
1. **Load JSON File**: Provide a file path to load and manage JSON data.
2. **Sort Data**: Choose fields to sort the object collection.
3. **Edit Objects**: Modify fields of objects, with validation for correct input.
4. **Auto-Save**: Automatically saves updates to a temporary file based on event triggers.

## Design Principles
- Adheres to SOLID principles, ensuring maintainability.
- Uses encapsulation to prevent unauthorized access or modification of object data.
- Implements Dependency Inversion to allow flexible and scalable design.

## Requirements
- .NET 6.0 or higher
- No external dependencies or NuGet packages allowed.
