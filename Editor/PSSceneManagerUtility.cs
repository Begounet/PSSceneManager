using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PSSceneManager
{
    public class PSSceneManagerUtility
    {

        static public void SaveSceneAsset()
        {
            string savePath = DetectSaveDatas();

            SceneSaveAsset sceneSaveAsset = ScriptableObject.CreateInstance<SceneSaveAsset>();

            CreateRuntimeSceneLoaderIfRequired(sceneSaveAsset);

            sceneSaveAsset.SceneSetups = ConvertEditorToRuntimeSceneSetups(EditorSceneManager.GetSceneManagerSetup());

            AssetDatabase.CreateAsset(sceneSaveAsset, savePath);
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }

        public static bool LoadSceneAsset()
        {
            string savePath = DetectSaveDatas();

            SceneSaveAsset saveAsset = AssetDatabase.LoadAssetAtPath<SceneSaveAsset>(savePath);
            if (saveAsset)
            {
                RemoveDeletedScenes(ref saveAsset);

                EditorSceneManager.RestoreSceneManagerSetup(ConvertRuntimeToEditorSceneSetups(saveAsset.SceneSetups));
                return (true);
            }

            return (false);
        }

        public static string DetectSaveDatas()
        {
            Scene mainScene = EditorSceneManager.GetSceneAt(0);

            string mainSceneDirectory = Path.GetDirectoryName(mainScene.path);
            string generatedAssetPath = Path.Combine(mainSceneDirectory, GetGeneratedAssetName(mainScene.name));

            return (generatedAssetPath);
        }

        private static string GetGeneratedAssetName(string MainSceneName)
        {
            return (MainSceneName + "_SceneDatas.asset");
        }

        private static void CreateRuntimeSceneLoaderIfRequired(SceneSaveAsset SaveAsset)
        {
            Scene sceneBackup = EditorSceneManager.GetActiveScene();
            {
                RuntimeSceneLoader runtimeSceneLoader = GameObject.FindObjectOfType<RuntimeSceneLoader>();

                if (runtimeSceneLoader == null)
                {
                    GameObject runtimeSceneLoaderGo = new GameObject();
                    runtimeSceneLoaderGo.name = "RuntimeSceneLoader";
                    runtimeSceneLoader = runtimeSceneLoaderGo.AddComponent<RuntimeSceneLoader>();
                }

                if (runtimeSceneLoader.SceneDataAsset == null)
                {
                    runtimeSceneLoader.SceneDataAsset = SaveAsset;
                }
            }
            EditorSceneManager.SetActiveScene(sceneBackup);
        }

        private static RuntimeSceneSetup[] ConvertEditorToRuntimeSceneSetups(SceneSetup[] Scenes)
        {
            RuntimeSceneSetup[] runtimeSceneSetups = new RuntimeSceneSetup[Scenes.Length];

            for (int i = 0; i < runtimeSceneSetups.Length; ++i)
            {
                runtimeSceneSetups[i] = new RuntimeSceneSetup();
                runtimeSceneSetups[i].LoadEditorSceneSetup(Scenes[i]);
            }

            return (runtimeSceneSetups);
        }

        private static SceneSetup[] ConvertRuntimeToEditorSceneSetups(RuntimeSceneSetup[] RuntimeSceneSetups)
        {
            SceneSetup[] sceneSetups = new SceneSetup[RuntimeSceneSetups.Length];

            for (int i = 0; i < RuntimeSceneSetups.Length; ++i)
            {
                sceneSetups[i] = RuntimeSceneSetups[i].ConvertToSceneSetup();
            }

            return (sceneSetups);
        }

        private static void RemoveDeletedScenes(ref SceneSaveAsset SaveAsset)
        {
            SaveAsset.SceneSetups = Array.FindAll<RuntimeSceneSetup>(SaveAsset.SceneSetups, (RuntimeSceneSetup InSceneSetup) =>
            {
                string scenePath = InSceneSetup.path;
                if (string.IsNullOrEmpty(scenePath))
                {
                    return (false);
                }

                string fullPath = Path.GetFullPath(scenePath);
                return (File.Exists(fullPath));
            });
        }

    }
}