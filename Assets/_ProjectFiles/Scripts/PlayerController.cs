using Game.Visuals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class PlayerController : SerializedMonoBehaviour
    {
        [SerializeField] private FollowCamera followCamera;
        [SerializeField] private PlayerControllerUi controllerUi;

        private Player _player;
        
        private void Awake()
        {
            // Ждем создания игрока и после проводим настройку.
            Player.OnInstanceCreated.Subscribe(OnPlayerAssigned);
        }

        private void Update()
        {
            if (_player == null)
                return;
            
            var horizontal = Input.GetAxisRaw("Horizontal");

            _player.Move(horizontal);
            
            if(Input.GetKeyDown(KeyCode.Mouse0))
                _player.UseAbility();
        }

        private void OnPlayerAssigned(Player player)
        {
            _player = player;

            if (followCamera != null)
                followCamera.Target = new Transform2DPoint(_player.transform);
            
            if(controllerUi != null)
                controllerUi.Accept(_player);
        }
    }
}