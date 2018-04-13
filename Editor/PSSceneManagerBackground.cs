using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PSSceneManager
{

    // Static Constructor called on editor startup
    // Will create an instance of PSSceneManagerBackground
    [InitializeOnLoad]
    public class PSSceneManagerEditorInitializer
    {
        static PSSceneManagerEditorInitializer()
        {
            new PSSceneManagerBackground();
        }
    }

    // Class watching for scene load
    public class PSSceneManagerBackground
    {
        private string CurrentMainScene = null;

        const string IsInSessionKey = "PSSceneManagerEditor_IsInSession";
        bool HasSaveForCompilation = false;

        public PSSceneManagerBackground()
        {   
            if (EditorPrefs.HasKey(IsInSessionKey))
            {
                // Debug.Log("Clear compilation flag");
                EditorPrefs.DeleteKey(IsInSessionKey);
                CurrentMainScene = EditorSceneManager.GetSceneAt(0).path;
            }
             
            //Add Update Method to be called each editor frame (even if not in play mode)
            EditorApplication.update += Update;
            EditorApplication.playmodeStateChanged += HandePlayModeStateChanged;
        }

        void HandePlayModeStateChanged()
        {
            // Manage when the editor change its Play mode
            EditorPrefs.SetBool(IsInSessionKey, true);
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                string currentScenePath = EditorSceneManager.GetSceneAt(0).path;
                if (CurrentMainScene != currentScenePath)
                {
                    CurrentMainScene = currentScenePath;
                    OnSceneChanged();
                }
            }
             
            // Prepare for compilation
            if (EditorApplication.isCompiling && !HasSaveForCompilation)
            {
                // Debug.Log("Set flag for compilation");
                HasSaveForCompilation = true; // Will be reset to false only when the instance will be recreated. Use a variable instead of checking with HasKey during the compilation
                EditorPrefs.SetBool(IsInSessionKey, true);
            }
        }

        void OnSceneChanged()
        {
            // Get EditorWindow if present in editor to display error message
            PSSceneManagerEditorWindow window = PSSceneManagerEditorWindow.Instance;
            string errorMessage = "";

            if (!PSSceneManagerUtility.LoadSceneAsset())
            {
                errorMessage = "No data to load";
            }

            if (window != null)
            {
                window.Status = errorMessage;
            }
        }
    }
}