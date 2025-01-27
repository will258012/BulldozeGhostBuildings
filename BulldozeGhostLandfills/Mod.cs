using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections;
using WillCommons;
namespace BulldozeGhostBuildings
{
    public class Mod : PatcherModBase
    {
        public override string BaseName => "Bulldoze Ghost Buildings";
        public override string Description => "Demolish your ghost buildings more easily";
        public override string HarmonyID => "Will258012.BulldozeGhostBuildings";
        public void OnSettingsUI(UIHelper helper)
        {
            bulldozeButton = helper.AddButton("Find and bulldoze all ghost buildings in city", FindAndBulldozeGhostBuildings) as UIButton;
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
                var buildingsToBulldoze = new FastList<ushort>();

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

                    var infoClass = BuildingManager.instance.m_buildings.m_buffer[building].Info.m_class;
                    BuildingManager.instance.RemoveServiceBuilding(building, infoClass.m_service);
                    yield return null;
                }

                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                BaseName,
                buildingsToBulldoze.m_size > 0 ?
                $"Successfully bulldozed {buildingsToBulldoze.m_size} ghost building(s)!"
                : "No ghost buildings found!",
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
            if (!HasPatched)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                BaseName,
                @"This mod won't work because the Harmony patches could not be applied successfully :(
Please check the Harmony mod status and try again, or report us with your output_log.txt file.",
                true);
                if (bulldozeButton != null)
                {
                    bulldozeButton.tooltip = "Disable due to mod error";
                }
            }
            else
            {
                if (bulldozeButton != null)
                {
                    bulldozeButton.isEnabled = true;
                    bulldozeButton.tooltip = null;
                }
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