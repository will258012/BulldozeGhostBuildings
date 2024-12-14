using ColossalFramework;
using HarmonyLib;

namespace BulldozeGhostLandfills.Patches
{
    [HarmonyPatch]
    public static class BulldozePatch
    {
        [HarmonyPatch(typeof(LandfillSiteAI), nameof(LandfillSiteAI.CheckBulldozing))]
        [HarmonyPatch(typeof(CemeteryAI), nameof(CemeteryAI.CheckBulldozing))]
        [HarmonyPatch(typeof(SnowDumpAI),nameof(SnowDumpAI.CheckBulldozing))]
        public static void Postfix(ref ToolBase.ToolErrors __result, ref Building data)
        {
            // The original method only checks if the building is empty.
            // This modification accounts for the special case of building, which don't actually exist.
            if (!data.m_flags.IsFlagSet(Building.Flags.Created))
            {
                // Modify the return value to bypass restrictions for ghost landfills.
                __result = ToolBase.ToolErrors.None;
            }
        }
    }
}
