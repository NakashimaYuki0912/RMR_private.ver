# ADR-001: Selected Hub, Handbook, and Atlas UI

## Status

Accepted

## Date

2026-07-12

## Context

The RMR start surfaces had drifted into plain centered panels and text-only lists. They no longer matched the visual language of *Library of Ruina*, and the handbook duplicated a scrollbar with separate body-page controls. The Atlas also needed an explicit rule for which collections use urban-progress filters.

Three local HTML variants were reviewed for each surface before production implementation.

## Decision

- **Hub A — invitation index:** full-screen reception illustration, left identity/sigil block, and right vertical action index. Existing actions and gates remain unchanged.
- **Handbook H-A — illustrated index:** six illustrated chapter entries on the left, an illustrated header on the right, and one continuous scrollbar. No body-page number or previous/next buttons.
- **Atlas A-A — archive wall:** four category rails on the left, a 4x5 collection wall in the center, and a persistent detail panel on the right.
- Role books and combat pages may be filtered by the urban stage where they are obtained.
- Abnormality pages and E.G.O. combat pages are flat permanent collections. Their UI hides the progress header and section buttons, resets the active section to `All`, and ignores section filtering.
- All new TMP text uses `ApplyTmpFontPreservingSharpMaterial` and `FontStyles.Normal` for CJK clarity.

## Alternatives Considered

### Centered Hub card

Rejected because it flattened every action into the same visual weight and did not resemble a reception invitation.

### Handbook dossier or continuous city timeline

Rejected in favor of H-A because the illustrated chapter index makes targeted lookup faster while keeping continuous scrolling inside each chapter.

### Atlas bookshelf or single-card catalogue

Rejected because both reduce information density. A-A is better for hundreds of permanent collection entries while retaining a large detail view.

## Consequences

- The three surfaces share dark reception backgrounds, restrained gold rules, cream text, and red only for destructive actions.
- Hub behavior, save checks, realization gates, and callbacks are unchanged; only visual construction changes.
- Atlas stage-filter controls must never be shown for abnormality or E.G.O. categories.
- `tools/static_checks/shop_atlas/RMR_0712_selected_ui_static_check.ps1` guards the accepted structure and CJK font rules.
