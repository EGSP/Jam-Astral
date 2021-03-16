using System;
using UnityEngine;

namespace Game.Visuals
{
    public class PlayerControllerUi : MonoBehaviour
    {
        [SerializeField] private AbilityVisual abilityVisual;
        [SerializeField] private HealthVisual healthVisual;

        private Player _player;
        
        public void Accept(Player player)
        {
            _player = player;

            var health = _player.Health;

            if (health.IsSome)
                healthVisual.Accept(health.Value);
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