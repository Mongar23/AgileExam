using MBevers;
using MBevers.Menus;
using UnityEngine;

namespace VeiligWerken
{
    /// <summary>
    ///     This class handles the movement of the player and the accompanying animation.
    ///     <para>Created by Mathias on 12-05-2021</para>
    /// </summary>
    public class PlayerMovement : ExtendedMonoBehaviour
    {
        private static readonly int ANIMATOR_HORIZONTAL = Animator.StringToHash("Horizontal");
        private static readonly int ANIMATOR_VERTICAL = Animator.StringToHash("Vertical");
        private static readonly int ANIMATOR_SPEED = Animator.StringToHash("Speed");

        [SerializeField] [Range(1.0f, 10.0f)] private float movementSpeed = 10.0f;
        [SerializeField] [Range(1.0f, 2.0f)] private float sprintMultiplier = 1.25f;

        private bool IsSprinting => Input.GetAxis("Sprint") > 0;

        private Animator animator;
        private new Rigidbody2D rigidbody2D = null;
        private Vector2 input = new Vector2();

        private void Start()
        {
            rigidbody2D = ForceComponent<Rigidbody2D>();
            animator = GetComponentIfInitialized<Animator>();
        }

        private void Update()
        {
            if(MenuManager.Instance.IsAnyOpen) { return; }

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input.Normalize();

            animator.SetFloat(ANIMATOR_HORIZONTAL, input.x);
            animator.SetFloat(ANIMATOR_VERTICAL, input.y);
            animator.SetFloat(ANIMATOR_SPEED, input.sqrMagnitude);
            animator.speed = IsSprinting ? sprintMultiplier : 1.0f;
        }

        private void FixedUpdate()
        {
            if(MenuManager.Instance.IsAnyOpen) { return; }
            
            Vector2 movement = input * (movementSpeed * Time.fixedDeltaTime);

            if(IsSprinting) { movement *= sprintMultiplier; }

            rigidbody2D.MovePosition(rigidbody2D.position + movement);
        }
    }
}