using Egsp.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Levels
{
    /// <summary>
    /// Компонент просто загружает переданный уровень.
    /// </summary>
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LoadSceneMode mode;
        
        public void LoadLevel(LevelInfo levelInfo)
        {
            LevelInfo.LoadLevel(levelInfo);
        }

        /// <summary>
        /// Загружает следующий по списку уровень.
        /// </summary>
        public void LoadNextLevel()
        {
            LevelInfo.LoadNextLevel();
        }
    }
}