using UnityEngine;
using UnityEngine.InputSystem;


namespace WebGameProject
{
	/// <summary>
	/// 로컬 플레이어.
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public sealed class LocalPlayer : Player
	{
		#region INSPECTOR
		[Header("Move")]
		[SerializeField] private float moveSpeed = 4.5f;
		[SerializeField] private float sprintSpeed = 7.0f;
		[SerializeField] private float acceleration = 20f;
		[SerializeField] private float gravity = -25f;
		[SerializeField] private float stickToGroundForce = -5f;
		#endregion

		/// <summary>
		/// 캐릭터 컨트롤러.
		/// </summary>
		private CharacterController m_CharacterController;

		/// <summary>
		/// 수평 속도.
		/// </summary>
		private Vector3 m_HorizontalVelocity;

		/// <summary>
		/// 수직 속도.
		/// </summary>
		private float m_VerticalVelocity;

		/// <summary>
		/// 이동 입력.
		/// </summary>
		private InputAction m_MoveAction;

		/// <summary>
		/// 가속 입력.
		/// </summary>
		private InputAction m_SprintAction;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_CharacterController = GetComponent<CharacterController>();

			// 입력 설정.
			m_MoveAction = new InputAction("Move", InputActionType.Value);
			var syntax = m_MoveAction.AddCompositeBinding("2DVector");
			syntax.With("Up", "<Keyboard>/w");
			syntax.With("Down", "<Keyboard>/s");
			syntax.With("Left", "<Keyboard>/a");
			syntax.With("Right", "<Keyboard>/d");

			m_SprintAction = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/leftShift");
		}

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			m_MoveAction.Enable();
			m_SprintAction.Enable();
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected override void OnDisable()
		{
			base.OnDisable();

			m_MoveAction.Disable();
			m_SprintAction.Disable();
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		private void Update()
		{
			var move = m_MoveAction.ReadValue<Vector2>();
			var sprint = m_SprintAction.IsPressed();
			var targetSpeed = sprint ? sprintSpeed : moveSpeed;

			var wishDirection = (transform.right * move.x + transform.forward * move.y);
			if (wishDirection.sqrMagnitude > 1f)
				wishDirection.Normalize();

			var wishVelocity = wishDirection * targetSpeed;

			m_HorizontalVelocity = Vector3.MoveTowards(
				m_HorizontalVelocity,
				wishVelocity,
				acceleration * Time.deltaTime
			);

			if (m_CharacterController.isGrounded)
			{
				if (m_VerticalVelocity < 0f) m_VerticalVelocity = stickToGroundForce;
			}
			else
			{
				m_VerticalVelocity += gravity * Time.deltaTime;
			}

			var velocity = new Vector3(m_HorizontalVelocity.x, m_VerticalVelocity, m_HorizontalVelocity.z);
			m_CharacterController.Move(velocity * Time.deltaTime);
		}
	}
}