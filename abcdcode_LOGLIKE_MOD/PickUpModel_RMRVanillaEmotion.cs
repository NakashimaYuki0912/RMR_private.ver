using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LOR_XML;
using RogueLike_Mod_Reborn;
using UnityEngine;

namespace abcdcode_LOGLIKE_MOD
{
    public class PickUpModel_RMRVanillaEmotion : PickUpModelBase
    {
        private readonly string _script;

        public PickUpModel_RMRVanillaEmotion(string script)
        {
            _script = script ?? string.Empty;
            LoadText();
        }

        public static bool HasVanillaAbility(string script)
        {
            return FindAbilityType(script) != null;
        }

        public static PickUpModel_RMRVanillaEmotion TryCreate(string script)
        {
            return HasVanillaAbility(script) ? new PickUpModel_RMRVanillaEmotion(script) : null;
        }

        public override bool IsCanPickUp(UnitDataModel target)
        {
            return target != null && !CheckDead(target);
        }

        public override void OnPickUp()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Player))
                ApplyTo(unit);
        }

        public override void OnPickUp(BattleUnitModel model)
        {
            ApplyTo(model);
        }

        private void ApplyTo(BattleUnitModel model)
        {
            if (model == null || rewardinfo == null)
                return;

            EmotionCardAbilityBase ability = CreateAbility(_script);
            EmotionCardXmlInfo card = LogLikeMod.GetRegisteredPickUpXml(rewardinfo);
            if (ability == null || card == null)
            {
                Debug.LogError($"[RMRVanillaEmotion] Failed to apply vanilla emotion card: {_script}");
                return;
            }
            CreaturePickUpModel.ApplyEmotionCard(card, ability, model);
        }

        private void LoadText()
        {
            try
            {
                AbnormalityCard desc = ResolveAbnormalityDesc(_script);
                if (desc != null && !IsMissingDesc(desc))
                {
                    Name = string.IsNullOrEmpty(desc.cardName) ? _script : desc.cardName;
                    Desc = desc.abilityDesc ?? string.Empty;
                    FlaverText = desc.flavorText ?? string.Empty;
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[RMRVanillaEmotion] Failed to load text for {_script}: {ex.Message}");
            }
            Name = _script;
            Desc = string.Empty;
            FlaverText = string.Empty;
        }

        /// <summary>
        /// Vanilla AbnormalityCards.xml is keyed by EmotionCard.Name (e.g. SnowWhite_Vine),
        /// while reward scripts are snowwhite1. Looking up by script alone yields "Not found"
        /// and previously overwrote the good Name-keyed entry during GetReward injection.
        /// </summary>
        public static AbnormalityCard ResolveAbnormalityDesc(string script)
        {
            if (string.IsNullOrEmpty(script))
                return null;

            AbnormalityCardDescXmlList list = Singleton<AbnormalityCardDescXmlList>.Instance;
            if (list == null)
                return null;

            // 1) Prefer the vanilla EmotionCard.Name (SnowWhite_Vine / RedShoes_Kirakira / …).
            EmotionCardXmlInfo vanilla = RMRAbnormalityUnlockManager.FindVanillaEmotionCard(script);
            if (vanilla != null && !string.IsNullOrEmpty(vanilla.Name))
            {
                AbnormalityCard byName = list.GetAbnormalityCard(vanilla.Name);
                if (byName != null && !IsMissingDesc(byName))
                    return byName;
            }

            // 2) Registered pickup may already hold the resolved Name.
            if (LogLikeMod.PickUpXml_Dummy_Passive != null)
            {
                foreach (var kv in LogLikeMod.PickUpXml_Dummy_Passive)
                {
                    if (kv.Value == null)
                        continue;
                    EmotionCardXmlInfo registered = kv.Value.Find(c =>
                        c != null
                        && c.Script != null
                        && c.Script.Any(s => string.Equals(s, script, StringComparison.OrdinalIgnoreCase)));
                    if (registered != null && !string.IsNullOrEmpty(registered.Name)
                        && !string.Equals(registered.Name, script, StringComparison.OrdinalIgnoreCase))
                    {
                        AbnormalityCard byRegistered = list.GetAbnormalityCard(registered.Name);
                        if (byRegistered != null && !IsMissingDesc(byRegistered))
                            return byRegistered;
                    }
                }
            }

            // 3) Fall back to script key (works for cards whose Name == Script).
            AbnormalityCard byScript = list.GetAbnormalityCard(script);
            if (byScript != null && !IsMissingDesc(byScript))
                return byScript;

            return byScript;
        }

        public static bool IsMissingDesc(AbnormalityCard desc)
        {
            if (desc == null)
                return true;
            return IsMissingText(desc.cardName)
                && IsMissingText(desc.abilityDesc)
                && IsMissingText(desc.flavorText);
        }

        public static bool IsMissingText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return true;
            return string.Equals(text, "Not found", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "Not Found", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get or create a dictionary-backed AbnormalityCard for <paramref name="key"/>.
        /// GetAbnormalityCard may return a transient "Not found" stub that is intentionally
        /// not cached (to avoid poisoning real abno locales). Writers must upsert here so
        /// LevelUpUI / stage pick can later read the filled text.
        /// </summary>
        public static AbnormalityCard EnsureDescEntry(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            AbnormalityCardDescXmlList list = Singleton<AbnormalityCardDescXmlList>.Instance;
            if (list == null)
                return null;

            FieldInfo field = typeof(AbnormalityCardDescXmlList).GetField("_dictionary", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            Dictionary<string, AbnormalityCard> dictionary = field != null
                ? field.GetValue(list) as Dictionary<string, AbnormalityCard>
                : null;
            if (dictionary == null)
                return null;

            if (dictionary.TryGetValue(key, out AbnormalityCard existing) && existing != null)
                return existing;

            AbnormalityCard created = new AbnormalityCard()
            {
                id = key,
                abnormalityName = string.Empty,
                cardName = string.Empty,
                abilityDesc = string.Empty,
                flavorText = string.Empty,
                dialogues = null
            };
            dictionary[key] = created;
            return created;
        }

        /// <summary>
        /// Inject resolved name/desc into the AbnormalityCard slot used by the pick UI.
        /// Never overwrites a good vanilla entry with "Not found".
        /// </summary>
        public static void InjectResolvedDesc(EmotionCardXmlInfo card, PickUpModelBase pickUp)
        {
            if (card == null)
                return;
            string key = !string.IsNullOrEmpty(card.Name) ? card.Name
                : (card.Script != null && card.Script.Count > 0 ? card.Script[0] : null);
            if (string.IsNullOrEmpty(key))
                return;

            // Must be dictionary-backed — transient GetAbnormalityCard stubs are discarded.
            AbnormalityCard target = EnsureDescEntry(key);
            if (target == null)
                return;

            // Prefer PickUpModel text when it is real localized content (custom PickUpModels).
            if (pickUp != null && !IsMissingText(pickUp.Name))
            {
                target.cardName = pickUp.Name;
                if (!IsMissingText(pickUp.FlaverText))
                    target.flavorText = pickUp.FlaverText;
                if (!string.IsNullOrEmpty(pickUp.Desc))
                    target.abilityDesc = pickUp.Desc;
                return;
            }

            // Otherwise keep / re-resolve vanilla text by script.
            string script = card.Script != null && card.Script.Count > 0 ? card.Script[0] : null;
            AbnormalityCard resolved = ResolveAbnormalityDesc(script);
            if (resolved != null && !IsMissingDesc(resolved))
            {
                // If resolved is a different dictionary entry, copy fields onto the UI key entry.
                if (!ReferenceEquals(resolved, target))
                {
                    target.cardName = resolved.cardName;
                    target.flavorText = resolved.flavorText;
                    target.abilityDesc = resolved.abilityDesc;
                    target.abnormalityName = resolved.abnormalityName;
                    if (resolved.dialogues != null)
                        target.dialogues = resolved.dialogues;
                }
            }
        }

        private static EmotionCardAbilityBase CreateAbility(string script)
        {
            Type type = FindAbilityType(script);
            return type == null ? null : Activator.CreateInstance(type) as EmotionCardAbilityBase;
        }

        private static Type FindAbilityType(string script)
        {
            if (string.IsNullOrEmpty(script))
                return null;

            string typeName = "EmotionCardAbility_" + script.Trim();
            IEnumerable<Assembly> assemblies = LogLikeMod.GetAssemList()
                .Concat(new[] { typeof(EmotionCardAbilityBase).Assembly })
                .Distinct();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (string.Equals(type.Name, typeName, StringComparison.OrdinalIgnoreCase)
                        && typeof(EmotionCardAbilityBase).IsAssignableFrom(type))
                        return type;
                }
            }
            return null;
        }
    }
}
