
using DG.Tweening;
using Egsp.Core.Inputs;
using Egsp.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : SerializedMonoBehaviour, IPhysicsEntity
    {
        public const float MovementBlockTime = 0.2f;
        
        [TitleGroup("Move")] [OdinSerialize] public float Speed { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityDistance { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityPower { get; private set; }
        [TitleGroup("Ability")] [OdinSerialize] public float AbilityDelay { get; private set; }

        [TitleGroup("Common")][SerializeField] private LayerMask groundMask;
        
        private Rigidbody2D rig;
        private Collider2D col;

        private Vector3 moveVelocity;

        private ContactFilter2D filter;

        private bool allowAbility = true;
        private Tween abilityDelayTween;

        private Tween movementBlock;

        private bool onGround;
        
        public Vector3 LookDirection { get; private set; }

        public bool IsMovementBlocked => movementBlock != null;

        public bool IsGrounded => onGround;
        
        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            filter = new ContactFilter2D();
            filter.SetLayerMask(groundMask);
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
            moveVelocity = Vector3.right * horizontal * speed;
        }

        private void ApplyMoveVelocity()
        {
            if (IsMovementBlocked)
                return;

            if (!IsGrounded)
                return;
            
            if (!DetectGround(moveVelocity.normalized, moveVelocity.magnitude, Time.fixedDeltaTime))
            {
                if (moveVelocity != Vector3.zero)
                {
                    var velocityChange = moveVelocity - (Vector3) rig.velocity;
                    velocityChange.y = 0;

                    ApplyForce(new Force(velocityChange, ForceMode.VelocityChange));
                }
            }
        }

        public void StopMove() => moveVelocity = Vector3.zero;

        // Position
        
        // Узнаем о наличии препятствия перед объектом.
        private bool DetectGround(Vector3 direction, float distance, float deltaTime = 1f)
        {
            var array = new RaycastHit2D[2];
            if (col.Cast(direction, filter, array, distance * deltaTime, 
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
                if (onGround == false)
                {
                    onGround = true;
                    OnGround();
                }
            }
            else
            {
                if (onGround == true)
                {
                    onGround = false;
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
            if (!allowAbility)
                return;
            
            BlockMovement();
            
            allowAbility = false;
            abilityDelayTween = DOVirtual.DelayedCall(AbilityDelay, () =>
            {
                allowAbility = true;
            });

            // Physics
            var raycastHit2D = Physics2D.Raycast(transform.position, LookDirection, AbilityDistance);
            if (raycastHit2D.collider != null)
            {

                var abilityEndPower = (1 - raycastHit2D.distance / AbilityDistance) * AbilityPower;

                IPhysicsEntity physicsEntity;
                if (raycastHit2D.collider.IsPhysicsEntity(out physicsEntity))
                {
                    physicsEntity.ApplyForce(
                        new Force(LookDirection * abilityEndPower, ForceMode.VelocityChange), this);
                }
                else
                {
                    ApplyForce(
                        new Force(-LookDirection * abilityEndPower, ForceMode.VelocityChange)); 
                }
            }
        }

        public void BlockMovement() => BlockMovement(MovementBlockTime);

        public void BlockMovement(float time)
        {
            if(movementBlock != null)
                movementBlock.Complete(true);
            
            moveVelocity = Vector3.zero;

            movementBlock = DOVirtual.DelayedCall(time, () => movementBlock = null);
        }

        private void LookToMouse()
        {
            var mousePos = InputExtensions.GetMouseWorldPosition();
            LookDirection = (mousePos - transform.position).normalized;
        }
        
        // Physics

        public void ApplyForce(Force force, IPhysicsEntity actor = null)
        {
            rig.AddForce(force);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * AbilityDistance);
        }
    }
}