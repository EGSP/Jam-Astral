using Egsp.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Levels
{
    // Триггер, который помечает уровень при активации игроком.
    public class LevelCompleter : CircleTrigger2D
    {
        public State State { get; private set; } = State.WaitingForActivation;

        protected override void OnEnter(GameObject enteredObject)
        {
            if (enteredObject.GetComponent<ISceneLoadTrigger>() != null)
                CompleteCurrentLevel();
        }

        public void CompleteCurrentLevel()
        {
            // Помечаем текущий уровень как пройденный.
            LevelInfo.CompleteCurrentLevel();
        }
    }
}