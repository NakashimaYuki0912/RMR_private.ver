$ErrorActionPreference = 'Stop'

$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
$bs = [char]92
$root = $script:RepoRoot
function Read-Text($rel) { Get-Content (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($label, $text, $needle) {
    if ($text -cnotlike "*$needle*") { throw "$label missing: $needle" }
}
function AssertNotContains($label, $text, $needle) {
    if ($text -clike "*$needle*") { throw "$label forbidden: $needle" }
}

$core = Read-Text 'RMR_Core.cs'
$buildOut = Join-Path $env:TEMP 'rmr_build_out\RogueLike Mod Reborn.dll'

# ============================================================
# 1. LoadVanillaCardArt 不含 _sprite = 直接字段赋值
# ============================================================
AssertNotContains 'LoadVanillaCardArt must not have _sprite = direct assignment' $core '_sprite = sprite'
AssertNotContains 'LoadVanillaCardArt must not have _sprite = in object initializer' $core '_sprite ='

Write-Host '[PASS] 1. No _sprite = direct assignment in LoadVanillaCardArt.'

# ============================================================
# 2. Workshop 图集列表都有 null 初始化
# ============================================================
AssertContains 'list must be null-initialized with ?? new List' $core '?? new List<ArtworkCustomizeData>()'
AssertContains 'listLog must null-check and initialize' $core 'listLog == null'

Write-Host '[PASS] 2. Workshop artwork lists have null initialization.'

# ============================================================
# 3. 反射字段缺失和 SetValue 异常不会逃出初始化
# ============================================================
AssertContains 'ArtworkSpriteField property must null-check cached field' $core 'if (_cachedArtworkSpriteField == null)'
AssertContains 'ArtworkSpriteField must log error when field not found' $core 'Cannot find field ArtworkCustomizeData._sprite'
AssertContains 'LoadVanillaCardArt must guard spriteField == null' $core 'if (spriteField == null)'
AssertContains 'LoadVanillaCardArt must try-catch SetValue per sprite' $core 'try'
AssertContains 'SetValue must be inside try-catch' $core 'catch (Exception ex)'
AssertContains 'SetValue failure must log warning' $core 'Failed to set _sprite'

Write-Host '[PASS] 3. Reflection field lookup and SetValue are exception-safe.'

# ============================================================
# 4. OnInitializeMod 中图集加载失败不会中断后续初始化
# ============================================================
AssertContains 'OnInitializeMod must wrap LoadVanillaCardArt in try-catch' $core 'try { LoadVanillaCardArt(); }'
AssertContains 'Catch must log error and continue' $core 'Continuing initialization without vanilla card artwork'

Write-Host '[PASS] 4. LoadVanillaCardArt failure does not block initialization.'

# ============================================================
# 5. BuildTimestamp 已更新，不再使用旧时间戳
# ============================================================
AssertNotContains 'BuildTimestamp must not be 2026-06-17T02:00Z' $core '2026-06-17T02:00Z'
$today = Get-Date -Format 'yyyy-MM-dd'
AssertContains "BuildTimestamp must be updated to $today" $core $today

Write-Host '[PASS] 5. BuildTimestamp updated to current date.'

# ============================================================
# 6. 输出 DLL 不含对 ArtworkCustomizeData._sprite 的 stfld
# ============================================================
if (Test-Path $buildOut) {
    $cecilPath = Join-Path $root 'dependencies\Mono.Cecil.dll'
    if (-not (Test-Path $cecilPath)) {
        throw "Mono.Cecil.dll not found at $cecilPath"
    }

    Add-Type -Path $cecilPath
    $assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($buildOut)
    try {
        $hits = @()
        foreach ($type in $assembly.MainModule.Types) {
            foreach ($method in $type.Methods) {
                if (-not $method.HasBody) {
                    continue
                }
                foreach ($instruction in $method.Body.Instructions) {
                    if ($instruction.OpCode.Code -eq [Mono.Cecil.Cil.Code]::Stfld -and
                        $instruction.Operand -and
                        $instruction.Operand.ToString() -match 'Workshop.ArtworkCustomizeData::_sprite') {
                        $hits += "$($type.FullName)::$($method.Name)"
                    }
                }
            }
        }
        if ($hits.Count -gt 0) {
            throw "Mono.Cecil check found stfld ArtworkCustomizeData._sprite in: $($hits -join ', ')"
        }
    }
    finally {
        $assembly.Dispose()
    }
    Write-Host '[PASS] 6. Output DLL has no stfld ArtworkCustomizeData._sprite (Mono.Cecil verified).'
} else {
    throw "Output DLL not found at $buildOut"
}

Write-Host ''
Write-Host 'RMR 0618 startup crash static check — ALL CHECKS PASSED.'
Write-Host ''

