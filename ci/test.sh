#!/bin/bash
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Run the xunit tests.
# --------------------------------------------------------------------------------------------------

TEST_RESULT_PATH="../../artifacts/xunit.results.xml"

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

info "Starting tests"

# Build the solution in Debug configuration (So that Debug.Asserts will fire)
dotnet build --configuration Debug src/TypedTreeGenerator.sln

# Run test
dotnet test src/TypedTreeGenerator.Tests/TypedTreeGenerator.Tests.csproj \
    --logger "xunit;LogFilePath=$TEST_RESULT_PATH"
EXIT_CODE=$?

info "Finished tests"
exit $EXIT_CODE
