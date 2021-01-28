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

            if(Input.GetKeyDown(KeyCode.Space))
                player.Jump();
            
            player.Move(horizontal);
        }
    }
}