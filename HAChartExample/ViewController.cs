using System;
using CoreGraphics;
using UIKit;
using HAPortable;

namespace HAChartExample
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var lineStripeChart = new LineStripeChart_iOS(ChartView.Frame);
            //lineStripeChart.SetChartType(ChartType.bmi.ToString());
            lineStripeChart.SetChartType(ChartType.body_fat.ToString());
            ChartView.Add(lineStripeChart);
          
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


    }
}
