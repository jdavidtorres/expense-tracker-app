#!/bin/bash

# .NET MAUI Development Container Setup Script
echo "Setting up .NET MAUI development environment..."

# Install .NET MAUI workload
echo "Installing .NET MAUI workload..."
dotnet workload update
dotnet workload install maui

# Verify .NET installation
echo "Verifying .NET installation..."
dotnet --version
dotnet workload list

# Restore packages for the solution
echo "Restoring NuGet packages..."
if [ -f "ExpenseTracker.sln" ]; then
    dotnet restore ExpenseTracker.sln
else
    echo "Solution file not found, skipping package restore"
fi

echo "Development environment setup complete!"
echo "You can now build and run the .NET MAUI application."
echo ""
echo "Available commands:"
echo "  dotnet build                 - Build the entire solution"
echo "  dotnet run --project ExpenseTracker.Maui  - Run the MAUI app"
echo "  dotnet test                  - Run tests (when available)"