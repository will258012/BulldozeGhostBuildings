using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections;
using System.Collections.Generic;
using WillCommons;
namespace BulldozeGhostLandfills
{
    public class Mod : PatcherModBase
    {
        public override string BaseName => "Bulldoze Ghost Landfills";
        public override string Description => "Use bulldozer to bulldoze your ghost landfills";
        public override string HarmonyID => "Will258012.BulldozeGhostLandfills";
        public void OnSettingsUI(UIHelper helper)
        {
            bulldozeButton = helper.AddButton("Find and bulldoze all ghost buildings in city", () => FindAndBulldozeGhostBuildings()) as UIButton;
            bulldozeButton.playAudioEvents = true;
            if (!isLoaded)
            {
                bulldozeButton.isEnabled = false;
                bulldozeButton.tooltip = "Please use this in game!";
            }
            else
            {
                bulldozeButton.isEnabled = true;
                bulldozeButton.tooltip = null;
            }
        }

        private void FindAndBulldozeGhostBuildings()
        {
            if (!isLoaded)
            {
                return;
            }
            try
            {
                var buildingsToBulldoze = new List<ushort>();

                for (ushort i = 0; i < BuildingManager.instance.m_buildings.m_buffer.Length; i++)
                {
                    var building = BuildingManager.instance.m_buildings.m_buffer[i];
                    if (building.m_buildIndex != default &&
                     building.m_flags != Building.Flags.None &&
                    !building.m_flags.IsFlagSet(Building.Flags.Deleted) &&
                    !building.m_flags.IsFlagSet(Building.Flags.Created))
                        buildingsToBulldoze.Add(i);
                }
                
                foreach (var building in buildingsToBulldoze)
                {
                    SimulationManager.instance.AddAction(ReleaseBuilding(building));
                }

                IEnumerator ReleaseBuilding(ushort building)
                {
                    BuildingManager.instance.ReleaseBuilding(building);
                    yield return null;
                }

                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                BaseName,
                buildingsToBulldoze.Count > 0 ? $"Successfully found and bulldozed {buildingsToBulldoze.Count} ghost building(s)!" : "No ghost buildings found!",
                false);
            }
            catch (Exception e)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                BaseName,
                "Something went wrong: \n" + e.ToString(),
                true);
                Logging.Exception(e);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            isLoaded = true;
            if (bulldozeButton != null)
            {
                bulldozeButton.isEnabled = true;
                bulldozeButton.tooltip = null;
            }
        }

        public override void OnLevelUnloading()
        {
            isLoaded = false;
            if (bulldozeButton != null)
            {
                bulldozeButton.isEnabled = false;
                bulldozeButton.tooltip = "Please use this in game!";
            }
        }

        private bool isLoaded = false;
        private UIButton bulldozeButton;
    }

}