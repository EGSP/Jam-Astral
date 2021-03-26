
using DG.Tweening;
using Egsp.Core.Inputs;
using Egsp.Core;
using Egsp.Extensions.Primitives;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    [LazyInstance(false)]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public partial class Player : SerializedSingleton<Player>, IMonoPhysicsEntity, ISceneLoadTrigger
    {
        public const float MovementBlockTime = 0.2f;
        
        [TitleGroup("Move")] [OdinSerialize] public float Speed { get; private set; }
        
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityDistance { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityRadius { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityPower { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityDelay { get; private set; }

        [TitleGroup("Common")][SerializeField] private LayerMask groundMask;
        
        private Rigidbody2D _rig;
        private Collider2D _col;

        // Runtime values
        private Vector3 _moveVelocity;

        // Physics
        private ContactFilter2D _filter;

        // Tweens
        private Tween _abilityDelayTween;
        private Tween _movementBlock;

        // States
        private bool _onGround;
        private bool _allowAbility = true;
        
        public Vector3 LookDirection { get; private set; }
        
        /// <summary>
        /// Этап заморозки способности. 0 - не готова 1 - готова.
        /// </summary>
        public float AbilityReadiness { get; private set; }

        public bool IsMovementBlocked => _movementBlock != null;

        public bool IsGrounded => _onGround;

        public GameObject GameObject => gameObject;

        protected override void Awake()
        {
            _rig = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(groundMask);

            AbilityReadiness = _allowAbility.ToInt();
            
            InitHealth();
            
            // Вызываю после, т.к. вызывается событие о создании экземпляра.
            base.Awake();
        }

        private void Update()
        {
            LookToMouse();
        }

        private void FixedUpdate()
        {
            ApplyMoveVelocity();
            CheckGroundState();
        }
        
        // Movement

        public void Move(float horizontal) => Move(horizontal, Speed);
        
        public void Move(float horizontal, float speed)
        {
            _moveVelocity = Vector3.right * horizontal * speed;
        }

        private void ApplyMoveVelocity()
        {
            if (IsMovementBlocked)
                return;

            if (!IsGrounded)
                return;
            
            if (!DetectGround(_moveVelocity.normalized, _moveVelocity.magnitude, Time.fixedDeltaTime))
            {
                if (_moveVelocity != Vector3.zero)
                {
                    var velocityChange = _moveVelocity - (Vector3) _rig.velocity;
                    velocityChange.y = 0;

                    ApplyForceInternal(new Force(velocityChange, ForceMode.VelocityChange));
                }
            }
        }

        public void StopMove() => _moveVelocity = Vector3.zero;

        // Position
        
        // Узнаем о наличии препятствия перед объектом.
        private bool DetectGround(Vector3 direction, float distance, float deltaTime = 1f)
        {
            var array = new RaycastHit2D[2];
            if (_col.Cast(direction, _filter, array, distance * deltaTime, 
                true) != 0)
            {
                return true;
            }

            return false;
        }

        private void CheckGroundState()
        {
            if (DetectGround(Vector3.down, 0.1f))
            {
                if (_onGround == false)
                {
                    _onGround = true;
                    OnGround();
                }
            }
            else
            {
                if (_onGround == true)
                {
                    _onGround = false;
                    OnAir();
                }
            }
        }

        private void OnGround()
        {
        }

        private void OnAir()
        {
        }

        public void UseAbility()
        {
            if (!_allowAbility)
                return;

            // Physics
            var raycastHit2D = Physics2D.CircleCast(transform.position, AbilityRadius,
                LookDirection, AbilityDistance - AbilityRadius);
            
            if (raycastHit2D.collider != null)
            {

                // var abilityEndPower = (1 - raycastHit2D.distance / AbilityDistance) * AbilityPower;
                var abilityEndPower = AbilityPower;

                IPhysicsEntity physicsEntity;
                if (raycastHit2D.collider.IsPhysicsEntity(out physicsEntity))
                {
                    BlockMovement();
                    physicsEntity.ApplyForce(
                        new Force(LookDirection * abilityEndPower, ForceMode.VelocityChange), this);
                }
                else
                {
                    ApplyForceInternal(
                        new Force(-LookDirection * abilityEndPower, ForceMode.VelocityChange), true); 
                }
                
                
                _allowAbility = false;
                _abilityDelayTween = DOVirtual.DelayedCall(AbilityDelay, () =>
                {
                    _allowAbility = true;
                });

                var _abilityReadinessTweener = DOVirtual.Float(0, 1, AbilityDelay,
                    f => AbilityReadiness = f);
            }
        }

        public void BlockMovement() => BlockMovement(MovementBlockTime);

        public void BlockMovement(float time)
        {
            if(_movementBlock != null)
                _movementBlock.Complete(true);
            
            _moveVelocity = Vector3.zero;

            _movementBlock = DOVirtual.DelayedCall(time, () => _movementBlock = null);
        }

        private void LookToMouse()
        {
            var mousePos = InputExtensions.GetMouseWorldPosition();
            LookDirection = (mousePos - transform.position).normalized;
        }
        
        // Physics

        public void ApplyForce(Force force, IPhysicsEntity actor = null)
        {
            ApplyForceInternal(force, true);
        }

        private void ApplyForceInternal(Force force, bool blockMovement = false, IPhysicsEntity actor = null)
        {
            if(blockMovement)
                BlockMovement();
            
            _rig.AddForce(force);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * AbilityDistance);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * (AbilityDistance-AbilityRadius), AbilityRadius);
        }
    }
}