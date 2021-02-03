using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HiddenGO
{
    public class HiddenGO : EditorWindow
    {
        [MenuItem("Tools/Hidden GO")]
        private static void ShowWindow()
        {
            var window = GetWindow<HiddenGO>();
            window.titleContent = new GUIContent("Hidden GO");
            window.Show();
        }

        private void OnGUI()
        {
            var selection = Selection.activeGameObject;
            if (selection)
            {
                if ((selection.hideFlags & HideFlags.HideInHierarchy) != 0)
                {
                    if (GUILayout.Button($"Show {selection.name}"))
                    {
                        selection.hideFlags &= ~HideFlags.HideInHierarchy;
                        EditorApplication.DirtyHierarchyWindowSorting();
                    }
                }
                else if (GUILayout.Button($"Hide {selection.name}"))
                {
                    selection.hideFlags |= HideFlags.HideInHierarchy;
                    EditorApplication.DirtyHierarchyWindowSorting();
                }
            }
            else
                EditorGUILayout.HelpBox("Select a GameObject to toggle it", MessageType.Info, true);

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                EditorGUILayout.LabelField("Hidden objects in " + scene.path);

                var rootGameObjects = scene.GetRootGameObjects();
                foreach (var root in rootGameObjects)
                    DrawHidden(root.transform);
            }
        }

        private static void DrawHidden(Transform root)
        {
            if ((root.hideFlags & HideFlags.HideInHierarchy) != 0)
            {
                if (GUILayout.Button($"{root.name}"))
                    Selection.activeGameObject = root.gameObject;
            }

            EditorGUI.indentLevel++;
            for (var i = 0; i < root.childCount; i++)
            {
                var transform = root.GetChild(i);
                DrawHidden(transform);
            }

            EditorGUI.indentLevel--;
        }
    }
}