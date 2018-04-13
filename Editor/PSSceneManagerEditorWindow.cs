using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace PSSceneManager
{

    public class PSSceneManagerEditorWindow : EditorWindow
    {
        public static PSSceneManagerEditorWindow Instance { get; private set; }

        Vector2 SceneListScrollViewPosition;
        string CurrentMainScene;
        public string Status;

        [MenuItem("Window/PS Scene Manager")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            PSSceneManagerEditorWindow window = (PSSceneManagerEditorWindow)EditorWindow.GetWindow(typeof(PSSceneManagerEditorWindow));
            window.Show();
            Instance = window;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Loaded scenes");

                if (!Application.isPlaying)
                {
                    SceneListScrollViewPosition = GUILayout.BeginScrollView(SceneListScrollViewPosition);
                    {
                        SceneSetup[] sceneSetups = EditorSceneManager.GetSceneManagerSetup();
                        for (int i = 0; i < sceneSetups.Length; ++i)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Space(20);
                                GUILayout.Label(sceneSetups[i].path);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();
                }
                else
                {
                    GUILayout.Label("Not available during Play");
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save scenes"))
                {
                    PSSceneManagerUtility.SaveSceneAsset();
                    Status = "Scenes saved !";
                }

                if (GUILayout.Button("Restore scenes"))
                {
                    if (PSSceneManagerUtility.LoadSceneAsset())
                    {
                        Status = "Load success ! ";
                    }
                    else
                    {
                        Status = "Cannot load asset : " + PSSceneManagerUtility.DetectSaveDatas();
                    }
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.Label(Status);
        }
    }

}