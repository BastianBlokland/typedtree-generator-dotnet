#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Create NuGet packages.
# --------------------------------------------------------------------------------------------------

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

package ()
{
    info "Package: $1"
    dotnet pack "src/$1/$1.csproj" \
        --output "artifacts" \
        --configuration Release \
        --include-source \
        --include-symbols \
        /p:TreatWarningsAsErrors=true /warnaserror
}

info "Start packaging"
package TypedTree.Generator.Core
package TypedTree.Generator.Cli

info "Finished packaging"
exit 0
