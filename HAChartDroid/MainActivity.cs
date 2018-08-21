using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using HAPortable;
using System;

namespace HAChartDroid
{
    [Activity(Label = "HAChartDroid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            TextView textView1 = FindViewById<TextView>(Resource.Id.TextView1);

            // draw bmi chart type 
            LineStripeChart lineStripeChart = FindViewById<LineStripeChart>(Resource.Id.HAChartDroid_LineStripeChart1);
            try
            {
                //lineStripeChart.SetChartType(ChartType.bmi.ToString());// set the  type to get the right data
                 lineStripeChart.SetChartType(ChartType.body_fat.ToString()); // for body_fat
            
                lineStripeChart.Init();
                lineStripeChart.Invalidate(); // Draw

            }
            catch (Exception e)
            {
                lineStripeChart.Visibility = ViewStates.Gone;
                textView1.Text = string.Format("Inadequate Chart Data: {0}", e.Message);
            }
            finally
            {
                
            }
        }
    }
}

