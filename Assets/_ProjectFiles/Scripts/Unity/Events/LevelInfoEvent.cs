using System;
using Game.Levels;
using UnityEngine.Events;

namespace Game.Visuals.Level
{
    [Serializable]
    public class LevelInfoEvent : UnityEvent<LevelInfo>
    {
    }
}