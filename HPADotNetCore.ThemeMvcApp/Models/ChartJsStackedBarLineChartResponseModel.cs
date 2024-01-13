namespace HPADotNetCore.ThemeMvcApp.Models
{
    public class ChartJsStackedBarLineChartResponseModel
    {
        public int DataCount { get; set; }

        public List<string> Labels { get; set; }

        public List<ChartJsStackedBarLineChartModel> DataSets { get; set; }

    }

}
