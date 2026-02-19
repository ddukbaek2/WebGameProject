using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 플레이어를 따라가는 1인칭 카메라.
/// </summary>
public sealed class FirstPersonCamera : MonoBehaviour
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
	private void Awake()
	{
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
	private void OnEnable()
	{
		m_LookAction.Enable();
	}

	/// <summary>
	/// 비활성화됨.
	/// </summary>
	private void OnDisable()
	{
		m_LookAction.Disable();
	}

	///// <summary>
	///// 갱신됨.
	///// </summary>
	//private void Update()
	//{
	//	if (Keyboard.current.escapeKey.wasPressedThisFrame)
	//	{
	//		if (Cursor.lockState == CursorLockMode.Locked)
	//		{
	//			Cursor.lockState = CursorLockMode.None;
	//			Cursor.visible = true;
	//		}
	//		else
	//		{
	//			Cursor.lockState = CursorLockMode.Locked;
	//			Cursor.visible = false;
	//		}
	//	}
	//}

	/// <summary>
	/// 나중에 갱신됨.
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

	//private void LateUpdate()
	//{
	//	if (player == null) return;

	//	// 1) Look 입력
	//	Vector2 delta = lookAction.ReadValue<Vector2>();
	//	yaw += delta.x * mouseSensitivity;
	//	pitch -= delta.y * mouseSensitivity;
	//	pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

	//	// 2) yaw는 플레이어 루트에 적용 (몸 회전)
	//	player.rotation = Quaternion.Euler(0f, yaw, 0f);

	//	// 3) 카메라는 플레이어 머리 위치로 “독립적으로” 따라감
	//	Vector3 targetPos = player.TransformPoint(headLocalOffset);

	//	if (!smoothFollow)
	//	{
	//		transform.position = targetPos;
	//	}
	//	else
	//	{
	//		// 지수 감쇠 형태로 부드럽게 따라가기
	//		float t = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
	//		transform.position = Vector3.Lerp(transform.position, targetPos, t);
	//	}

	//	// 4) 카메라 회전: yaw는 플레이어와 동일, pitch는 카메라만
	//	transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
	//}
}