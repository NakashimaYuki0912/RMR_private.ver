# Export cn/en/kr UIs.txt as a CSV for translators (id, cn, en, kr).
param(
    [string]$ProjectRoot = "",
    [string]$OutFile = ""
)

$ErrorActionPreference = "Stop"
if (-not $ProjectRoot) {
    # tools/localization -> tools -> project root
    $ProjectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
}
$loc = Join-Path $ProjectRoot "Localize"
if (-not $OutFile) {
    $OutFile = Join-Path $ProjectRoot "docs\localization\UI_KEYS_export.csv"
}

function Get-Map([string]$path) {
    $map = @{}
    $raw = [IO.File]::ReadAllText($path)
    foreach ($m in [regex]::Matches($raw, '<text\s+id="([^"]+)">(.*?)</text>', 'Singleline')) {
        $map[$m.Groups[1].Value] = $m.Groups[2].Value -replace "`r|`n", " "
    }
    return $map
}

$cn = Get-Map (Join-Path $loc "cn\UIs.txt")
$en = Get-Map (Join-Path $loc "en\UIs.txt")
$kr = Get-Map (Join-Path $loc "kr\UIs.txt")
$ids = ($cn.Keys + $en.Keys + $kr.Keys) | Sort-Object -Unique

function CsvEsc([string]$s) {
    if ($null -eq $s) { $s = "" }
    $s = $s.Replace('"', '""')
    return '"' + $s + '"'
}

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add("id,cn,en,kr")
foreach ($id in $ids) {
    $c = if ($cn.ContainsKey($id)) { $cn[$id] } else { "" }
    $e = if ($en.ContainsKey($id)) { $en[$id] } else { "" }
    $k = if ($kr.ContainsKey($id)) { $kr[$id] } else { "" }
    $lines.Add( (CsvEsc $id) + "," + (CsvEsc $c) + "," + (CsvEsc $e) + "," + (CsvEsc $k) )
}

$dir = Split-Path $OutFile -Parent
if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Force -Path $dir | Out-Null }
$utf8 = New-Object System.Text.UTF8Encoding $true
[IO.File]::WriteAllLines($OutFile, $lines, $utf8)
Write-Host "Wrote $($ids.Count) rows -> $OutFile"
