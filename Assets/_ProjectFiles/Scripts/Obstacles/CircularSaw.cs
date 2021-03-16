using Egsp.Core;
using Game.Entities;
using UnityEngine;

namespace Game.Obstacles
{
    public class CircularSaw : CircleTrigger2D
    {
        public static readonly Color SpikeColor = Color.red;  
        
        [SerializeField] private float punchPower;
        [SerializeField] private HitInfo damage;

        [SerializeField] private Transform spriteTransform;
        [SerializeField] private float anglePerDelta;

        protected override void Update()
        {
            base.Update();

            spriteTransform.localRotation *= Quaternion.Euler(0, 0, anglePerDelta * Time.deltaTime);
        }

        protected override void OnEnter(GameObject enteredObject)
        {
            var physicsEntity = enteredObject.GetComponent<IPhysicsEntity>();
            
            if (physicsEntity != null)
            {
                var health = enteredObject.GetComponent<Health>();

                if (health != null)
                    health.Damage(damage);
                
                physicsEntity.ApplyForceFrom(enteredObject.transform, transform, punchPower);
            }
        }

        protected override Color GetGizmosColor() => SpikeColor;
    }
    
}