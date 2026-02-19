using UnityEngine;


namespace WebGameProject
{
	/// <summary>
	/// 플레이어.
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public abstract class Player : UBehaviour
	{
		/// <summary>
		/// 갱신됨.
		/// </summary>
		protected virtual void Update()
		{

		}
	}
}