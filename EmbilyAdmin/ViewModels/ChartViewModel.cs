using System.Collections.Generic;

namespace EmbilyAdmin.ViewModels
{
    public class ChartViewModel
    {
        public List<int> Data { get; set; } = new List<int>();
        public string Label { get; set; } = "";
        public string yAxisID { get; set; } = "left-y-axis";      
    }
}