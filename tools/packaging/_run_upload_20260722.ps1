# One-off wrapper: upload 2026-07-22 build (key-page hover preview layer fix).
# Description on the Steam page is preserved (scraped and written back untouched).
$note = @"
[h2]2026-07-22 Update[/h2]
[list]
[*][b]Key Page hover preview fix[/b]: in the battle-prep inventory, hovering a Key Page now correctly shows the enlarged preview panel on top (it was being painted over by the raised RMR inventory layer).
[*]The preview layer boost is cleanly restored when the preview hides or when leaving the Key Page tab, so other UI ordering is unaffected.
[/list]

[h2]2026-07-22 更新[/h2]
[list]
[*][b]钥匙页悬停预览修复[/b]：战斗准备界面中，鼠标悬停钥匙页时放大预览面板现在会正确显示在最上层（此前被抬高的RMR库存图层遮挡）。
[*]预览关闭或离开钥匙页页签时会还原图层设置，不影响其他界面层级。
[/list]
"@.Trim()

# Invoke in-process so the multiline changenote stays one argument.
& "$PSScriptRoot\upload_workshop_preserve_desc.ps1" -ChangeNote $note
exit $LASTEXITCODE
