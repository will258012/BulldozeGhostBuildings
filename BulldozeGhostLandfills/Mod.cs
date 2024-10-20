using BulldozeGhostLandfills.Utils;
using ICities;
using System.Reflection;
namespace BulldozeGhostLandfills
{
    public class Mod : IUserMod, ILoadingExtension
    {
        public string Name => "Bulldoze Ghost Landfills v" + ModVersion;
        public string Description => "Use bulldozer to bulldoze your ghost landfills";
        private const string HarmonyId = "Will258012.BulldozeGhostLandfills";

        private string ModVersion
        {
            get
            {
                var assemblyVersion = ModAssembly.GetName().Version;
                return $"{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}";
            }
        }
        public void OnEnabled() => HarmonyPatcher.PatchOnReady(ModAssembly, HarmonyId);

        public void OnCreated(ILoading loading)
        {

        }
        public void OnLevelLoaded(LoadMode mode)
        {

        }

        public void OnLevelUnloading()
        {

        }
        public void OnReleased()
        {
        }

        public void OnDisabled() => HarmonyPatcher.TryUnpatch(HarmonyId);
        private Assembly ModAssembly => Assembly.GetExecutingAssembly();

    }
}