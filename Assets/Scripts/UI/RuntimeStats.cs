using Crockhead.Unity.UI;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;


namespace WebGameProject
{
	/// <summary>
	/// 프로파일 출력.
	/// </summary>
	public class RuntimeStats : UBehaviour
	{
		private UILabelView m_LabelView;
		private StringBuilder m_StringBuilder;
		private float m_CheckFrameRateTime;
		private int m_CheckFrameCount;
		private int m_FramePerSecond;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_LabelView = GetComponent<UILabelView>();
			m_StringBuilder = new StringBuilder();
			m_CheckFrameRateTime = 0f;
			m_CheckFrameCount = 0;
			m_FramePerSecond = 0;
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		private void Update()
		{
			m_StringBuilder.Clear();

			CheckFramePerSecond(m_StringBuilder);
			CheckMemory(m_StringBuilder);

			m_StringBuilder.AppendLine($"Cursor.lockState: {Cursor.lockState}");
			m_StringBuilder.AppendLine($"Cursor.visible: {Cursor.visible}");
			m_LabelView.text = m_StringBuilder.ToString();
		}

		/// <summary>
		/// 초당 프레임 체크.
		/// </summary>
		private void CheckFramePerSecond(StringBuilder stringBuilder)
		{
			++m_CheckFrameCount;
			m_CheckFrameRateTime += Time.unscaledDeltaTime;
			if (m_CheckFrameRateTime >= 1f)
			{
				m_FramePerSecond = (int)(m_CheckFrameCount / m_CheckFrameRateTime);
				m_CheckFrameCount = 0;
				m_CheckFrameRateTime = 0f;
			}

			stringBuilder.AppendLine($"Frame Per Second: {m_FramePerSecond}");
		}

		/// <summary>
		/// 메모리 체크.
		/// </summary>
		private void CheckMemory(StringBuilder stringBuilder)
		{
			// 모노/ILL2CPP 할당.
			var monoUsed = Profiler.GetMonoUsedSizeLong();
			var monoHeap = Profiler.GetMonoHeapSizeLong();

			// 유니티 할당.
			var totalAllocated = Profiler.GetTotalAllocatedMemoryLong();
			var totalReserved = Profiler.GetTotalReservedMemoryLong();
			var totalUnusedReserved = Profiler.GetTotalUnusedReservedMemoryLong();

			// 그래픽 할당.
			//var gfxDriver = Profiler.GetAllocatedMemoryForGraphicsDriver();			
			var textureAllocated = GetTotalAllocatedMemoryLong(Resources.FindObjectsOfTypeAll<Texture>());
			var meshAllocated = GetTotalAllocatedMemoryLong(Resources.FindObjectsOfTypeAll<Mesh>());
			var materialAllocated = GetTotalAllocatedMemoryLong(Resources.FindObjectsOfTypeAll<Material>());

			// 출력.
			stringBuilder.AppendLine($"monoUsed: {ToMBString(monoUsed)}");
			stringBuilder.AppendLine($"monoHeap: {ToMBString(monoHeap)}");
			stringBuilder.AppendLine($"totalAllocated: {ToMBString(totalAllocated)}");
			stringBuilder.AppendLine($"totalReserved: {ToMBString(totalReserved)}");
			stringBuilder.AppendLine($"totalUnusedReserved: {ToMBString(totalUnusedReserved)}");
			stringBuilder.AppendLine($"textureAllocated: {ToMBString(textureAllocated)}");
			stringBuilder.AppendLine($"meshAllocated: {ToMBString(meshAllocated)}");
			stringBuilder.AppendLine($"materialAllocated: {ToMBString(materialAllocated)}");
		}

		/// <summary>
		/// 애셋들의 크기를 반환.
		/// </summary>
		public static long GetTotalAllocatedMemoryLong(Object[] objects)
		{
			var size = 0L;
			if (objects == null)
				return size;

			foreach (var obj in objects)
			{
				if (obj == null)
					continue;

				size += Profiler.GetRuntimeMemorySizeLong(obj);
			}

			return size;
		}

		/// <summary>
		/// 메가바이트 변환.
		/// </summary>
		public static string ToMBString(long bytes, int decimals = 2)
		{
			var mb = bytes / (1024.0 * 1024.0);
			var format = "F" + Mathf.Max(0, decimals);
			return mb.ToString(format, CultureInfo.InvariantCulture) + "MB";
		}
	}
}