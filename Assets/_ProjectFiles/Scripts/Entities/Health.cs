using System;
using DG.Tweening;
using Egsp.Core;
using UnityEngine;

namespace Game.Entities
{
    public interface IHealthListener
    {
        void OnHealed(Health health, HealthPoint heal);

        void OnDamaged(Health health, HitInfo hitInfo);
        
        void OnKilled(Health health);
    }
    
    public class Health : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private int immuneTime;

        private Tween _immuneTween;

        public TypedBus<IHealthListener> HealthPointsBus = new TypedBus<IHealthListener>();

        public HealthPoint HealthPoints { get; private set; }
        
        /// <summary>
        /// Иммунитет к получению урона.
        /// </summary>
        public bool ImmuneActive { get; private set; }

        private void Awake()
        {
            ManualAwake();
        }

        private bool _manualyAwaked;
        public void ManualAwake()
        {
            if (_manualyAwaked)
                return;
            HealthPoints = new HealthPoint(health);

            _manualyAwaked = true;
        }

        public void Heal(HealthPoint healthPoint)
        {
            HealthPoints += healthPoint;
            HealthPointsBus.Raise(x => x.OnHealed(this, healthPoint));
        }

        private void ActivateImmune()
        {
            ImmuneActive = true;
            _immuneTween = DOVirtual.DelayedCall(immuneTime, () =>
            {
                ImmuneActive = false;
                _immuneTween = null;
            });
        }

        public void Damage(HitInfo hitInfo)
        {
            // Если в любом случае не будет нанес урон.
            if (ImmuneActive && !hitInfo.ignoreImmune)
                return;

            if (hitInfo.kill)
            {
                Kill();
            }
            else
            {
                HealthPoints -= hitInfo.damagePoint;
                
                if(HealthPoints.Zero)
                    Kill();
                else
                {
                    ActivateImmune();
                    HealthPointsBus.Raise(x => x.OnDamaged(this, hitInfo));
                }
            }
            
            Debug.Log(HealthPoints.count);
        }

        private void Kill()
        {
            if (_immuneTween != null)
                _immuneTween.Kill(true);

            HealthPoints = new HealthPoint(0);
            HealthPointsBus.Raise(x => x.OnKilled(this));
        }
    }

    [Serializable]
    public struct HealthPoint
    {
        public int count;

        public bool Zero => count == 0;

        public HealthPoint(int count)
        {
            if (count < 0)
                count = 0;
            
            this.count = count;
        }

        public static HealthPoint operator +(HealthPoint healthPoint1, HealthPoint healthPoint2)
        {
            return new HealthPoint(healthPoint1.count + healthPoint2.count);
        }

        public static HealthPoint operator -(HealthPoint healthPoint, DamagePoint damagePoint)
        {
            return new HealthPoint(healthPoint.count - damagePoint.count);
        }
    }

    [Serializable]
    public struct DamagePoint
    {
        public int count;

        public DamagePoint(int count)
        {
            if (count < 0)
                count = 0;
            
            this.count = count;
        }
    }

    [Serializable]
    public struct HitInfo
    {
        public DamagePoint damagePoint;

        /// <summary>
        /// Будет ли сущность уничтожена несмотря на показатели урона.
        /// Иммунитет не даст сущности умереть.
        /// </summary>
        public bool kill;

        public bool ignoreImmune;

        public HitInfo(DamagePoint damagePoint, bool kill = false, bool ignoreImmune = false)
        {
            this.damagePoint = damagePoint;
            this.ignoreImmune = ignoreImmune;
            this.kill = kill;
        }
    }
}