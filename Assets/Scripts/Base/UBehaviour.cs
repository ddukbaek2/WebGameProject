using UnityEngine;


/// <summary>
/// 기본 컴포넌트.
/// </summary>
public abstract class UBehaviour : MonoBehaviour
{
	/// <summary>
	/// 생성됨.
	/// </summary>
	protected virtual void Awake()
	{
	}

	/// <summary>
	/// 초기화됨.
	/// </summary>
	protected virtual void Start()
	{

	}

	/// <summary>
	/// 파괴됨.
	/// </summary>
	protected virtual void OnDestroy()
	{

	}

	/// <summary>
	/// 활성화됨.
	/// </summary>
	protected virtual void OnEnable()
	{
	}

	/// <summary>
	/// 비활성화됨.
	/// </summary>
	protected virtual void OnDisable()
	{
	}

	///// <summary>
	///// 갱신됨.
	///// </summary>
	//protected virtual void Update()
	//{
	//}

	///// <summary>
	///// 이후 갱신됨.
	///// </summary>
	//protected virtual void LateUpdate()
	//{
	//}
}