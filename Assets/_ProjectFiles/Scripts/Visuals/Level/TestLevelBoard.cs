using System;
using Game.Levels;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Visuals.Level
{
    [Obsolete]
    public class TestLevelBoard : SerializedMonoBehaviour
    {
        [SerializeField] private LevelBoard levelBoard;

        private void Awake()
        {
            levelBoard.LevelChoosed.Subscribe(OnLevelChoosed);
        }

        private void OnLevelChoosed(LevelInfo x)
        {
            Debug.Log(" choosed.");
        }

        [Button]
        public void Rebuild()
        {
            levelBoard.Rebuild();
        }

        [Button]
        public void Hide()
        {
            levelBoard.Disable();
        }

        [Button]
        public void Show()
        {
            levelBoard.Enable();
        }
    }
}