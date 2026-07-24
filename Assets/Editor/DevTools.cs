using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class DevTools
{
    [MenuItem("Dev Tools/Force Save Project %#&g")]
    public static void ForceSaveProject()
    {
        SaveProjectState();

        Debug.Log("Dev Tools complete:\n- Saved open scenes\n- Saved assets\n- Refreshed Asset Database\nReady for Git.");
    }

    [MenuItem("Dev Tools/Restart Editor %#&r")]
    public static void RestartEditor()
    {
        SaveProjectState();

        string projectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string editorPath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

        if (string.IsNullOrEmpty(editorPath))
        {
            Debug.LogError("Dev Tools could not find the Unity Editor executable, so the restart was cancelled.");
            return;
        }

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = editorPath,
                Arguments = $"-projectPath \"{projectPath}\"",
                WorkingDirectory = projectPath,
                UseShellExecute = false
            });

            Debug.Log($"Dev Tools launched a new Unity Editor instance for {projectPath}. Closing the current editor now.");
            EditorApplication.delayCall += () => EditorApplication.Exit(0);
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"Dev Tools could not restart Unity: {exception.Message}");
        }
    }

    private static void SaveProjectState()
    {
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}