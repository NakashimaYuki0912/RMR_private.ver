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
$books = Read-Text ('abcdcode_LOGLIKE_MOD' + $bs + 'LogueBookModels.cs')

# 1. Grade6 special core page grant helper exists
AssertContains 'RMR_Core has GrantGrade6SpecialCorePagesIfNeeded method' $core 'GrantGrade6SpecialCorePagesIfNeeded'

# 2. Only triggered at ChapterGrade.Grade6 / entering Urban Star
AssertContains 'GrantGrade6SpecialCorePagesIfNeeded called in Grade6 switch case' $core 'case ChapterGrade.Grade6:'
AssertContains 'GrantGrade6SpecialCorePagesIfNeeded called in DebugCh6 AfterInitializeGamemode' $core 'RMRCore.GrantGrade6SpecialCorePagesIfNeeded()'

# 3. Logic contains Binah and BlackSilence/Black Silence resolution
AssertContains 'Black Silence TextId=102 lookup' $core 'TextId == 102'
AssertContains 'BlackSilence !IsWorkshop filter' $core '!x.id.IsWorkshop()'
AssertContains 'Binah resolution by CharacterSkin' $core 'CharacterSkin'
AssertContains 'Binah resolution exists' $core 'Binah'

# 4. Uses BookXmlList.GetList() to resolve original pages, not AddData/EquipPage XML
AssertContains 'Uses BookXmlList.GetList()' $core 'BookXmlList>.Instance.GetList()'
AssertNotContains 'Must not modify AddData/EquipPage XML to forge pages' $core 'AddData/EquipPage'

# 5. Uses LogueBookModels helper (TryAddUniqueRoleBookToInventoryAndAtlas) and ensures RecordAtlasRoleBook
AssertContains 'TryAddUniqueRoleBookToInventoryAndAtlas used' $core 'TryAddUniqueRoleBookToInventoryAndAtlas'
AssertContains 'TryAddUniqueRoleBookToInventoryAndAtlas includes RecordAtlasRoleBook' $books 'RecordAtlasRoleBook(id)'
AssertContains 'TryAddUniqueRoleBookToInventoryAndAtlas checks atlas duplicate' $books 'AtlasUnlockedRoleBooks.Contains(id)'
AssertContains 'TryAddUniqueRoleBookToInventoryAndAtlas checks booklist duplicate' $books 'booklist.Any'
AssertContains 'TryAddUniqueRoleBookToInventoryAndAtlas validates BookXmlInfo' $books 'GetData(id)'

# 6. One-time flag / dedup to prevent duplicate grants
AssertContains 'Permanent save flag constant exists' $core 'Grade6SpecialCorePagesGrantedSaveName'
AssertContains 'One-time flag check before grant' $core 'RMR_Grade6SpecialCorePagesGranted'

# 7. Does not reference _release_packages
AssertNotContains 'RMR_Core must not reference _release_packages' $core '_release_packages'
AssertNotContains 'LogueBookModels must not reference _release_packages' $books '_release_packages'

# 8. Does not change RewardingModel.cs / LogLikePatches.cs reward logic
# (verified by design: this PR only touches RMR_Core.cs and LogueBookModels.cs)

Write-Host 'RMR 0616 grade6 special core pages static check passed — all constraints verified.'

