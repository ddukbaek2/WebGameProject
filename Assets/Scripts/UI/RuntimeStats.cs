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

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_LabelView = GetComponent<UILabelView>();
			m_StringBuilder = new StringBuilder();
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
			// managed heap(모노/IL2CPP 관리영역) 관련
			var monoUsed = Profiler.GetMonoUsedSizeLong();
			var monoHeap = Profiler.GetMonoHeapSizeLong();

			// Unity 네이티브 영역 추정(플랫폼에 따라 0/부정확 가능)
			var totalAllocated = Profiler.GetTotalAllocatedMemoryLong();
			var totalReserved = Profiler.GetTotalReservedMemoryLong();
			var totalUnusedReserved = Profiler.GetTotalUnusedReservedMemoryLong();

			// 그래픽 리소스 추정 (WebGL에서 의미 있게 나오는 편)
			var gfxDriver = Profiler.GetAllocatedMemoryForGraphicsDriver();

			// 출력.
			m_StringBuilder.Clear();
			m_StringBuilder.AppendLine($"monoUsed={ToMB(monoUsed)} monoHeap={ToMB(monoHeap)}");
			m_StringBuilder.AppendLine($"totalAllocated={ToMB(totalAllocated)} totalReserved={ToMB(totalReserved)} totalUnusedReserved={ToMB(totalUnusedReserved)}");
			m_StringBuilder.AppendLine($"gfxDriver={ToMB(gfxDriver)}");
			m_LabelView.text = m_StringBuilder.ToString();
		}

		/// <summary>
		/// 메가바이트 변환.
		/// </summary>
		public static string ToMB(long bytes, int decimals = 2)
		{
			var mb = bytes / (1024.0 * 1024.0);
			var format = "F" + Mathf.Max(0, decimals);
			return mb.ToString(format, CultureInfo.InvariantCulture) + "MB";
		}
	}
}