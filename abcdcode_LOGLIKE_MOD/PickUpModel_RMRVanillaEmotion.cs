using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LOR_XML;
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
                AbnormalityCard desc = Singleton<AbnormalityCardDescXmlList>.Instance.GetAbnormalityCard(_script);
                if (desc != null)
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
