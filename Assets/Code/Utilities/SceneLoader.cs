using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class SceneLoader
    {
        #region Static Fields

        public static bool loading_scene = false;

        #endregion


        #region Scene Loaders

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

        #endregion

    }
}