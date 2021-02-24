using System.Collections;
using Battle;
using Eventing;
using Mapping;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        #region Static Fields

        public static bool loading_scene = false;
        public static bool initial_overworld_load = false;

        #endregion


        #region Scene Loaders

        // Generic scene loader
        public static IEnumerator LoadScene(string scene)
        {
            loading_scene = true;
            AsyncOperation async_load = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!async_load.isDone)
            {
                yield return null;
            }
            loading_scene = false;
        }

        // Scene loader for use when FIRST loading into overworld
        public static IEnumerator InitialLoadOverworldScene()
        {
            loading_scene = true;
            initial_overworld_load = true;
            AsyncOperation async_load = SceneManager.LoadSceneAsync(Constants.OVERWORLD_SCENE);

            // Wait until the asynchronous scene fully loads
            while (!async_load.isDone)
            {
                yield return null;
            }

            loading_scene = false;
            initial_overworld_load = false;
        }

        public static IEnumerator LoadOverworldScene()
        {
            // TODO
            yield return null;
        }

        public static IEnumerator LoadBattleScene()
        {
            // TODO
            yield return null;
        }

        #endregion

    }
}