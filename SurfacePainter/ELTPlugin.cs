﻿using System.Collections.Generic;
using ColossalFramework.UI;
using ICities;
using NaturalResourcesBrush.API;
using NaturalResourcesBrush.Utils;
using RedirectionFramework;
using SurfacePainter.Detours;
using UnityEngine;

namespace SurfacePainter
{
    public class EltPlugin : IEltPlugin
    {
        public void Initialize()
        {
            Util.AddLocale("TUTORIAL_ADVISER", "Surface", "Surface Tool", "");
            Redirector<SurfacePanelDetour>.Deploy();
            Redirector<TerrainManagerDetour>.Deploy();
        }

        public void Dispose()
        {
            RevertDetours();
        }

        public static void RevertDetours()
        {
            Redirector<SurfacePanelDetour>.Revert();
            SurfacePanelDetour.Dispose();
            Redirector<TerrainManagerDetour>.Revert();
        }

        public IEnumerable<ToolBase> SetupTools(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                return new ToolBase[] { };
            }
            var surfaceBrushTool = ToolsModifierControl.GetTool<InGameSurfaceTool>();
            if (surfaceBrushTool != null)
            {
                return new ToolBase[] { };
            }
            surfaceBrushTool = ToolsModifierControl.toolController.gameObject.AddComponent<InGameSurfaceTool>();
            return new ToolBase[] { surfaceBrushTool };
        }

        public void CreateToolbars(LoadMode mode)
        {
            if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            {
                return;
            }
            var uiTabstrip = ToolsModifierControl.mainToolbar.component as UITabstrip;
            if (uiTabstrip == null)
            {
                return;
            }
            var mainToolbar = ToolsModifierControl.mainToolbar;
            if (mainToolbar == null)
            {
                return;
            }
            ToolbarButtonSpawner.SpawnSubEntry(uiTabstrip, "Surface", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true, mainToolbar.m_OptionsBar, mainToolbar.m_DefaultInfoTooltipAtlas);


            var SurfacePanel = UIView.FindObjectOfType<SurfacePanel>();
            var buttons = SurfacePanel.GetComponentsInChildren<UIButton>();
            foreach (var button in buttons)
            {
                if (button.name == "PavementB")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "SurfacePavementB" });
                }
                if (button.name == "Gravel")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "SurfaceGravel" });
                }
                if (button.name == "Field")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "SurfaceField" });
                }
                if (button.name == "Clip")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "SurfaceClip" });
                }
                if (button.name == "Ruined")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "SurfaceRuined" });
                }
            }

            var GameMainToolbar = UIView.FindObjectOfType<GameMainToolbar>();
            var Gamebuttons = GameMainToolbar.GetComponentsInChildren<UIButton>();
            foreach (var button in Gamebuttons)
            {
                if (button.name == "Surface")
                {
                    button.atlas = Util.CreateAtlasFromResources(new List<string> { "ToolbarIconSurface", "ToolbarIconBase" });
                }
            }

        }

        public bool SupportsSingle(ToolBase tool)
        {
            return tool is InGameSurfaceTool;
        }

        public void SetSize(float size, bool minSliderValue)
        {
            var surfaceTool = ToolsModifierControl.GetCurrentTool<InGameSurfaceTool>();
            if (surfaceTool != null)
            {
                surfaceTool.m_brushSize = size;
                if (minSliderValue)
                {
                    surfaceTool.m_mode = InGameSurfaceTool.Mode.Single;
                }
                else
                {
                    surfaceTool.m_mode = InGameSurfaceTool.Mode.Brush;
                }
            }
        }

        public void SetStrength(float strength)
        {
        }

        public void SetBrush(Texture2D brush)
        {
            var surfaceTool = ToolsModifierControl.GetCurrentTool<InGameSurfaceTool>();
            if (surfaceTool != null)
            {
                surfaceTool.m_brush = brush;
            }
        }
    }
}