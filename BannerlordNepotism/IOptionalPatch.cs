using HarmonyLib;

namespace BannerlordNepotism
{
    public interface IOptionalPatch
    {
        public bool TryPatch(Harmony harmony);

        public bool MenusInitialised(Harmony harmony);
    }
}
