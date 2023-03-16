using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace BannerlordNepotism
{
    public class FamilyControlSupportPatch : IOptionalPatch
    {
        private Assembly? familyControlAssembly = null;

        public bool TryPatch(Harmony harmony)
        {
            return true;
        }

        public bool MenusInitialised(Harmony harmony)
        {
            try
            {
                if (!Main.Settings!.PatchFamilyControl)
                {
                    return false;
                }

                familyControlAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.StartsWith("FamilyControl, "));

                if (familyControlAssembly != null)
                {
                    var FamilyControlBehaviorType = familyControlAssembly.GetType("FamilyControl.FamilyControlBehavior", false, true);
                    if (FamilyControlBehaviorType != null)
                    {
                        harmony.Patch(AccessTools.Method(FamilyControlBehaviorType, "AddJoinClanDialogs"), prefix: new HarmonyMethod(typeof(FamilyControlSupportPatch), nameof(AddJoinClanDialogs)));
                    }
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Debug.WriteDebugLineOnScreen(e.ToString());
                return false;
            }
        }

        private static bool AddJoinClanDialogs(CampaignGameStarter starter)
        {
            // Prevent adding those dialogs as Nepotism does the same with additional features
            return false;
        }
    }
}
