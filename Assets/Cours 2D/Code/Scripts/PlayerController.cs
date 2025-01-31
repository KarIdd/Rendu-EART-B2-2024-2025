using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace HappyHarvest
{
    public class PlayerController : MonoBehaviour
    {
        public InputActionAsset InputAction;
        public float Speed = 4.0f;

        public Animator Animator => m_Animator;

        //This is private as we don't want to be able to set coins without going through the accessor above that ensure
        //the UI is updated, but is tagged as SerializedField so it appear in the editor so designer can set the starting
        //amount of coins

        private Rigidbody2D m_Rigidbody;

        private InputAction m_MoveAction;

        private Vector3 m_CurrentWorldMousePos;
        private Vector2 m_CurrentLookDirection;

        private Animator m_Animator;

        private int m_DirXHash = Animator.StringToHash("DirX");
        private int m_DirYHash = Animator.StringToHash("DirY");
        private int m_SpeedHash = Animator.StringToHash("Speed");

        void Awake()
        {
            if (GameManager.Instance.Player != null)
            {
                Destroy(gameObject);
                return;
            }

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponentInChildren<Animator>();

            //we can only set DontDestroyOnLoad root object, so ensure its root (Level Designer sometime place the character
            //prefab already in the scene and can sometime tuck it under other object in the hierarchy)
            gameObject.transform.SetParent(null);

            GameManager.Instance.Player = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            //Move action doesn't have any callback as it will be polled in the movement code directly.
            m_MoveAction = InputAction.FindAction("Gameplay/Move");
            m_MoveAction.Enable();

            m_CurrentLookDirection = Vector2.right;
        }

        void FixedUpdate()
        {
            var move = m_MoveAction.ReadValue<Vector2>();

            //note: == and != for vector2 is overriden to take in account floating point imprecision.
            if (move != Vector2.zero)
            {
                SetLookDirectionFrom(move);
            }
            else
            {
                //we aren't moving, look direction is based on the currently aimed toward point
                if (IsMouseOverGameWindow())
                {
                    Vector3 posToMouse = m_CurrentWorldMousePos - transform.position;
                    SetLookDirectionFrom(posToMouse);
                }
            }

            var movement = move * Speed;
            var speed = movement.sqrMagnitude;

            m_Animator.SetFloat(m_DirXHash, m_CurrentLookDirection.x);
            m_Animator.SetFloat(m_DirYHash, m_CurrentLookDirection.y);
            m_Animator.SetFloat(m_SpeedHash, speed);

            m_Rigidbody.MovePosition(m_Rigidbody.position + movement * Time.deltaTime);
        }

        bool IsMouseOverGameWindow()
        {
            return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
        }

        void SetLookDirectionFrom(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                m_CurrentLookDirection = direction.x > 0 ? Vector2.right : Vector2.left;
            }
            else
            {
                m_CurrentLookDirection = direction.y > 0 ? Vector2.up : Vector2.down;
            }
        }
    }
}