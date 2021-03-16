using System.Collections.Generic;
using System.Linq;
using Egsp.Core;
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

        public static List<LevelInfo> LevelInfos { get; private set; }
        
        static LevelInfo()
        {
            LevelInfos = new List<LevelInfo>();
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

        public static List<LevelInfo> ReloadLevelInfos()
        {
            var levelInfos = Storage.Global.LoadObjects<LevelInfo>("levelInfos");

            if (levelInfos.IsSome)
                return LevelInfos = levelInfos.Value;
            else
                return LevelInfos = new List<LevelInfo>();
        }

        public static void SaveLevelInfos(IEnumerable<LevelInfo> levelInfos)
        {
            Storage.Global.SaveObjects("levelInfos", levelInfos);
        }
    }
}