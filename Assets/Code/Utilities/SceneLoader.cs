using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class SceneLoader
    {
        #region Scene Loaders

        public static IEnumerator LoadScene(string scene)
        {
            AsyncOperation async_load = SceneManager.LoadSceneAsync(scene);

            // Wait until the asynchronous scene fully loads
            while (!async_load.isDone)
            {
                yield return null;
            }
        }

        #endregion

    }
}