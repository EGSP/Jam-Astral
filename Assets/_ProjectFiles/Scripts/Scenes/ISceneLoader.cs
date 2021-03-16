using UnityEngine.SceneManagement;

namespace Egsp.Core
{
    /// <summary>
    /// Этот объект загружает сцену.
    /// </summary>
    public interface ISceneLoader
    {
        State State { get; }

        void LoadScene();
    }
    
    public enum State
    {
        Loading,
        WaitingForActivation
    }

    /// <summary>
    /// Данный объект может вызвать загрузку сцены. Например объект-игрок.
    /// </summary>
    public interface ISceneLoadTrigger
    {
        
    }
}