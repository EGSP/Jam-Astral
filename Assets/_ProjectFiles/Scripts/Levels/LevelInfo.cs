using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
using Egsp.Extensions.Collections;
using Egsp.Files;

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
            public abstract LinkedList<LevelInfo> LoadLevels();

            public class LevelsFromDictionary : LevelsSource
            {
                private readonly string _dictionary;

                public LevelsFromDictionary(string dictionary)
                {
                    _dictionary = dictionary;
                }
                
                public override LinkedList<LevelInfo> LoadLevels()
                {
                    var linkedList = Storage.Global.LoadObjectsFromDirectory<LevelInfo>("Levels");

                    return linkedList;
                }
            }
        }
        
        public static void LoadLevelInfos(LevelsSource source, LoadMode loadMode = LoadMode.Overwrite)
        {
            var linkedList = source.LoadLevels();

            if (loadMode == LoadMode.Add)
                LevelInfos.Join(linkedList);
            else
                LevelInfos = linkedList;
        }

        public static void ReloadLevelInfos(bool forceReload = false)
        {
            if (AlreadyLoaded && !forceReload)
                return;
            
            LoadLevelInfos(new LevelsSource.LevelsFromDictionary("Levels"));

            AlreadyLoaded = true;
        }

        public static void SaveLevelInfos(IEnumerable<LevelInfo> levelInfos)
        {
            Storage.Global.SaveObjects("levelInfos", levelInfos);
        }
    }
}