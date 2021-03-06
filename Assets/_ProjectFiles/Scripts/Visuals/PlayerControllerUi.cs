﻿using UnityEngine;

namespace Game.Visuals
{
    /// <summary>
    /// Корневой компонент для UI, связанного с игроком. Производит первоначальную настройку.
    /// Требует вызова извне.
    /// </summary>
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
            {
                healthVisual.Accept(health.Value);
            }else
                Debug.Log("[UI] Player's health not initialized.");
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