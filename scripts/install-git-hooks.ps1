$ErrorActionPreference = 'Stop'

$repositoryRoot = git rev-parse --show-toplevel
if ($LASTEXITCODE -ne 0) {
    throw 'Run this script from inside the HighLoadShop Git repository.'
}

if (-not (Get-Command gitleaks -ErrorAction SilentlyContinue)) {
    throw 'Gitleaks is not installed. Install it from https://github.com/gitleaks/gitleaks#installing and run this script again.'
}

git -C $repositoryRoot config core.hooksPath .githooks
if ($LASTEXITCODE -ne 0) {
    throw 'Could not configure Git to use the repository hooks.'
}

Write-Host 'Git hooks installed. The pre-commit hook will scan staged changes with Gitleaks.'
