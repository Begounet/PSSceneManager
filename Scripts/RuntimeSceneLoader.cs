using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSSceneManager
{

    public class RuntimeSceneLoader : MonoBehaviour
    {
        public SceneSaveAsset SceneDataAsset;

        private void Awake()
        {
            if (!Application.isEditor)
            {
                foreach (RuntimeSceneSetup rss in SceneDataAsset.SceneSetups)
                {
                    if (rss.path != SceneManager.GetActiveScene().path)
                    {
                        SceneManager.LoadScene(rss.path, LoadSceneMode.Additive);
                    }
                }
            }
        }
    }

}