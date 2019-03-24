#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Build the solution.
# --------------------------------------------------------------------------------------------------

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

info "Starting build"
dotnet build src/TypedTreeGenerator.sln

info "Finished build"
exit 0
