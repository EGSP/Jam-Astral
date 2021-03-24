using System.Linq;
using Egsp.Core;

namespace Game.Levels
{
    public partial struct LevelInfo
    {
        public string LevelName;
        public bool Completed;

        public bool IsDefault => string.IsNullOrWhiteSpace(LevelName);

        public LevelInfo(string levelName)
        {
            LevelName = levelName;
            Completed = false;
        }

        public override bool Equals(object obj)
        {
            if (obj is LevelInfo levelInfo)
            {
                return LevelName == levelInfo.LevelName;
            }
            return false;
        }
    }
}