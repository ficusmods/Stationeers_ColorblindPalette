using BepInEx;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Assets.Scripts.Objects;
using System.IO;
using System.Linq;
using Assets.Scripts.Serialization;
using BepInEx.Logging;

namespace ColorblindPalette;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ColorblindConfig config;
    public static Dictionary<string, Texture2D> colorTextures = new Dictionary<string, Texture2D>();
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        if (!LoadConfig()) return;
        CreateTextures();
        Prefab.OnPrefabsLoaded += Prefab_OnPrefabsLoaded;
    }

    private bool LoadConfig()
    {
        var fpath = Path.Combine(Path.GetDirectoryName(Info.Location), "config.xml");
        if(!(XmlSerialization.Deserialize(new XmlSerializer(typeof(ColorblindConfig)), fpath) is ColorblindConfig dsconfig))
        {
            Logger.LogError("Failed to deserialize config");
            return false;
        }
        Plugin.config = dsconfig;
        return true;
    }

    private void CreateTextures()
    {
        var basePath = Path.Combine(Path.GetDirectoryName(Info.Location), "textures");
        foreach(var swap in config.ColorSwaps)
        {
            var texPath = Path.Combine(basePath, swap.TextureNew + ".png");
            if(!File.Exists(texPath))
            {
                Logger.LogError($"{texPath} missing.");
                continue;
            }

            Texture2D tex = null;

            using (FileStream fs = File.OpenRead(texPath))
            {
                if(fs.Length > int.MaxValue)
                {
                    Logger.LogError($"{swap.TextureNew} is too large.");
                    continue;
                }

                byte[] imgRaw = new byte[fs.Length];
                if(!(fs.Read(imgRaw, 0, (int)fs.Length) > 0))
                {
                    Logger.LogError($"{swap.TextureNew} image contains no data.");
                    continue;
                }
                tex = new Texture2D(2, 2);
                tex.LoadImage(imgRaw);
                tex.name = swap.TextureNew;
            }

            if (tex == null) continue;
            
            colorTextures.Add(swap.ColorNameOriginal, tex);
        }
    }

    private void GlobalMaterialSwap()
    {
        var allMats = Resources.FindObjectsOfTypeAll<Material>();
        foreach (var mat in allMats)
        {
            var matName = mat.name;
            if (!matName.StartsWith("Color")) continue;
            foreach (var swap in config.ColorSwaps.Where((swap) => matName.EndsWith(swap.ColorNameOriginal) || matName.EndsWith(swap.ColorNameOriginal + "Emissive")))
            {
                var tex = colorTextures[swap.ColorNameOriginal];
                Logger.LogInfo($"{mat.name}:{mat.mainTexture.name} texture replaced with {tex.name}");
                mat.mainTexture = tex;

                if (matName.EndsWith("Emissive"))
                {
                    Color newColor = new Color(swap.EmissionColor.R, swap.EmissionColor.G, swap.EmissionColor.B, 1.0f);
                    mat.SetColor("_EmissionColor", newColor * swap.EmissionIntensity);
                    mat.SetTexture("_EmissionMap", tex);
                    mat.EnableKeyword("_EMISSION");
                    Logger.LogInfo($"{mat.name} emission map, color replaced.");
                }
            }
        }
    }

    private void StructureMaterialSwap()
    {
        Logger.LogError("Unsupported. Generic binding utilities not available in this Unity version.");
        return;
        // TODO revisit if game engine gets updated.
        // Game uses animation clips to change materials on structures depending on the interactable state.
        // These bindings are not editable at runtime in this unity version.
        // Changing it with animation events doesn't seem to work and the material gets overridden by the binding.
        foreach (Structure structure in Prefab.AllPrefabs.Where((p) => p is Structure))
        {
            var name = structure.name.Replace("Structure", "");
            if (!config.StructureFilter.Contains(name)) continue;

            foreach (var clip in structure.BaseAnimator.runtimeAnimatorController.animationClips)
            {
                // TODO Edit bindings here
            }
        }
    }

    private void Prefab_OnPrefabsLoaded()
    {
        Logger.LogInfo("Prefabs loaded");
        if(config.StructureFilter.Count > 0)
        {
            Logger.LogInfo("Using filter mode with structures:");
            string msg = config.StructureFilter[0];
            for (int i = 1; i < config.StructureFilter.Count; i++)
            {
                var filter = config.StructureFilter[i];
                msg += ", " + filter;
            }
            Logger.LogInfo(msg);
            StructureMaterialSwap();
        }
        else
        {
            Logger.LogInfo("Using global mode");
            GlobalMaterialSwap();
        }
    }

}
