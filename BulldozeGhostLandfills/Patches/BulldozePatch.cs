using ColossalFramework;
using HarmonyLib;

namespace BulldozeGhostBuildings.Patches
{
    [HarmonyPatch]
    public static class BulldozePatch
    {
        [HarmonyPatch(typeof(LandfillSiteAI), nameof(LandfillSiteAI.CheckBulldozing))]
        [HarmonyPostfix]
        public static void CheckBulldozingPatch1(ref ToolBase.ToolErrors __result, ref Building data)
            => CheckBulldozingPatch(ref __result, ref data);

        [HarmonyPatch(typeof(CemeteryAI), nameof(CemeteryAI.CheckBulldozing))]
        [HarmonyPostfix]
        public static void CheckBulldozingPatch2(ref ToolBase.ToolErrors __result, ref Building data)
            => CheckBulldozingPatch(ref __result, ref data);

        [HarmonyPatch(typeof(SnowDumpAI), nameof(SnowDumpAI.CheckBulldozing))]
        public static void CheckBulldozingPatch(ref ToolBase.ToolErrors __result, ref Building data)
        {
            // The original method only checks if the building is empty.
            // This modification accounts for the special case of building, which don't actually exist.
            if (!data.m_flags.IsFlagSet(Building.Flags.Created))
            {
                // Modify the return value to bypass restrictions for ghost landfills.
                __result = ToolBase.ToolErrors.None;
            }
        }

        [HarmonyPatch(typeof(BuildingManager), nameof(BuildingManager.ReleaseBuilding))]
        [HarmonyPostfix]
        public static void ReleaseBuildingPatch(ref BuildingManager __instance, ushort building)
        {
            /*
            Game can load buildings that have citizen units, but when the building is released, the citizen units won't be reset.
            For ghost buildings, they will still exist after saving and reloading, even if they have been bulldozed,
            because they still have citizen units data.
            This manually resets citizen units to prevent the game from loading these buildings again.
            */
            __instance.m_buildings.m_buffer[building].m_citizenUnits =
            __instance.m_buildings.m_buffer[building].m_citizenCount = default;
        }
    }
}
