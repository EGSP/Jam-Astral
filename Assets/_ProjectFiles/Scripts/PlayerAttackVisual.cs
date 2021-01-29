using System;
using Egsp.Core.Inputs;
using Egsp.Extensions.Primitives;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class PlayerAttackVisual : SerializedMonoBehaviour
    {
        [TitleGroup("Attack")] [SerializeField] private Transform attackVisual;
        [TitleGroup("Attack")] [SerializeField] private float distanceFromCentre;

        [SerializeField] private Player player;

        private void Update()
        {
            Rotate(player);
        }

        public void Rotate(Player player)
        {
            if (player == null)
                return;

            var visualPosition = player.transform.position + player.LookDirection * distanceFromCentre;
            
            attackVisual.position = visualPosition;
            attackVisual.localRotation = player.LookDirection.LookRotation2D();
        }
    }
}