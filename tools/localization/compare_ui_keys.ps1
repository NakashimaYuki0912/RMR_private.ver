# Compare Localize/{cn,en,kr}/UIs.txt key sets.
# Exit 0 if all match; exit 1 if any language is missing keys.
param(
    [string]$ProjectRoot = ""
)

$ErrorActionPreference = "Stop"
if (-not $ProjectRoot) {
    # tools/localization -> tools -> project root
    $ProjectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}
$loc = Join-Path $ProjectRoot "Localize"
if (-not (Test-Path $loc)) { throw "Localize not found under $ProjectRoot" }

function Get-Keys([string]$path) {
    if (-not (Test-Path $path)) { return @() }
    $raw = [IO.File]::ReadAllText($path)
    return @([regex]::Matches($raw, 'id="([^"]+)"') | ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique)
}

$cn = Get-Keys (Join-Path $loc "cn\UIs.txt")
$en = Get-Keys (Join-Path $loc "en\UIs.txt")
$kr = Get-Keys (Join-Path $loc "kr\UIs.txt")

Write-Host "UIs.txt key counts: cn=$($cn.Count) en=$($en.Count) kr=$($kr.Count)"

function Show-Missing($name, $base, $other) {
    $miss = @($base | Where-Object { $other -notcontains $_ })
    if ($miss.Count -eq 0) {
        Write-Host ("  {0}: OK" -f $name) -ForegroundColor Green
        return 0
    }
    Write-Host ("  {0}: MISSING {1} keys" -f $name, $miss.Count) -ForegroundColor Red
    $miss | ForEach-Object { Write-Host "    - $_" }
    return $miss.Count
}

$fail = 0
$fail += Show-Missing "en vs cn" $cn $en
$fail += Show-Missing "kr vs cn" $cn $kr
$fail += Show-Missing "cn vs en (extra in en?)" $en $cn

if ($fail -gt 0) {
    Write-Host "FAILED ($fail missing entries)." -ForegroundColor Red
    exit 1
}
Write-Host "All UIs.txt key sets match." -ForegroundColor Green
exit 0
