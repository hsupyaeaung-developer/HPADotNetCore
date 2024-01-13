using System.Collections.Generic;

namespace HPADotNetCore.ThemeMvcApp.Models
{
	public class CanvasJsSimpleLineChartResponseModel
	{
		public List<CanvasJsSimpleLineChartModel> DataPoints { get; set; }
		public string ChartTitle { get; set; }
		public string XAxisTitle { get; set; }
		public string YAxisTitle { get; set; }
	}
}
