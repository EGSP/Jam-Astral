using System;
using System.IO;
using Egsp.Extensions;
using Egsp.Files;
using Game.Levels;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public partial class LevelInfoAssetManager
{
    private const string LevelsDirectoryPath = "Assets/_ProjectFiles/Scenes/Levels";
    private const string SceneNameFilter = "lvl";
    
    static LevelInfoAssetManager()
    {
        Debug.Log("Init LIAM.");
        EditorSceneManager.sceneSaved += EditorSceneManagerOnSceneSaved;
    }

    private static void EditorSceneManagerOnSceneSaved(Scene scene)
    {
        if (IsLevel(scene.name))
        {
            if (!InCorrectDirectory(scene.path))
            {
                var newPath = LevelsDirectoryPath + "/" + scene.name + ".unity";
                AssetDatabase.MoveAsset(scene.path, newPath);

                CreateLevelMetaData(scene);
            }
        }
    }
    
    private static void CreateLevelMetaData(Scene scene)
    {
        CreateLevelMetaData(scene.name);
    }
    
    private static void CreateLevelMetaData(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
            return;
        
        var levelInfo = new LevelInfo(sceneName);
        var serializedData = SerializationUtility.SerializeValue(levelInfo, DataFormat.JSON);

        var absolutePath = Path.GetFullPath($"{LevelsDirectoryPath}/Meta");
        var fs = new FileStream($"{absolutePath}/{levelInfo.LevelName}.txt",
            FileMode.Create, FileAccess.Write);

        var bw = new BinaryWriter(fs);
        bw.Write(serializedData);
        fs.Close();
    }

    private static bool IsLevel(string sceneName)
    {
        if (sceneName.Contains(SceneNameFilter, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private static bool InCorrectDirectory(string pathWithName)
    {
        var path = Path.GetDirectoryName(pathWithName);

        return FileSystemExtensions.ComparePaths(path, LevelsDirectoryPath);
    }

    private static ImmutableList<EditorBuildSettingsScene> GetAllBuildScenes()
    {
        var scenes = EditorBuildSettings.scenes.ToImmutableList();
        return scenes;
    }
}
