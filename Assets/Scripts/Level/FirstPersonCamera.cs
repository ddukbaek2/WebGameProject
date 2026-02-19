using UnityEngine;
using UnityEngine.InputSystem;


namespace WebGameProject
{
	/// <summary>
	/// 플레이어를 따라가는 1인칭 카메라.
	/// </summary>
	public sealed class FirstPersonCamera : UBehaviour
	{
		#region INSPECTOR
		[Header("Target")]
		[SerializeField] private Transform m_PlayerTransform;                 // Player 루트
		[SerializeField] private Vector3 m_HeadLocalOffset = new Vector3(0f, 1.6f, 0f); // 머리 위치(플레이어 로컬 기준)

		[Header("Look")]
		[SerializeField] private float m_MouseSensitivity = 0.12f;
		[SerializeField] private float m_PitchMin = -85f;
		[SerializeField] private float m_PitchMax = 85f;

		[Header("Follow")]
		[SerializeField] private bool m_IsSmoothFollow = false;
		[SerializeField] private float m_FollowSharpness = 25f;
		#endregion

		/// <summary>
		/// 가로축 회전.
		/// </summary>
		private float m_Yaw;

		/// <summary>
		/// 세로축 회전.
		/// </summary>
		private float m_Pitch;

		/// <summary>
		/// 시야 입력.
		/// </summary>
		private InputAction m_LookAction;

		/// <summary>
		/// 카메라 회전 여부 프로퍼티.
		/// </summary>
		private bool CanLook
		{
			get
			{
				// Cursor.lockState == CursorLockMode.Locked
				return Application.isFocused;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			if (m_PlayerTransform == null)
			{
				// 씬에서 Player 태그 쓰면 자동 연결하고 싶으면:
				// var go = GameObject.FindWithTag("Player");
				// if (go != null) player = go.transform;
			}

			// 마우스 델타는 Pointer가 환경에 따라 더 안정적
			m_LookAction = new InputAction("Look", InputActionType.Value);
			m_LookAction.AddBinding("<Pointer>/delta");

			//Cursor.lockState = CursorLockMode.Locked;
			//Cursor.visible = false;

			if (m_PlayerTransform != null)
				m_Yaw = m_PlayerTransform.eulerAngles.y;
			m_Pitch = 0f;
		}

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			m_LookAction.Enable();
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected override void OnDisable()
		{
			base.OnDisable();

			m_LookAction.Disable();
		}

		/// <summary>
		/// 이후 갱신됨.
		/// </summary>
		private void LateUpdate()
		{
			if (m_PlayerTransform == null) return;

			// 🔒 마우스 락이 풀리면 카메라 회전 중단
			if (CanLook)
			{
				var moveDelta = m_LookAction.ReadValue<Vector2>();

				m_Yaw += moveDelta.x * m_MouseSensitivity;
				if (m_Yaw >= 360f) m_Yaw -= 360f;
				else if (m_Yaw < 0f) m_Yaw += 360f;

				m_Pitch -= moveDelta.y * m_MouseSensitivity;
				m_Pitch = Mathf.Clamp(m_Pitch, m_PitchMin, m_PitchMax);
			}

			// 플레이어의 정면 방향 설정.
			m_PlayerTransform.rotation = Quaternion.Euler(0f, m_Yaw, 0f);

			// 플레이어의 상하 시각 방향 설정.
			var targetPos = m_PlayerTransform.TransformPoint(m_HeadLocalOffset);
			transform.position = targetPos;
			transform.rotation = Quaternion.Euler(m_Pitch, m_Yaw, 0f);
		}
	}
}