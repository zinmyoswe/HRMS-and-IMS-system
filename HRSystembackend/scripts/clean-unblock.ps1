<#
PowerShell helper to remove build outputs and any Swashbuckle assemblies that may be blocked by AppLocker/WDAC.
Run as Administrator if possible.
#>
param(
    [string]$ProjectPath = "HRSystembackend/HRSystembackend",
    [switch]$DoRestore
)

Write-Host "Working directory: $(Get-Location)"
Write-Host "ProjectPath: $ProjectPath"

# Ensure path separators
$projBin = Join-Path $ProjectPath 'bin'
$projObj = Join-Path $ProjectPath 'obj'

Write-Host "Removing bin and obj folders for project: $projBin and $projObj"
Try {
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue $projBin, $projObj
    Write-Host "Removed bin/obj (if existed)."
} Catch {
    Write-Warning "Failed to remove some files: $_"
}

# Remove any Swashbuckle DLLs under the repo (output folders)
Write-Host "Searching for Swashbuckle assemblies to remove..."
$swAssemblies = Get-ChildItem -Path . -Recurse -Filter 'Swashbuckle*.dll' -ErrorAction SilentlyContinue
if ($swAssemblies) {
    foreach ($a in $swAssemblies) {
        try {
            Write-Host "Removing $($a.FullName)"
            Remove-Item -Force -ErrorAction SilentlyContinue $a.FullName
        } catch {
            Write-Warning "Could not remove $($a.FullName): $_"
        }
    }
} else {
    Write-Host "No Swashbuckle assemblies found in repo output folders."
}

# Clear NuGet caches
Write-Host "Clearing NuGet caches..."
dotnet nuget locals all --clear

if ($DoRestore) {
    Write-Host "Restoring packages..."
    dotnet restore
}

Write-Host "Done. Try rebuilding the project: dotnet build HRSystembackend/HRSystembackend.csproj"
Write-Host "If the FileLoadException persists, this likely indicates an OS Application Control policy (AppLocker/WDAC) blocking assembly execution."
Write-Host "In that case, consult your security/IT team to allow the assembly or disable the policy for this path."
