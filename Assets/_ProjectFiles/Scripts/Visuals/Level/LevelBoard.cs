using Egsp.Core;
using Egsp.Core.Ui;
using Game.Levels;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Visuals.Level
{
    /// <summary>
    /// Компонент отрисовки иконок уровней на панели.
    /// </summary>
    public class LevelBoard : Visual
    {
        [SerializeField] private LevelIcon iconPrefab;
        [SerializeField] private IContainer container;
        // UnityEvent с передачей LevelInfo.
        [SerializeField] private LevelInfoEvent onLevelChoosedEvent;

        /// <summary>
        /// Событие срабатывает при выборе уровня игроком.
        /// </summary>
        public WeakEvent<LevelInfo> LevelChoosed { get; set; } = new WeakEvent<LevelInfo>();


        private void Awake()
        {
            container = GetComponentInChildren<IContainer>();
        }

        /// <summary>
        /// Перерисовывает всю панель.
        /// </summary>
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

        // Уровень считается выбранным при нажатии на иконку.
        private void OnIconClicked(LevelIcon x)
        {
            LevelChoosed.Raise(x.LevelInfo);
            // Вызов события, которое связано с интерфейсом через UnityEvent.
            onLevelChoosedEvent.Invoke(x.LevelInfo);
        }
    }
}