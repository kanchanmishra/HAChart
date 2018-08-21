using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAPortable
{
    public class ChartData
    {
        public List<string> YValues { get; set; }
        public List<string> BarColors { get; set; }
        public List<string> BarText { get; set; }
        public List<string> XValues { get; set; }
        public List<string> Legends { get; set; }
        
        public float BaseValue { get; set; }
        public int Radius { get; set; }
        public int Percentage { get; set; }

        private string ErrorCountDontMatch = "The total number of XValues and Legends don't match";
        private string ErrorXValues = "The total number of XValues is zero";
        private string ErrorYValues = "The total number of YValues is zero";

        public bool DoXValuesExist()
        {
            if (XValues== null || XValues.Count == 0)
                throw new ChartException(ErrorXValues);
            else
                return true;
        }

        public bool DoYValuesExist()
        {
            if (YValues== null || YValues.Count == 0)
                throw new ChartException(ErrorYValues);
            else
                return true;

        }
        public bool AreXValAndLegendsCountSame()
        {
            if (Legends.Count != XValues.Count)
                throw new ChartException(ErrorCountDontMatch);
            else
                return true;

        }

        public bool ValidateAll()
        {
            bool result1 = false;
            bool result2 = false;
            bool result3 = false;
            try
            {
                result1 = DoXValuesExist();
                result2 = DoYValuesExist();
                result3 = AreXValAndLegendsCountSame();

            }
            catch (Exception e)
            {
                throw e;
            }
               if (result1 && result2 && result3)
                    return true;
                else
                    return false;
            

        }
    }
}
