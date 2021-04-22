using System;
using System.Linq;
using Egsp.Core;

namespace Game.Levels
{
    // Здесь описаны данные одного экземпляра.
    public partial struct LevelInfo
    {
        public string LevelName;
        public bool Completed;

        /// <summary>
        /// Это значение нужно только для упорядочивания загрузки уровней.
        /// </summary>
        public readonly int OrderId;

        public bool IsDefault => string.IsNullOrWhiteSpace(LevelName);

        public LevelInfo(string levelName)
        {
            LevelName = levelName;
            Completed = false;
            
            OrderId = 0;
        }

        public LevelInfo(string levelName, int orderId)
        {
            LevelName = levelName;
            Completed = false;
            
            OrderId = orderId;
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