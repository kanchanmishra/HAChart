using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Java.Lang;
using HAPortable;

namespace HAChartDroid
{
    public class LineStripeChart : View
    {
        string chartType = string.Empty;
        private List<RectF> barRect;
        private List<Rect> linesRect;

        private Path mLinePath;
        private Paint mLinePathPaint;
        private Paint mBarPaint;
        private Paint mLinePaint;
        private TextPaint mTextPaint;
        Rect rectGraph = new Rect();

        private float textSize12, textSize10, textSize8;
        private int mHorizontalLineStroke = 3;

        ChartData chartData = new ChartData();

        //private float baseValue = 0;
        //private int radius;
        //private List<string> barcolors = new List<string>();
        //private List<string> barText = new List<string>();
        //private List<string> xValues = new List<string>();
        //private List<string> legends = new List<string>();
        //private List<string> yValues = new List<string>();

        public void SetChartType(string chartType)
        {
            this.chartType = chartType;
        }

        public LineStripeChart(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public LineStripeChart(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);           
        }

        private void InitGraphics()
        {
            mBarPaint = new Paint();
            mBarPaint.SetStyle(Paint.Style.FillAndStroke);
            mBarPaint.AntiAlias = true;
            mBarPaint.Dither=true;

            mLinePaint = new Paint();
            mLinePaint.SetStyle(Paint.Style.Stroke);
            mLinePaint.AntiAlias=true;
            mLinePaint.Dither=true;

            barRect = new List<RectF>(chartData.BarColors.Count);
            for (int i = 0; i < chartData.BarColors.Count; i++)
            {
                barRect.Add(new RectF());
            }
           
            linesRect = new List<Rect>(chartData.XValues.Count);
            for (int i = 0; i < chartData.XValues.Count; i++)
            {
                linesRect.Add(new Rect(0, 0, 0, 0));
            }

            mLinePath = new Path();
            mLinePathPaint = new Paint();
            mLinePathPaint.Hinting = PaintHinting.Off;
            mLinePathPaint.SetStyle(Paint.Style.Stroke);
            mLinePathPaint.Color = Color.White;
            mLinePathPaint.StrokeWidth = 3;
            mLinePathPaint.AntiAlias =true;

            textSize12 = 12 * Resources.DisplayMetrics.Density;
            textSize10 = 10 * Resources.DisplayMetrics.Density;
            textSize8 = 8 * Resources.DisplayMetrics.Density;

        }

