using System;
using Game.Visuals.Level;
using UnityEngine;

namespace Game.Levels
{
    public class LevelCompleteHandler : MonoBehaviour
    {
        [SerializeField] private LevelInfoEvent onLevelCompleted;
        [SerializeField] private LevelInfoEvent onLevelRecompleted;

        private void Awake()
        {
            LevelInfo.OnLevelComplete.Subscribe(HandleComplete);
            LevelInfo.OnLevelRecomplete.Subscribe(HandleRecomplete);
        }

        private void HandleComplete((LevelInfo, LevelCompleteType) tuple)
        {
            if (tuple.Item2 == LevelCompleteType.Ingame)
            {
                onLevelCompleted.Invoke(tuple.Item1);
            }
            else
            {
                Debug.Log("Игровой уровень завершен не игровым способом.");
            }
        }

        private void HandleRecomplete(LevelInfo levelInfo)
        {
            onLevelRecompleted.Invoke(levelInfo);
        }
    }
}