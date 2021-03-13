using System;
using UnityEngine;

namespace Game.Visuals
{
    public class PlayerControllerUi : MonoBehaviour
    {
        [SerializeField] private AbilityVisual abilityVisual;

        private Player _player;
        
        public void Accept(Player player)
        {
            _player = player;
        }

        private void Update()
        {
            if (_player == null)
                return;

            if (abilityVisual != null)
                abilityVisual.SetOpacity(_player.AbilityReadiness);
        }
    }
}