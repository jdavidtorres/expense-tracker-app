# CI/CD Pipeline Documentation

This document describes the Continuous Integration (CI) pipeline setup for the Expense Tracker .NET MAUI application.

## Pipeline Overview

The CI pipeline is designed to ensure code quality, security, and cross-platform compatibility without implementing Continuous Deployment (CD) since we don't have app stores configured yet.

## Workflows

### 1. Main CI Build (`ci.yml`)

**Triggers:**
- Pull requests to `main` or `develop` branches
- Direct pushes to `main` or `develop` branches

**Jobs:**

#### `build-shared`
- **Platform**: Ubuntu Latest
- **Purpose**: Build shared libraries and web project
- **Steps**:
  - Build `ExpenseTracker.Shared` project
  - Build `ExpenseTracker.Web` project
  - Run tests (if available)
  - Upload test results as artifacts

#### `build-android`
- **Platform**: Ubuntu Latest
- **Purpose**: Build Android MAUI application
- **Dependencies**: Requires `build-shared` to complete
- **Requirements**:
  - Java 17 (Microsoft distribution)
  - MAUI Android workloads
- **Target**: `net8.0-android`

#### `build-windows`
- **Platform**: Windows Latest
- **Purpose**: Build Windows MAUI application
- **Dependencies**: Requires `build-shared` to complete
- **Requirements**:
  - MAUI workloads
- **Target**: `net8.0-windows10.0.19041.0`

#### `build-macos`
- **Platform**: macOS Latest
- **Purpose**: Build iOS and macCatalyst MAUI applications
- **Dependencies**: Requires `build-shared` to complete
- **Requirements**:
  - MAUI workloads
- **Targets**: 
  - `net8.0-ios`
  - `net8.0-maccatalyst`

#### `code-quality`
- **Platform**: Ubuntu Latest
- **Purpose**: Code formatting and static analysis
- **Dependencies**: Requires `build-shared` to complete
- **Checks**:
  - Code formatting validation using `dotnet format`
  - Static analysis during build process

### 2. Security Scanning (`security.yml`)

**Triggers:**
- Pushes to `main` or `develop` branches
- Pull requests to `main` branch
- Scheduled daily at 2 AM UTC

**Jobs:**

#### `codeql`
- **Platform**: Ubuntu Latest
- **Purpose**: Security vulnerability scanning using GitHub CodeQL
- **Language**: C#
- **Queries**: Security and quality rules
- **Permissions**: 
  - Read actions and contents
  - Write security events

#### `dependency-review`
- **Platform**: Ubuntu Latest
- **Purpose**: Review dependencies in pull requests
- **Trigger**: Only on pull requests
- **Configuration**:
  - Fails on moderate+ severity vulnerabilities
  - Denies GPL-2.0 and GPL-3.0 licenses

### 3. Dependency Updates (`dependency-update.yml`)

**Triggers:**
- Scheduled weekly on Mondays at 8 AM UTC
- Manual workflow dispatch

**Jobs:**

#### `dependency-update`
- **Platform**: Ubuntu Latest
- **Purpose**: Check for outdated packages and create issues
- **Steps**:
  - Scan all projects for outdated packages
  - Generate JSON reports
  - Automatically create GitHub issues for outdated dependencies
- **Artifacts**: Dependency reports (30-day retention)

## Features

### ✅ Implemented Features

1. **Cross-Platform Build Validation**
   - Android (Linux runner)
   - Windows (Windows runner)
   - iOS/macCatalyst (macOS runner)

2. **Code Quality Assurance**
   - Automated code formatting checks
   - Static analysis integration
   - Build warnings monitoring

3. **Security Monitoring**
   - CodeQL security scanning
   - Dependency vulnerability review
   - License compliance checking

4. **Dependency Management**
   - Automated outdated package detection
   - Weekly dependency health reports
   - Automatic issue creation for updates

5. **Artifact Management**
   - Test result preservation
   - Dependency report archival
   - Build caching for faster execution

6. **Performance Optimization**
   - NuGet package caching
   - Parallel job execution
   - Incremental builds

### 🚫 Not Implemented (Future Considerations)

1. **Continuous Deployment (CD)**
   - App store publishing
   - Package distribution
   - Release automation

2. **Advanced Testing**
   - UI testing (no test projects exist yet)
   - Integration testing
   - Performance testing

3. **Advanced Security**
   - Secret scanning (using GitHub native features instead)
   - Container security scanning
   - Infrastructure as Code scanning

## Configuration

### Environment Variables

```yaml
env:
  DOTNET_VERSION: '8.0.x'          # .NET SDK version
  DOTNET_NOLOGO: true              # Disable .NET welcome message
  DOTNET_CLI_TELEMETRY_OPTOUT: true # Disable telemetry
```

### Required GitHub Permissions

- **Contents**: Read (for code checkout)
- **Actions**: Read (for workflow execution)
- **Security Events**: Write (for CodeQL results)
- **Issues**: Write (for dependency update issues)

### Dependencies

#### Required Tools
- .NET 8.0 SDK
- MAUI workloads (`maui`, `maui-android`)
- Java 17 (for Android builds)

#### GitHub Actions
- `actions/checkout@v4`
- `actions/setup-dotnet@v4`
- `actions/setup-java@v4`
- `actions/cache@v4`
- `actions/upload-artifact@v4`
- `github/codeql-action/init@v3`
- `github/codeql-action/analyze@v3`
- `actions/dependency-review-action@v4`
- `actions/github-script@v7`

## Monitoring and Maintenance

### Status Badges

Add these to your README.md to show build status:

```markdown
[![CI Build](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/ci.yml/badge.svg)](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/ci.yml)
[![Security Scan](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/security.yml/badge.svg)](https://github.com/jdavidtorres/expense-tracker-app/actions/workflows/security.yml)
```

### Regular Maintenance

1. **Weekly**: Review dependency update issues
2. **Monthly**: Update GitHub Actions to latest versions
3. **Quarterly**: Review and update security policies
4. **As Needed**: Add new build targets or quality checks

## Troubleshooting

### Common Issues

1. **MAUI Workload Installation Failures**
   - Ensure the runner has sufficient disk space
   - Check for .NET SDK compatibility
   - Verify workload names are correct

2. **Android Build Failures**
   - Verify Java 17 is properly installed
   - Check Android SDK availability
   - Review target framework versions

3. **Code Formatting Failures**
   - Run `dotnet format` locally before pushing
   - Ensure .editorconfig is properly configured
   - Check for conflicting formatting rules

4. **Cache Issues**
   - Clear NuGet caches if builds fail inconsistently
   - Update cache keys when dependencies change
   - Monitor cache hit rates

### Build Performance

- **Average CI Duration**: ~15-20 minutes for full pipeline
- **Shared Build**: ~3-5 minutes
- **Platform Builds**: ~5-8 minutes each
- **Security Scans**: ~5-10 minutes

## Future Enhancements

1. **Add Unit Tests**: Implement comprehensive test coverage
2. **Integration Tests**: Add API and UI integration testing
3. **Performance Monitoring**: Add build time and artifact size tracking
4. **Advanced Caching**: Implement more granular caching strategies
5. **Release Automation**: Prepare for future app store deployments
6. **Multi-Environment**: Add staging/production environment configurations

## Contributing

When contributing to this repository:

1. Ensure your code passes all CI checks
2. Add tests for new functionality
3. Update documentation for new features
4. Follow the established code formatting standards
5. Review security scan results and address any issues

The CI pipeline is designed to catch issues early and maintain high code quality standards across all supported platforms.