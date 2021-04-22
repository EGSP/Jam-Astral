using Egsp.Core;
using Egsp.Core.Ui;
using Game.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Visuals.Level
{
    /// <summary>
    /// Кликабельная иконка, которая содержит базовую информацию об уровне.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class LevelIcon : Visual, IPointerDownHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        private LevelInfo _levelInfo;

        public WeakEvent<LevelIcon> Clicked { get; private set; } = new WeakEvent<LevelIcon>();

        /// <summary>
        /// Переданный уровень.
        /// </summary>
        public LevelInfo LevelInfo
        {
            get => _levelInfo;
            set
            {
                _levelInfo = value;
                Completed = _levelInfo.Completed;
                Name = _levelInfo.LevelName;
            }
        }
        
        private bool Completed
        {
            get => _levelInfo.Completed; 
            set
            {
                if(value)
                    image.color = Color.green;
                else
                    image.color = Color.yellow;
            }
        }

        private string Name
        {
            get => _levelInfo.LevelName; 
            set => text.text = value;
        }

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData)
        { 
            Clicked.Raise(this);
        }
    }
}