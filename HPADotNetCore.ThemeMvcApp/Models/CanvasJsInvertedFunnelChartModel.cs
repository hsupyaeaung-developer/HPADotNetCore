﻿namespace HPADotNetCore.ThemeMvcApp.Models
{
    public class CanvasJsInvertedFunnelChartModel
    {
        public List<DataPoint> DataPoints { get; set; }
    }

    public class DataPoint
    {
        public int Y { get; set; }
        public string Label { get; set; }
    }
}
