namespace HPADotNetCore.ThemeMvcApp.Models
{
	public class ChartJsMultiAxisLineChartResponseModel
	{
		public int DataCount { get; set; }
		public List<string> Labels { get; set; }
		public List<ChartJsMultiAxisLineChartModel> Datasets { get; set; }
	}
}
