#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Create NuGet packages.
# --------------------------------------------------------------------------------------------------

export BUILD_NUMBER=$1
export SOURCE_SHA=$2

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

package ()
{
    info "Package: $1"
    dotnet pack "src/$1/$1.csproj" \
        --output "$(pwd)/artifacts" \
        --configuration Release \
        --include-source \
        --include-symbols \
        /p:TreatWarningsAsErrors=true /warnaserror
}

info "Start packaging (buildnumber: '$BUILD_NUMBER', sourcesha: '$SOURCE_SHA')"
package TypedTree.Generator.Core
package TypedTree.Generator.Cli

info "Finished packaging"
exit 0
