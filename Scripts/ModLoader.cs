using UnityEngine;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace Mod3D
{
    public sealed class ModLoader : MonoBehaviour
    {
        public static Mod mod;
        public static GameObject go;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            if (go != null)
                return;

            mod = initParams.Mod;

            Debug.Log("Started setup of : " + mod.Title);

            //start loading all assets asynchrousnly - the bool paramater tells it to unload the asset bundle since all assets are loaded
            ModManager.Instance.GetComponent<MonoBehaviour>().StartCoroutine(mod.LoadAllAssetsFromBundleAsync(true));
            //go = mod.GetAsset<GameObject>("MobilePersonAsset.prefab", true);
            mod.IsReady = true;
        }

        /// <summary>
        /// Safely expose GetAsset
        /// </summary>
        public static T GetAsset<T>(string assetName, bool clone = true) where T : Object
        {
            return mod.GetAsset<T>(assetName, clone);
        }
    }
}
