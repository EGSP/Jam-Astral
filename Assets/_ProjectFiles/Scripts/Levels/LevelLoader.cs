using Egsp.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Levels
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LoadSceneMode mode;
        
        public void LoadLevel(LevelInfo levelInfo)
        {
            GameSceneManager.Instance.LoadScene(levelInfo.LevelName, mode);
        }
        
    }
}