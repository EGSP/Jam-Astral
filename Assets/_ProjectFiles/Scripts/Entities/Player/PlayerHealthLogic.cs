using Egsp.Core;
using Game.Entities;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Health))]
    public partial class Player
    {
        private Health _health;

        public Option<Health> Health => _health == null ? Option<Health>.None : _health;

        public void InitHealth()
        {
            _health = GetComponent<Health>();
            _health.ManualAwake();
        }
    }
}