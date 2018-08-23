using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using HAPortable;

namespace HAChartExample
{
    public class LineStripeChart_iOS : UIView
    {

        string chartType = string.Empty;
        List<string> barcolors;
        List<string> barText;
        List<string> xValues;
        List<string> legends;
        List<string> yValues;

        List<CGRect> linesRect;
        List<CGRect> barRect;
        List<CGPoint> linesPoint;

        UIFont font1, font2;
        int circleRadius;

        int percentage;
        float baseValue = 0;

        CGRect BaseFrame;

        public void SetChartType(string chartType)
        {
            this.chartType = chartType;
        }

        public LineStripeChart_iOS(CGRect frame) : base(frame)
        {

            BaseFrame = frame;
            // InitializeGraphValue();
            this.BackgroundColor = UIColor.LightGray;
        }



        public void InitializeGraphValue()
        {

            try
            {
                if (!string.IsNullOrEmpty(chartType))
                {
                    // assign charts values
                    var chartValues = new ChartValues();
                    var chartData = chartValues.SetChartValues(chartType);
                    //will throw an exception on validation
                    if (chartData.ValidateAll())
                    {
                        // assign values

                        barcolors = chartData.BarColors;
                        barText = chartData.BarText;
                        xValues = chartData.XValues;
                        legends = chartData.Legends;
                        yValues = chartData.YValues;
                        circleRadius = chartData.Radius;
                        baseValue = chartData.BaseValue;
                        percentage = chartData.Percentage;

                        linesRect = new List<CGRect>(xValues.Capacity);
                        linesPoint = new List<CGPoint>(xValues.Capacity);
                        barRect = new List<CGRect>(barcolors.Capacity);

                        font1 = UIFont.SystemFontOfSize(16);
                        font2 = UIFont.SystemFontOfSize(11);

                        //Commented as we are not using Animation for now
                        // PerformSelector(new ObjCRuntime.Selector("Animateline"), null, 1.0);

                    }
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }
        public UIColor colorWithHexString(string hexString)
        {
            string colorString = hexString.Replace("#", "").ToUpper();
            UIColor newColor = null;
            try
            {
                float red, green, blue;
                float alpha = 1.0f;

                switch (colorString.Length)
                {
                    case 3: // #RGB
                        {
                            red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                            green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                            blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                            newColor = UIColor.FromRGBA(red, green, blue, alpha);
                            break;
                        }
                    case 4:            // #ARGB
                        alpha = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                        red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(3, 1)), 16) / 255f;
                        newColor = UIColor.FromRGBA(red, green, blue, alpha);
                        break;
                    case 6: // #RRGGBB
                        {
                            red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                            green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                            blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                            newColor = UIColor.FromRGBA(red, green, blue, alpha);
                            break;
                        }
                    case 8: // #RRGGBBAA
                        {
                            red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                            green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                            blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                            alpha = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;

                            newColor = UIColor.FromRGBA(red, green, blue, alpha);
                            break;
                        }

                    default:
                        throw new ArgumentOutOfRangeException(string.Format("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexString));

                }
            }
            catch (Exception genEx)
            {
                newColor = null;
            }
            return newColor;

        }

        public int getQuadrant(float value)
        {
            int quadrant = 0;

            if (value > (float.Parse(yValues[yValues.Count - 1])))
            {
                quadrant = yValues.Count - 1;
            }
            else
            {
                for (int i = 0; i < yValues.Count; i++)
                {
                    if (value < float.Parse(yValues[i]))
                    {
                        quadrant = i;
                        break;
                    }
                }
            }
            return quadrant;
        }


        // Only override drawRect: if you perform custom drawing.
        // An empty implementation adversely affects performance during animation.
        public override void Draw(CGRect rect)
        {
            if (barRect == null || barRect.Capacity == 0)
                return;

            try
            {

                int padding = 0;
                int contentWidth = int.Parse(rect.Size.Width.ToString()) - padding * 2;
                int contentHeight = int.Parse(rect.Size.Height.ToString()) - padding * 2;

                float boxHeight = 0;

                int noOfBars = barcolors.Count;

                int textWidth = 20;   //get the height dynamically
                int textHeight = 0;  //get the height dynamically


                float fcontentWidth = contentWidth - textWidth;
                float fcontentHeight = contentHeight - textHeight * 2;

                float left = padding + textWidth * 2;
                float top = padding * 2 + textHeight + (circleRadius * 3) + 5;
                float right = fcontentWidth - left;
                float bottom = fcontentHeight - top;

                right = fcontentWidth;
                //bottom = fcontentHeight;

                CGRect rectGraph = CGRect.FromLTRB(left, top, right, bottom);


                CGContext context = UIGraphics.GetCurrentContext();  

                context.SetFillColor(UIColor.Green.CGColor);
                context.FillRect(rectGraph);

                noOfBars = barcolors.Count;

                float prevVal = baseValue;
                float diffVal = float.Parse(yValues[noOfBars - 1]) - prevVal;

                float boxHeight1 = 0;

                barRect.Clear();
                linesPoint.Clear();

                // draw bars/Strips
                for (int i = 0; i < noOfBars; i++)
                {
                    float cVal = float.Parse(yValues[i]) - prevVal;

                    boxHeight = (cVal * 100) / diffVal;
                    boxHeight = (float.Parse(rectGraph.Size.Height.ToString()) * boxHeight) / 100;

                    prevVal = float.Parse(yValues[i]);

                    context.SetFillColor(this.colorWithHexString(barcolors[i]).CGColor);                    

                    CGRect tempBarRect = new CGRect(rectGraph.X, (rectGraph.Y + rectGraph.Size.Height) - boxHeight1 - boxHeight, rectGraph.Size.Width, boxHeight);

                    barRect.Add(tempBarRect);
                   
                    context.FillRect(tempBarRect);


                    string text1 = barText[i];

                    CGSize size1 = text1.StringSize(font1);

                    CGRect r1 = new CGRect(tempBarRect.X + 10, tempBarRect.Y + tempBarRect.Size.Height / 2 - size1.Height / 2, size1.Width, size1.Height);

                    context.SetFillColor(UIColor.White.CGColor);
                    text1.DrawString(r1, font1, UILineBreakMode.Clip, UITextAlignment.Left);

                   
                    r1 = new CGRect(tempBarRect.X, tempBarRect.Y, tempBarRect.Size.Width, 2);

                    context.FillRect(r1);
                    //Draw y values
                    text1 = yValues[i];
                    size1 = text1.StringSize(font1);

                    r1 = new CGRect(tempBarRect.X - size1.Width - 5, tempBarRect.Y - size1.Height / 2, size1.Width, size1.Height);
                    context.SetFillColor(UIColor.Black.CGColor);
                    text1.DrawString(r1, font1, UILineBreakMode.Clip, UITextAlignment.Right);

                    boxHeight1 += boxHeight;
                }
                //Draw bottom text

                string text = baseValue.ToString();

                CGSize size = text.StringSize(font1);

                CGRect r = new CGRect(rectGraph.X - size.Width - 5, rectGraph.Y + rectGraph.Size.Height - size.Height, size.Width, size.Height);

                text.DrawString(r, font1, UILineBreakMode.Clip, UITextAlignment.Right);




                //Draw x & y axis border
                r = new CGRect(rectGraph.X, rectGraph.Y, 2, rectGraph.Size.Height);
                context.FillRect(r);

                r = new CGRect(rectGraph.X, rectGraph.Y + rectGraph.Size.Height - 2, rectGraph.Size.Width, 2);
                context.FillRect(r);

                int horizontalpadding = 20;

                int pointdistance = (int)(rectGraph.Size.Width - horizontalpadding * 2) / (xValues.Count + 1);



                CGPoint lastPoint = new CGPoint();

                linesRect.Clear();
                var drawLines = new List<CGPoint>();
                // draw Line and x axis
                for (int i = 0; i < xValues.Count; i++)
                {
                    context.SetFillColor(UIColor.Black.CGColor);

                    float currentValue = float.Parse(xValues[i]);

                    int quadrant = this.getQuadrant(currentValue);

                    float tempvalue = 0;
                    int percentage = 0;
                    float difference = 0;

                    if (quadrant > 0)
                    {
                        tempvalue = currentValue - float.Parse(yValues[quadrant - 1]);
                        difference = float.Parse(yValues[quadrant]) - float.Parse(yValues[quadrant - 1]);
                        percentage = (int)((tempvalue * 100) / difference);

                    }
                    else
                    {
                        tempvalue = currentValue - baseValue;
                        difference = float.Parse(yValues[quadrant]) - baseValue;
                        percentage = (int)((tempvalue * 100) / difference);

                    }

                    if (percentage > 100)
                        percentage = 100 + circleRadius;
                    

                    float ycenterpoint = (boxHeight * percentage) / 100;

                    CGRect someRect = new CGRect(barRect[quadrant].X, barRect[quadrant].Y, barRect[quadrant].Width, barRect[quadrant].Height);

                    r = new CGRect(pointdistance * (i + 2), someRect.Y + someRect.Size.Height - ycenterpoint, 2, ycenterpoint);

                    context.SetFillColor(UIColor.LightGray.CGColor);
                    context.SetStrokeColor(0, 0, 0, 0.5f);
                    context.SetLineWidth(1);
                    context.BeginPath();
                    context.MoveTo(pointdistance * (i + 2), someRect.Y + someRect.Size.Height - ycenterpoint);
                    context.AddLineToPoint(pointdistance * (i + 2), rectGraph.Y + rectGraph.Size.Height);


                    context.StrokePath();


                    if (i == 0)
                    {
                        lastPoint = new CGPoint(pointdistance * (i + 2), someRect.Y + someRect.Size.Height - ycenterpoint);
                    }
                    else
                    {

                        context.SetStrokeColor(UIColor.White.CGColor);
                        context.SetFillColor(UIColor.White.CGColor);
                        context.BeginPath();
                        context.MoveTo(lastPoint.X, lastPoint.Y);
                        context.AddLineToPoint(pointdistance * (i + 2), someRect.Y + someRect.Size.Height - ycenterpoint);
                        context.StrokePath();

                        lastPoint = new CGPoint(pointdistance * (i + 2), someRect.Y + someRect.Size.Height - ycenterpoint);

                    }

                    linesPoint.Add(lastPoint);
                    linesRect.Add(new CGRect(pointdistance * (i + 2) - circleRadius / 2, someRect.Y + someRect.Size.Height - ycenterpoint - circleRadius / 2, circleRadius, circleRadius));


                }

                // draw legend
                context.BeginPath();
                context.SetFillColor(colorWithHexString("#409bd6").CGColor);
                context.SetLineWidth(2);
                context.SetStrokeColor(1.0f, 1.0f, 1.0f, 1.0f);

                for (int i = 0; i < xValues.Count; i++)
                {

                    CGRect someRect = linesRect[i];
                    context.FillEllipseInRect(someRect);
                    context.StrokeEllipseInRect(someRect);

                }

                context.StrokePath();
                context.SetFillColor(UIColor.Black.CGColor);
                context.FillRect(new CGRect(rectGraph.X, rectGraph.Y + 2, 2, rectGraph.Size.Height - 2));
                context.FillRect(new CGRect(rectGraph.X, rectGraph.Y + rectGraph.Size.Height - 2, rectGraph.Size.Width, 2));
                context.SetFillColor(UIColor.Black.CGColor);

                // Draw X values
                for (int i = 0; i < xValues.Count; i++)
                {

                    context.SetFillColor(UIColor.White.CGColor);

                    CGRect rect1 = linesRect[i];

                    string text1 = xValues[i];
                    CGSize size1 = text1.StringSize(font2);

                    CGRect r1 = new CGRect(rect1.X, rect1.Y + (rect1.Size.Height - size1.Height) / 2, rect1.Size.Width, size1.Height);
                    text1.DrawString(r1, font2, UILineBreakMode.Clip, UITextAlignment.Center);

                    text1 = legends[i];
                    size1 = text1.StringSize(font1);
                    r1 = new CGRect((rect1.X + circleRadius / 2) - size1.Width / 2, rectGraph.Y + rectGraph.Size.Height, size1.Width, size1.Height);
                    context.SetFillColor(UIColor.Black.CGColor);
                    text1.DrawString(r1, font1, UILineBreakMode.Clip, UITextAlignment.Center);


                }

            }
            catch (System.Exception e)
            {
                throw new System.Exception("Draw failed", e);
            }
        }


        #region Animate Line

        /*
        [Export("Animateline")]
        public void Animateline()
        {
            UIBezierPath path = new UIBezierPath();
            if (linesPoint.Count > 1)
            {

                CGPoint somepoint = new CGPoint(linesPoint[0]);
                //[path moveToPoint:somepoint];

                path.MoveTo(somepoint);
                for (int i = 1; i < linesPoint.Count; i++)
                {
                    CGPoint somepoint1 = new CGPoint(linesPoint[i]);

                    path.AddLineTo(somepoint1);
                    //[path addLineToPoint:somepoint1];
                }

            }


            CAShapeLayer pathLayer = new CAShapeLayer();
            pathLayer.Frame = this.Bounds; 
            pathLayer.Path = path.CGPath;
            pathLayer.StrokeColor = UIColor.White.CGColor;
            pathLayer.FillColor = UIColor.Clear.CGColor;
            pathLayer.LineWidth = 1.5f;
            pathLayer.LineJoin = new NSString(CGLineJoin.Bevel.ToString()); // kCALineJoinBevel;
           
            //[self.layer addSublayer:pathLayer];
            this.Layer.AddSublayer(pathLayer);

            CABasicAnimation pathAnimation = new CABasicAnimation();//[CABasicAnimation animationWithKeyPath: @"strokeEnd"];
            pathAnimation.Duration = 1.0;
            pathAnimation.From = (NSNumber)0.01f;
            pathAnimation.To = (NSNumber)1.0f;
          
            pathLayer.AddAnimation(pathAnimation, "strokeEnd");

            for (int i = 0; i < xValues.Count; i++)
            {
                CGRect someRect = linesRect[i];
                CAShapeLayer circleLayer = new CAShapeLayer();
                // [circleLayer setPath:[[UIBezierPath bezierPathWithOvalInRect:someRect] CGPath]];
                var b = UIBezierPath.FromOval(someRect);//  bezierPathWithOvalInRect;

                circleLayer.Path = b.CGPath;
                circleLayer.StrokeColor = UIColor.White.CGColor;
                circleLayer.FillColor = colorWithHexString("#409bd6").CGColor;
                circleLayer.LineWidth = 1.5f;

                // [self.layer addSublayer:circleLayer];

                this.Layer.AddSublayer(circleLayer);

                string text = xValues[i].ToString();
                CGSize size = new CGSize(11, 11);  //; font2;

                CATextLayer TextLayer = new CATextLayer();//[CATextLayer layer];
                someRect.Y = someRect.Y + (size.Height / 2);
                TextLayer.Frame = someRect;
                TextLayer.String = text;
                TextLayer.FontSize = 12;
                TextLayer.ForegroundColor = UIColor.White.CGColor;
                TextLayer.Wrapped = false;
                //        TextLayer.position = CGPointMake(someRect.origin.x,
                //                                         someRect.origin.y + (someRect.size.height -
                //                                                          size.height) / 2);
                TextLayer.TextAlignmentMode = CATextLayerAlignmentMode.Center; //kCAAlignmentCenter;

                this.Layer.AddSublayer(TextLayer);
                //[self.layer addSublayer:TextLayer];


            }

        }
        */
        #endregion
    }
}




