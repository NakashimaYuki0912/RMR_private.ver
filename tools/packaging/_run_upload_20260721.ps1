# One-off wrapper: upload current build with Floor Realization changenote.
# Description on the Steam page is preserved (scraped and written back untouched).
$note = @"
[h2]2026-07-21 Update[/h2]
[list]
[*][b]Floor Realization reward isolation[/b]: Realization-exclusive Abnormality Pages and E.G.O. are now strictly gated behind each Floor's first Realization clear (triple-checked by passive ID range, script whitelist and unique-level).
[*]Normal battle reward pools, shop pools and permanent-tier unlock pools no longer leak locked Realization-exclusive pages.
[*]Loading an existing run save now sanitizes any locked exclusive pages that leaked in from older versions.
[/list]

[h2]2026-07-21 更新[/h2]
[list]
[*][b]解放战奖励隔离[/b]：解放战专属异想体书页与E.G.O现在严格绑定各楼层解放战首通解锁（被动ID段、脚本白名单、Unique等级三重校验）。
[*]普通战斗奖励池、商店池与永久层级解锁池不再混入未解锁的解放战专属书页。
[*]读取旧存档时会自动清理此前版本混入的未解锁专属书页。
[/list]
"@.Trim()

# Invoke in-process so the multiline changenote stays one argument.
& "$PSScriptRoot\upload_workshop_preserve_desc.ps1" -ChangeNote $note -SkipPrepare
exit $LASTEXITCODE
