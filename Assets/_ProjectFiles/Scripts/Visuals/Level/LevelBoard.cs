using Egsp.Core;
using Egsp.Core.Ui;
using Game.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Visuals.Level
{
    public class LevelBoard : Visual
    {
        [SerializeField] private LevelIcon iconPrefab;
        [SerializeField] private IContainer container;
        [SerializeField] private LevelInfoEvent onLevelChoosedEvent;

        public WeakEvent<LevelInfo> LevelChoosed = new WeakEvent<LevelInfo>();
        

        private void Awake()
        {
            container = GetComponentInChildren<IContainer>();
        }

        public void Rebuild()
        {
            container.Clear();
            
            var levels = LevelInfo.LevelInfos;
            
            foreach (var levelInfo in levels)
            {
                var icon = container.PutPrefab(iconPrefab);

                icon.LevelInfo = levelInfo;
                icon.Clicked.Subscribe(OnIconClicked);
            }
        }

        private void OnIconClicked(LevelIcon x)
        {
            Debug.Log(x.LevelInfo.IsDefault);
            LevelChoosed.Raise(x.LevelInfo);
            onLevelChoosedEvent.Invoke(x.LevelInfo);
        }
    }
}