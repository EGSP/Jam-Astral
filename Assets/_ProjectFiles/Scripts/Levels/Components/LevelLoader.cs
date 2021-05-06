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
            GameSceneManager.Instance.LoadScene(levelInfo.LevelName, mode);
        }

        /// <summary>
        /// Загружает следующий по списку уровень.
        /// </summary>
        public void LoadNextLevel()
        {
            var nextLevel = LevelInfo.GetNextLevel();
            if (!nextLevel.IsSome)
            {
                Debug.Log("Следующий уровень не был найден.");
                return;
            }

            var nextSceneName = nextLevel.Value.LevelName;
            GameSceneManager.Instance.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}