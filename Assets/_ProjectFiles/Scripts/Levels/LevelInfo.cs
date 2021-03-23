using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Egsp.Core;
using Egsp.Extensions.Collections;
using Egsp.Files;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Levels
{
    public struct LevelInfo
    {
        public string LevelName;
        public bool Completed;

        public bool IsDefault => string.IsNullOrWhiteSpace(LevelName);

        public LevelInfo(string levelName)
        {
            LevelName = levelName;
            Completed = false;
        }

        //------------- STATIC

        private static LinkedList<LevelInfo> _levelInfos;
        public static LinkedList<LevelInfo> LevelInfos
        {
            get => _levelInfos;
            private set => _levelInfos = value;
        }
        

        private static bool AlreadyLoaded { get; set; }
        
        static LevelInfo()
        {
            LevelInfos = new LinkedList<LevelInfo>();
            ReloadLevelInfos();
        }

        public static LevelInfo GetCurrentLevel()
        {
            var scene = GameSceneManager.GetActiveScene();
            var levelInfo = LevelInfos.FirstOrDefault(x => x.LevelName == scene.name);
            if (levelInfo.IsDefault)
                return new LevelInfo("default");
            return levelInfo;
        }

        public enum LoadMode
        {
            Add,
            Overwrite
        }

        public abstract class LevelsSource
        {
            public abstract Task<LinkedList<LevelInfo>> LoadLevels();

            public class LevelsFromDictionary : LevelsSource
            {
                private readonly string _directory;

                public LevelsFromDictionary(string directory)
                {
                    _directory = directory;
                }
                
                public override async Task<LinkedList<LevelInfo>> LoadLevels()
                {
                    var linkedList = Storage.Global.LoadObjectsFromDirectory<LevelInfo>(_directory);
                    return linkedList;
                }
            }

            public class LevelsFromAddressables : LevelsSource
            {
                private LinkedList<LevelInfo> _levelInfos = new LinkedList<LevelInfo>();
                
                public override async Task<LinkedList<LevelInfo>> LoadLevels()
                {
                    var ao =
                        Addressables.LoadAssetsAsync<TextAsset>("level_meta", OnTextLoaded);

                    await ao.Task;

                    return _levelInfos;
                }

                public void OnTextLoaded(TextAsset asset)
                {
                    var levelInfo = Storage.DefaultSerializer.Deserialize<LevelInfo>(asset.bytes);

                    if (levelInfo.IsSome)
                        _levelInfos.AddLast(levelInfo.Value);
                }
            }
        }
        
        public static void LoadLevelInfosToExisting(LevelsSource source, LoadMode loadMode = LoadMode.Overwrite)
        {
            var linkedList = LoadLevelInfos(source);

            if (loadMode == LoadMode.Add)
                LevelInfos.Join(linkedList);
            else
                LevelInfos = linkedList;
        }

        public static LinkedList<LevelInfo> LoadLevelInfos(LevelsSource source)
        {
            var task = source.LoadLevels();
            return task;
        }

        public static void ReloadLevelInfos(bool forceReload = false)
        {
            if (AlreadyLoaded && !forceReload)
                return;
            
            LoadLevelInfosToExisting(new LevelsSource.LevelsFromDictionary("Levels"));

            AlreadyLoaded = true;
        }

        public static void SaveLevelInfos(IEnumerable<LevelInfo> levelInfos)
        {
            Storage.Global.SaveObjectsByFiles("Levels", levelInfos,
                x => x.LevelName);
        }
    }
}