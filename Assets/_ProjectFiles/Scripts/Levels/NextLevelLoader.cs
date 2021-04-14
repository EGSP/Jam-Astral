using Egsp.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Levels
{
    public class NextLevelLoader : CircleTrigger2D, ISceneLoader
    {
        public State State { get; private set; } = State.WaitingForActivation;

        protected override void OnEnter(GameObject enteredObject)
        {
            if (enteredObject.GetComponent<ISceneLoadTrigger>() != null)
                LoadScene();
        }

        public void LoadScene()
        {
            // Помечаем текущий уровень.
            LevelInfo.CompleteCurrentLevel();
            
            var nextLevel = LevelInfo.GetNextLevel();
            if (!nextLevel.IsSome)
            {
                Debug.Log("Следующий уровень не был найден.");
                return;
            }

            if (State == State.Loading)
                return;

            var nextSceneName = nextLevel.Value.LevelName;
            GameSceneManager.Instance.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}