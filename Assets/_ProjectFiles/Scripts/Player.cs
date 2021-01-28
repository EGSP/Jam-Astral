
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : SerializedMonoBehaviour
    {
        [OdinSerialize] public float Speed { get; private set; }
        [OdinSerialize] public float JumpHeight { get; private set; }
        [SerializeField] private float jumpDelay;
        
        [SerializeField] private LayerMask groundMask;

        private Rigidbody2D rig;
        private Collider2D col;

        private Vector3 moveVelocity;

        private ContactFilter2D filter;

        private bool allowJump = true;
        private Tween jumpDelayer;

        private bool onGround;
        
        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            filter = new ContactFilter2D();
            filter.SetLayerMask(groundMask);
        }

        private void FixedUpdate()
        {
            if (!DetectGround(moveVelocity.normalized, moveVelocity.magnitude))
            {
                var velocityChange = moveVelocity - (Vector3)rig.velocity;
                velocityChange.y = 0;
            
                rig.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            
            CheckGroundState();
        }

        public void Move(float horizontal) => Move(horizontal, Speed);
        public void Move(float horizontal, float speed)
        {
            moveVelocity = Vector3.right * horizontal * speed;
        }

        public void StopMove() => moveVelocity = Vector3.zero;

        public void Jump() => Jump(Vector3.up);
        public void Jump(Vector3 direction)
        {
            if (!allowJump || !onGround)
                return;
            
            allowJump = false;
            rig.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);

            jumpDelayer = DOVirtual.DelayedCall(jumpDelay, () =>
            {
                allowJump = true;
                jumpDelayer = null;
            });
        }
        
        // Узнаем о наличии препятствия перед объектом.
        private bool DetectGround(Vector3 direction, float distance, bool useDeltaTime = true)
        {
            var deltaTime = Time.fixedDeltaTime;
            if (!useDeltaTime)
                deltaTime = 1;
            
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
            if (DetectGround(Vector3.down, 0.1f, false))
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
            if (jumpDelayer != null)
            {
                jumpDelayer.Complete(true);
            }
        }

        private void OnAir()
        {
            
        }
    }
    
    
    public static class Physics2DExtensions {
        public static void AddForce (this Rigidbody2D rigidbody2D, Vector2 force, ForceMode mode = ForceMode.Force) {
            switch (mode) {
                case ForceMode.Force:
                    rigidbody2D.AddForce (force);
                    break;
                case ForceMode.Impulse:
                    rigidbody2D.AddForce (force / Time.fixedDeltaTime);
                    break;
                case ForceMode.Acceleration:
                    rigidbody2D.AddForce (force * rigidbody2D.mass);
                    break;
                case ForceMode.VelocityChange:
                    rigidbody2D.AddForce (force * rigidbody2D.mass / Time.fixedDeltaTime);
                    break;
            }
        }
     
        public static void AddForce (this Rigidbody2D rigidbody2D, float x, float y, ForceMode mode = ForceMode.Force) {
            rigidbody2D.AddForce (new Vector2 (x, y), mode);
        }
        
        
    }
}