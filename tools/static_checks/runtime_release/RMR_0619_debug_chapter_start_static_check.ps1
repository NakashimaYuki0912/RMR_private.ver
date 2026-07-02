$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
param(
    [string]$Root = (Split-Path -Parent $MyInvocation.MyCommand.Path)
)

$ErrorActionPreference = 'Stop'

function Read-Text([string]$Path) {
    return [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
}

function Assert-Contains([string]$Text, [string]$Needle, [string]$Message) {
    if ($Text.IndexOf($Needle, [System.StringComparison]::Ordinal) -lt 0) {
        throw $Message
    }
}

function Assert-NotContains([string]$Text, [string]$Needle, [string]$Message) {
    if ($Text.IndexOf($Needle, [System.StringComparison]::Ordinal) -ge 0) {
        throw $Message
    }
}

$logLikeModPath = Join-Path $Root 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$corePath = Join-Path $Root 'RMR_Core.cs'

$logLikeMod = Read-Text $logLikeModPath
$core = Read-Text $corePath

Assert-Contains $logLikeMod 'ResolveDebugStartStage' 'Debug chapter saves must resolve the active gamemode StageStart instead of using a hard-coded rest stage.'
Assert-Contains $logLikeMod 'RMRCore.CurrentGamemode.StageStart' 'Debug chapter saves must persist the current gamemode StageStart as curstage.'
Assert-NotContains $logLikeMod 'new LorId(LogLikeMod.ModId, 855).LogGetSaveData()' 'Debug chapter saves must not hard-code generic rest stage 855 as curstage.'
Assert-Contains $core 'LogLikeMod.curstageid = this.StageStart;' 'DebugCh6 initialization must keep curstageid aligned to its StageStart before the auto-end initial event.'
Assert-Contains $core 'LogLikeMod.curstagetype = abcdcode_LOGLIKE_MOD.StageType.Start;' 'DebugCh6 must remain a Start-stage route entry, not a Rest-stage route entry.'
Assert-Contains $core 'LogLikeMod.nextlist = LogueBookModels.GetNextList(ChapterGrade.Grade6, true);' 'DebugCh6 initial event must populate Grade6 nextlist before ending the dummy stage.'

Write-Host 'PASS: debug chapter start-stage guards are present.'

