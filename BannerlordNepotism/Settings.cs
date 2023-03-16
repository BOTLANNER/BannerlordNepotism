using System.Collections.Generic;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
#if MCM_v5
using MCM.Abstractions.Base.Global;
#else
using MCM.Abstractions.Settings.Base.Global;
#endif

namespace BannerlordNepotism
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => $"{Main.Name}_v1";
        public override string DisplayName => Main.DisplayName;
        public override string FolderName => Main.Name;
        public override string FormatType => "json";

        private const string AddJoinClan_Hint = "Adds dialogue for family members to join player clan. [ Default: ON ]";

        [SettingPropertyBool("Add Join Clan Dialogue", HintText = AddJoinClan_Hint, RequireRestart = true, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("General Settings", GroupOrder = 0)]
        public bool AddJoinClan { get; set; } = true;

        private const string AddJoinKingdom_Hint = "Adds dialogue for family members who are clan leaders to join player kingdom. [ Default: ON ]";

        [SettingPropertyBool("Add Join Kingdom Dialogue", HintText = AddJoinKingdom_Hint, RequireRestart = true, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("General Settings")]
        public bool AddJoinKingdom { get; set; } = true;

        private const string AddMergeKingdoms_Hint = "Adds dialogue for family members who are rulers of kingdoms to merge into player kingdom. [ Default: ON ]";

        [SettingPropertyBool("Add Merge Kingdoms Dialogue", HintText = AddMergeKingdoms_Hint, RequireRestart = true, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("General Settings")]
        public bool AddMergeKingdoms { get; set; } = true;

        private const string DirectFamilyOnly_Hint = "Only applies dialogue to direct family members. When disabled will apply to extended relatives as well (recommended). [ Default: OFF ]";

        [SettingPropertyBool("Direct Family Only", HintText = DirectFamilyOnly_Hint, RequireRestart = false, Order = 3, IsToggle = false)]
        [SettingPropertyGroup("General Settings", GroupOrder = 0)]
        public bool DirectFamilyOnly { get; set; } = false;


        private const string RequireRelationshipToJoinClan_Hint = "Requires specified relationship level for player to be able to ask family member to join clan. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Join Clan", HintText = RequireRelationshipToJoinClan_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings", GroupOrder = 1)]
        public bool RequireRelationshipToJoinClan { get; set; } = true;


        private const string RequiredRelationshipToJoinClan_Hint = "Requires specified relationship level for player to be able to ask family member to join clan. [ Default: 30 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Join Clan", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToJoinClan_Hint, RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToJoinClan { get; set; } = 30f;


        private const string RequireRelationshipToJoinKingdom_Hint = "Requires specified relationship level for player to be able to ask family member clan to join kingdom. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Join Kingdom", HintText = RequireRelationshipToJoinKingdom_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireRelationshipToJoinKingdom { get; set; } = true;


        private const string RequiredRelationshipToJoinKingdom_Hint = "Required relationship level for player to be able to ask family member clan to join kingdom. [ Default: 60 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Join Kingdom", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToJoinKingdom_Hint, RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToJoinKingdom { get; set; } = 60f;


        private const string RequireRelationshipToMergeKingdoms_Hint = "Requires specified relationship level for player to be able to ask family member to merge kingdoms. [ Default: ON ]";

        [SettingPropertyBool("Require relationship level for Merge Kingdoms", HintText = RequireRelationshipToMergeKingdoms_Hint, RequireRestart = false, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Requirements Settings")]
        public bool RequireRelationshipToMergeKingdoms { get; set; } = true;


        private const string RequiredRelationshipToMergeKingdoms_Hint = "Required relationship level for player to be able to ask family member to merge kingdom.s [ Default: 60 ]";

        [SettingPropertyFloatingInteger("Required relationship level for Merge Kingdoms", minValue: 0f, maxValue: 100f, HintText = RequiredRelationshipToMergeKingdoms_Hint, RequireRestart = false, Order = 1)]
        [SettingPropertyGroup("Requirements Settings")]
        public float RequiredRelationshipToMergeKingdoms { get; set; } = 60f;


        private const string PatchFamilyControl_Hint = "Patches Family Control mod if found to prevent duplicate dialogs for joining player clan. [ Default: ON ]";

        [SettingPropertyBool("Patch Family Control", HintText = PatchFamilyControl_Hint, RequireRestart = true, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Patches", GroupOrder = 99)]
        public bool PatchFamilyControl { get; set; } = true;
    }
}
