using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace PSSceneManager
{
    // Copy of SceneSetup that is not only for editor
    [System.Serializable]
    public class RuntimeSceneSetup
    {
        public bool isActive;
        //
        // Summary:
        //     ///
        //     If the scene is loaded.
        //     ///
        public bool isLoaded;
        //
        // Summary:
        //     ///
        //     Path of the scene. Should be relative to the project folder. Like: "AssetsMyScenesMyScene.unity".
        //     ///
        public string path;
        
#if UNITY_EDITOR

        public SceneSetup ConvertToSceneSetup()
        {
            SceneSetup ss = new SceneSetup();
            ss.isActive = isActive;
            ss.isLoaded = isLoaded;
            ss.path = path;
            return (ss);
        }

        public void LoadEditorSceneSetup(SceneSetup InSceneSetup)
        {
            isActive = InSceneSetup.isActive;
            isLoaded = InSceneSetup.isLoaded;
            path = InSceneSetup.path;
        }

#endif

    }
}