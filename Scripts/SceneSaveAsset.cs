using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PSSceneManager
{

    [System.Serializable]
    public class SceneSaveAsset : ScriptableObject
    {
        public RuntimeSceneSetup[] SceneSetups;
    }

}