﻿using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Template = MorvarchsPlaystyleExpansions.Common.CommonReferencedTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;

namespace MorvarchsPlaystyleExpansions.Feats
{
    public static class DivineFightingTechniques
    {
        public static void Configure()
        {
            ConfigureShootingStar();
        }

        public static void ConfigureShootingStar()
        {
            string FeatName = "ShootingStarFeat";

            FeatureConfigurator.New(FeatName, "3643D358-1387-461F-A0E5-14954AF42828")
                .SetDisplayName(LocalizationTool.CreateString("ShootingStarName", "Way of the Shooting Star", false))
                .SetDescription(LocalizationTool.CreateString("ShootingStarhDescription", "Utilizing the fighting techniques channeled through Desna, add your charisma to attack and damage with starknives instead of dexterity or strength.", false))
                .AddFeatureTagsComponent(FeatureTag.Attack)
                .AddFeatureTagsComponent(FeatureTag.Damage)
                .AddFeatureTagsComponent(FeatureTag.Melee)
                .SetGroups(FeatureGroup.Feat, FeatureGroup.CombatFeat)
                .AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.ChaoticGood)
                .AddPrerequisiteParametrizedWeaponFeature(Template.WeaponFocus, Kingmaker.Enums.WeaponCategory.Starknife)
                .AddRecommendationStatComparison(Kingmaker.EntitySystem.Stats.StatType.Charisma, Kingmaker.EntitySystem.Stats.StatType.Dexterity, 4)
                .AddRecommendationHasFeature(Template.Desna)
                .AddWeaponTypeDamageStatReplacement(Kingmaker.Enums.WeaponCategory.Starknife, false, Kingmaker.EntitySystem.Stats.StatType.Charisma, false)
                //.AddAttackTypeAttackBonus(true, null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail, Kingmaker.EntitySystem.Stats.StatType.Charisma, Kingmaker.Enums.WeaponSubCategory.None, new List<Blueprint<BlueprintWeaponTypeReference>> { Template.Starknife })
                .Configure();

            FeatureSelectionConfigurator.For(Template.BasicFeatSelectionGuid).AddToAllFeatures(FeatName).Configure();
        }
    }
}
