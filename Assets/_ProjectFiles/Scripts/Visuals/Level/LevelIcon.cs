using Egsp.Core;
using Egsp.Core.Ui;
using Game.Levels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Visuals.Level
{
    [RequireComponent(typeof(Image))]
    public class LevelIcon : Visual, IPointerDownHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        private LevelInfo _levelInfo;
        
        public WeakEvent<LevelIcon> Clicked = new WeakEvent<LevelIcon>();

        public bool Completed
        {
            get => _levelInfo.Completed;
            private set
            {
                if(value)
                    image.color = Color.green;
                else
                    image.color = Color.yellow;
            }
        }

        public string Name
        {
            get => _levelInfo.LevelName;
            private set => text.text = value;
        }

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

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData)
        { 
            Debug.Log("clicked");
            Clicked.Raise(this);
        }
    }
}