using System.IO;
using System.Linq;
using Egsp.Extensions;
using UnityEditor;
using UnityEngine.SceneManagement;

public partial class LevelInfoAssetManager
{
    [MenuItem("Tools/Game/Scenes/AddSceneToBuild")]
    public static void AddSceneToBuild()
    {
        var scene = SceneManager.GetActiveScene();

        bool exist = false;
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var buildScene = EditorBuildSettings.scenes[i];

            if (buildScene.path == scene.path)
            {
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            var original = EditorBuildSettings.scenes;

            var list = original.ToList();
            list.Add(new EditorBuildSettingsScene(scene.path, true));
            
            EditorBuildSettings.scenes = list.ToArray();
        }
    }

    [MenuItem("Tools/Game/Scenes/ConvertToLevel")]
    // Премещает сцену в нужную папку, переименовывает ее и создает мета-данные.
    public static void ConvertToLevel()
    {
        var scene = SceneManager.GetActiveScene();

        if (IsLevel(scene.name))
            return;

        var newName = scene.name + "_lvl";
        
        if (!InCorrectDirectory(scene.path))
        {
            var newPath = LevelsDirectoryPath + "/" + newName + ".unity";
            AssetDatabase.MoveAsset(scene.path, newPath);
        }
        else
        {
            AssetDatabase.RenameAsset(scene.path, newName);
        }

        CreateLevelMetaData(scene);
    }

    [MenuItem("Tools/Game/Scenes/GenerateScenesMetaData")]
    public static void GenerateScenesMetaData()
    {
        var directoryInfo = new DirectoryInfo(LevelsDirectoryPath);
        var fileInfos = directoryInfo.GetFiles("*lvl*.unity");
        
        for (var i = 0; i < fileInfos.Length; i++)
        {
            CreateLevelMetaData(fileInfos[i].GetNameOnly(), i);
        }
    }
}