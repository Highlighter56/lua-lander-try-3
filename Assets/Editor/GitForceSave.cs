using UnityEngine;
using UnityEditor;

public static class GitForceSave
{
    // This adds a clickable option right in your Unity top menu bar!
    [MenuItem("Git Tools/Force Git Sync")]
    public static void SyncProjectToDisk()
    {
        // 1. Force Unity to save all unsaved open scene files
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();

        // 2. Force Unity to write all tags, layers, and project settings to your hard drive
        AssetDatabase.SaveAssets();

        Debug.Log("🎨 Git Sync Complete! Your tags and settings have been written to disk. Check GitHub Desktop!");
    }
}