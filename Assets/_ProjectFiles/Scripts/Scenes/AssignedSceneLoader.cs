using Egsp.Core;
using Game;
using UnityEngine;

namespace Game.Scene
{
    public class AssignedSceneLoader : CircleTrigger2D, IAssignedSceneLoader
    {
        [Tooltip("Case insensitive and empty-safe")]
        [SerializeField] private string nextSceneName;

        public State state { get; private set; } = State.WaitingForActivation;
        
        public string NextSceneName => nextSceneName;

        protected override void OnEnter(GameObject enteredObject)
        {
            if (enteredObject.GetComponent<Player>())
                LoadAssignedScene();
        }

        public void LoadAssignedScene()
        {
            if (state == State.Loading)
                return;
            
            switch (GameSceneManager.SceneExistInBuild(NextSceneName))
            {
                case GameSceneManager.SceneExistInBuildResult.Exist:
                    GameSceneManager.Instance.LoadSceneSingle(NextSceneName, true,
                        null, null);

                    state = State.Loading;
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.NotExist:
                    Debug.Log("Scene does not exist!");
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.IncorrectName:
                    Debug.Log($"Incorrect scene name : {NextSceneName}");
                    break;
            }
        }

        public enum State
        {
            Loading,
            WaitingForActivation
        }
    }
}