using System.Collections.Generic;

using TaleWorlds.SaveSystem;

namespace BannerlordNepotism
{
    internal sealed class CustomSaveableTypeDefiner : SaveableTypeDefiner
    {
        public const int SaveBaseId_b0tlanner0 = 300_711_200;
        public const int SaveBaseId = SaveBaseId_b0tlanner0 + 14;

        public CustomSaveableTypeDefiner() : base(SaveBaseId) { }

        protected override void DefineClassTypes()
        {
            base.DefineClassTypes();

            base.AddClassDefinition(typeof(PlayerAsKingSelectionKingdomDecision), 1257, null);
        }

        protected override void DefineContainerDefinitions()
        {
            base.DefineContainerDefinitions();
        }
    }
}
