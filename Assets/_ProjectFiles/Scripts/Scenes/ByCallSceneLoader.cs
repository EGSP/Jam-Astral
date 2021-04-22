using UnityEngine;

namespace Egsp.Core
{
    /// <summary>
    /// Компонент загружает сцену по переданному названию. Используется вместе с UnityEvents.
    /// </summary>
    public class ByCallSceneLoader : MonoBehaviour, ISceneLoader
    {
        public State State { get; private set; } = State.WaitingForActivation;

        public void LoadScene()
        {
            return;
        }
        
        public void LoadScene(string sceneName)
        {
            if (State == State.Loading)
                return;
            
            switch (GameSceneManager.SceneExistInBuild(sceneName))
            {
                case GameSceneManager.SceneExistInBuildResult.Exist:
                    GameSceneManager.Instance.LoadSceneSingle(sceneName, true,
                        null, null);

                    State = State.Loading;
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.NotExist:
                    Debug.Log("Scene does not exist!");
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.IncorrectName:
                    Debug.Log($"Incorrect scene name : {sceneName}");
                    break;
            }
        }
    }
}