using Microsoft.VisualBasic;

namespace HPADotNetCore.ThemeMvcApp.Models
{
    public class ChartJsTimeScaleModel
    {
        public List<string> labels { get; set; }
        public double[][] number { get; set; }
    }
}
