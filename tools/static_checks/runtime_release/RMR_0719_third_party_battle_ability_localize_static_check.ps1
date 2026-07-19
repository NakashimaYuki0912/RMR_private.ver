$ErrorActionPreference = 'Stop'

$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}

$sourcePath = Join-Path $script:RepoRoot 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$source = [System.IO.File]::ReadAllText($sourcePath, [System.Text.Encoding]::UTF8)

function Assert-Contains {
    param(
        [string]$Name,
        [string]$Text,
        [string]$Needle
    )

    if (-not $Text.Contains($Needle)) {
        throw "$Name missing: $Needle"
    }
}

function Assert-Matches {
    param(
        [string]$Name,
        [string]$Text,
        [string]$Pattern
    )

    if (-not [System.Text.RegularExpressions.Regex]::IsMatch(
        $Text,
        $Pattern,
        [System.Text.RegularExpressions.RegexOptions]::Singleline)) {
        throw "$Name missing control flow: $Pattern"
    }
}

function Get-Section {
    param(
        [string]$Text,
        [string]$Start,
        [string]$End
    )

    $startIndex = $Text.IndexOf($Start, [System.StringComparison]::Ordinal)
    if ($startIndex -lt 0) {
        throw "Section start missing: $Start"
    }
    $endIndex = $Text.IndexOf($End, $startIndex + $Start.Length, [System.StringComparison]::Ordinal)
    if ($endIndex -lt 0) {
        throw "Section end missing: $End"
    }
    return $Text.Substring($startIndex, $endIndex - $startIndex)
}

function Assert-InOrder {
    param(
        [string]$Name,
        [string]$Text,
        [string[]]$Needles
    )

    $cursor = -1
    foreach ($needle in $Needles) {
        $cursor = $Text.IndexOf($needle, $cursor + 1, [System.StringComparison]::Ordinal)
        if ($cursor -lt 0) {
            throw "$Name missing or out of order: $needle"
        }
    }
}

$refresh = Get-Section $source `
    'public static void RefreshVanillaBattleLocalize(string language, string reason)' `
    'public static void ReloadModCardAndDiceLocalize(string language)'

Assert-InOrder 'third-party ability descriptions survive the vanilla reload' $refresh @(
    'SnapshotBattleCardAbilityDescriptions()',
    'loader.LoadBattleCardAbilityDescriptions(language);',
    'RestoreMissingBattleCardAbilityDescriptions('
)
Assert-Matches 'ability description restoration runs from the loader finally block' $refresh `
    'try\s*\{\s*loader\.LoadBattleCardAbilityDescriptions\(language\);\s*\}\s*finally\s*\{\s*restoredAbilityDescriptions\s*=\s*RestoreMissingBattleCardAbilityDescriptions\('

$snapshot = Get-Section $source `
    'private static Dictionary<string, BattleCardAbilityDesc> SnapshotBattleCardAbilityDescriptions()' `
    'private static int RestoreMissingBattleCardAbilityDescriptions('
Assert-Contains 'snapshot copies the pre-reload dictionary' $snapshot 'new Dictionary<string, BattleCardAbilityDesc>(current)'

$restore = Get-Section $source `
    'private static int RestoreMissingBattleCardAbilityDescriptions(' `
    'public static void RefreshVanillaBattleLocalize(string language, string reason)'
Assert-Matches 'restore skips keys reloaded by the selected vanilla language before adding' $restore `
    'if\s*\(current\.ContainsKey\(entry\.Key\)\)\s*continue;\s*current\.Add\(entry\.Key, entry\.Value\);'
Assert-Contains 'restore re-adds only missing pre-reload descriptions' $restore 'current.Add(entry.Key, entry.Value);'
Assert-Contains 'restore ignores unusable entries' $restore 'entry.Value == null'
Assert-Contains 'snapshot failure degrades without blocking vanilla reload' $snapshot 'Could not snapshot battle ability descriptions'
Assert-Contains 'restore failure degrades without masking loader failures' $restore 'Could not restore battle ability descriptions'

Write-Host 'RMR third-party battle ability localization preservation check passed.'
