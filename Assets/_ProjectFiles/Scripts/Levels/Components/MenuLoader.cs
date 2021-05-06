using Egsp.Core;
using UnityEngine;

namespace Game.Levels
{
    public class MenuLoader : MonoBehaviour
    {
        public void LoadMenu()
        {
            GameSceneManager.Instance.LoadSceneSingle("MainMenu", true);
        }
    }
}