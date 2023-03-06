using BepInEx.Configuration;
using UnityEngine;

namespace EnhancementsAndTweaks
{
    internal interface IMod {
        public string GetName();

        public string GetDescription();

        public bool PreInstall(ConfigFile config, AssetBundle assets);
    }
}
