using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public class PlayerController : SerializedMonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private FollowCamera followCamera;

        private void Awake()
        {
            followCamera.Target = new Transform2DPoint(player.transform);
        }

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");

            player.Move(horizontal);
            
            if(Input.GetKeyDown(KeyCode.Mouse0))
                player.UseAbility();
        }
    }
}