        public void Init()
        {
            
            try
            {
                if (!string.IsNullOrEmpty(chartType))
                {
                    // assign charts values
                    var chartValues = new ChartValues();
                    chartData = chartValues.SetChartValues(chartType);
                    //will throw an exception on validation
                    if (chartData.ValidateAll())
                    {
                        // Set up a default TextPaint object
                        mTextPaint = new TextPaint();

                        // Call InitGraphics
                        InitGraphics();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw  e;
            }

            
        }
        

        int GetQuadrant(float value)
        {
            
            int quadrant = 0;

            if (value > float.Parse(chartData.YValues[chartData.YValues.Count - 1]))
            {
                quadrant = chartData.YValues.Count - 1;                
            }
            else
            {
                for (int i = 0; i < chartData.YValues.Count; i++)
                {
                    if (value <= float.Parse(chartData.YValues[i]))
                    {
                        quadrant = i;
                        break;
                    }
                }
            }
            return quadrant;
        }
       
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            // call Draw Chart
            DrawChart(canvas);

        }

        void DrawChart(Canvas canvas)
        {
            if (barRect== null || barRect.Count == 0)
                return;

            try
            {

                var contentWidth = Width;
                var contentHeight = Height;

                // background color of the canvas 
                canvas.DrawColor(Color.White);

                mTextPaint.SetStyle(Paint.Style.Stroke);
                mTextPaint.AntiAlias = true;
                mTextPaint.TextAlign = Paint.Align.Left;
                mTextPaint.StrokeWidth = 0;
                mTextPaint.TextSize = textSize12;

                //get the text width
                string text = "0120";
                Rect bounds = new Rect();
                mTextPaint.GetTextBounds(text, 0, text.Length, bounds);
                int textWidth = bounds.Width();
                int textHeight = bounds.Height();

                float fcontentWidth = contentWidth - textWidth;
                float fcontentHeight = contentHeight - textHeight * 2;

                rectGraph.Top = textHeight + (chartData.Radius * 3) + 5;
                rectGraph.Left = textWidth * 2;
                rectGraph.Right = (int)fcontentWidth;
                rectGraph.Bottom = (int)fcontentHeight;

                canvas.DrawRect(rectGraph, mBarPaint);

                // vertical data  preparation
                int noOfBars = chartData.BarColors.Count;
                float boxHeight = rectGraph.Height() / noOfBars;

                float prevVal = chartData.BaseValue;
                float diffVal = float.Parse(chartData.YValues[noOfBars - 1]) - prevVal;

                float boxHeight1 = 0;

                mTextPaint.TextSize = textSize12;
                mTextPaint.Color = Color.White;
                mTextPaint.TextAlign = Paint.Align.Left;

                mLinePaint.Color = Color.White;
                mLinePaint.StrokeWidth = mHorizontalLineStroke;

                //Draw vertical data
                for (int i = 0; i < noOfBars; i++)
                {
                    //dynamic height

                    float cVal = float.Parse(chartData.YValues[i]) - prevVal;
                    boxHeight = (cVal * 100) / diffVal;
                    boxHeight = (rectGraph.Height() * boxHeight) / 100;
                    prevVal = float.Parse(chartData.YValues[i]);

                    mBarPaint.Color = Color.ParseColor(chartData.BarColors[i]);

                    barRect[i].Left = rectGraph.Left;
                    barRect[i].Top = rectGraph.Bottom - boxHeight1 - boxHeight;
                    barRect[i].Right = rectGraph.Right;
                    barRect[i].Bottom = rectGraph.Bottom - boxHeight1;

                    canvas.DrawRect(barRect[i], mBarPaint);

                    mTextPaint.TextSize = textSize12;
                    mTextPaint.Color = Color.White;
                    mTextPaint.TextAlign = Paint.Align.Left;

                    canvas.DrawText(chartData.BarText[i], barRect[i].Left + 20, barRect[i].Top + barRect[i].Height() / 2 + textHeight / 2, mTextPaint);
                    canvas.DrawLine(barRect[i].Left, barRect[i].Bottom, barRect[i].Right, barRect[i].Bottom, mLinePaint);

                    mTextPaint.TextSize = textSize10;
                    mTextPaint.Color = Color.Black;

                    //Draw Vertical texts
                    mTextPaint.TextAlign = Paint.Align.Right;
                    canvas.DrawText(chartData.YValues[i], barRect[i].Left - 10, barRect[i].Top + textHeight / 2, mTextPaint);

                    boxHeight1 += boxHeight;

                }

                canvas.DrawText("" + chartData.BaseValue, barRect[0].Left - 10, barRect[0].Bottom, mTextPaint);

                //409bd6 Circle Color
                mBarPaint.Color = Color.ParseColor("#409bd6");

                //Draw x & Y axis borders
                mTextPaint.StrokeWidth = 5;
                canvas.DrawLine(rectGraph.Left, rectGraph.Top, rectGraph.Left, rectGraph.Bottom, mTextPaint);
                canvas.DrawLine(rectGraph.Left, rectGraph.Bottom, rectGraph.Right, rectGraph.Bottom, mTextPaint);

                //Draw horizontal data
                int horizantalpadding = 20;

                mTextPaint.TextSize = textSize10;
                mTextPaint.Color = Color.Black;

                int pointdistance = (rectGraph.Width() - horizantalpadding * 2) / (chartData.XValues.Count + 1);

                for (int i = 0; i < chartData.XValues.Count; i++)
                {

                    mTextPaint.Color = Color.Black;
                    mTextPaint.StrokeWidth = 0;

                    float currentValue = float.Parse(chartData.XValues[i]);

                    int quadrant = GetQuadrant(currentValue);

                    float tempvalue = 0;
                    int percentage = 0;
                    float difference = 0;

                    if (quadrant > 0)
                    {
                        tempvalue = currentValue - float.Parse(chartData.YValues[quadrant - 1]);

                        difference = float.Parse(chartData.YValues[quadrant]) - float.Parse(chartData.YValues[quadrant - 1]);

                        percentage = (int)((tempvalue * 100) / (difference));
                    }
                    else
                    {
                        tempvalue = currentValue - chartData.BaseValue;

                        difference = float.Parse(chartData.YValues[quadrant]) - chartData.BaseValue;

                        percentage = (int)((tempvalue * 100) / (difference));
                    }

                    //Avoid drawing out of the bounds

                    if (percentage > 100)
                        percentage = percentage + (chartData.Radius);



                    //calculate box height
                    float ycenterpoint = 0;
                    if (barRect.Count-1 >= i)
                         ycenterpoint = (barRect[i].Height() * percentage) / 100;
                    else
                        ycenterpoint = (barRect[i-1].Height() * percentage) / 100;



                    mTextPaint.TextAlign = Paint.Align.Center;

                    //draw x legends
                    mTextPaint.TextSize = textSize10;
                    mTextPaint.GetTextBounds(chartData.Legends[i], 0, chartData.Legends[i].Length, bounds);
                    textHeight = bounds.Height();
                    canvas.DrawText(chartData.Legends[i], (pointdistance * (i + 2)), rectGraph.Bottom + textHeight + 10, mTextPaint);


                    // draw vertical Lines
                    mTextPaint.Color = (Color.DarkGray);
                    mTextPaint.TextAlign = (Paint.Align.Center);
                    mTextPaint.StrokeWidth = 1;
                    linesRect[i].Left = (pointdistance * (i + 2));
                    // modify the top value if the last value is beyond last stripe 
                    linesRect[i].Top = (int)(barRect[quadrant].Bottom - ycenterpoint);
                    canvas.DrawLine(linesRect[i].Left, linesRect[i].Top, linesRect[i].Left, rectGraph.Bottom, mTextPaint);

                    //draw lines between points
                    mTextPaint.Color = (Color.Aqua);
                    mTextPaint.StrokeWidth = 2;
                    mTextPaint.SetStyle(Paint.Style.Stroke);

                    if (i == 0)
                        mLinePath.MoveTo((pointdistance * (i + 2)), linesRect[i].Top);
                    else
                        mLinePath.LineTo((pointdistance * (i + 2)), linesRect[i].Top);
                }

                canvas.DrawPath(mLinePath, mLinePathPaint);


                // plot X  values and draw Circle for them
                for (int i = 0; i < linesRect.Count; i++)
                {
                    // draw Inner circle
                    canvas.DrawCircle(linesRect[i].Left, linesRect[i].Top, chartData.Radius, mBarPaint);

                    // draw outer circle
                    mTextPaint.Color = (Color.Black);
                    canvas.DrawCircle(linesRect[i].Left, linesRect[i].Top, chartData.Radius, mTextPaint);

                    // draw x values
                    mTextPaint.Color = (Color.White);
                    mTextPaint.TextSize = textSize8;
                    canvas.DrawText(chartData.XValues[i], linesRect[i].Left, linesRect[i].Top + textHeight / 2, mTextPaint);

                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Withdrawal failed", e);
            }
        }

        #region Animate Line

        /*
        public void initAnim()
        {
            if (linesRect == null)
                return;

            if (linesRect.Count > 0)
            {

                //Log.d("deepen length = ",""+linesRect.length);

                mLinePath.Reset();
                mLinePath.MoveTo(linesRect[0].Left, linesRect[0].Top);
                mLinePath.LineTo(linesRect[1].Left, linesRect[1].Top);

                for (int i = 1; i < linesRect.Count; i++)
                {
                    mLinePath.LineTo(linesRect[i].Left, linesRect[i].Top);

                }
            }


            PathMeasure measure = new PathMeasure(mLinePath, false);

            measureLength = measure.Length;

            float[] intervals = new float[] { measureLength, measureLength };

            ObjectAnimator animator = ObjectAnimator.OfFloat(this, "phase", 1.0f, 0.0f);
            animator.SetDuration(2500);
            //animator.setInterpolator(new DecelerateInterpolator());
            animator.Start();
        }

        //is called by animtor object
        public void setPhase(float phase)
        {
            //Log.d("pathview","setPhase called with:" + String.valueOf(phase));
            mLinePathPaint.SetPathEffect(createPathEffect(measureLength, phase, 0.0f));
            Invalidate();//will calll onDraw
        }

        private static PathEffect createPathEffect(float pathLength, float phase, float offset)
        {
            return new DashPathEffect(new float[] { pathLength, pathLength },
                    System.Math.Max(phase * pathLength, offset));
        }
        */
        #endregion

    }
}
