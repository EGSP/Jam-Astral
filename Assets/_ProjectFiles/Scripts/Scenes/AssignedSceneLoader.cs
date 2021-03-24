using UnityEngine;

namespace Egsp.Core
{
    public class AssignedSceneLoader : CircleTrigger2D, ISceneLoader
    {
        [Tooltip("Case insensitive and empty-safe")]
        [SerializeField] private string nextSceneName;

        public State State { get; private set; } = State.WaitingForActivation;
        
        public string NextSceneName => nextSceneName;

        protected override void OnEnter(GameObject enteredObject)
        {
            if (enteredObject.GetComponent<ISceneLoadTrigger>() != null)
                LoadScene();
        }

        public void LoadScene()
        {
            if (State == State.Loading)
                return;
            
            switch (GameSceneManager.SceneExistInBuild(NextSceneName))
            {
                case GameSceneManager.SceneExistInBuildResult.Exist:
                    GameSceneManager.Instance.LoadSceneSingle(NextSceneName, true,
                        null, null);

                    State = State.Loading;
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.NotExist:
                    Debug.Log($"Scene: {nextSceneName} - does not exist!");
                    break;
                
                case GameSceneManager.SceneExistInBuildResult.IncorrectName:
                    Debug.Log($"Incorrect scene name : {NextSceneName}");
                    break;
            }
        }
    }
}