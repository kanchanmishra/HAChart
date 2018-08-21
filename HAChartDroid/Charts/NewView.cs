using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace HAChartDroid
{
    public class NewView : View
    {
        public NewView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public NewView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }

       
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)

        {

            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            SetMeasuredDimension(800, 800);

        }

        protected override void OnDraw(Canvas canvas)

        {

            base.OnDraw(canvas);

            Paint mBarPaint = new Paint();

            mBarPaint.SetStyle(Paint.Style.FillAndStroke);

            mBarPaint.AntiAlias = true;

            mBarPaint.Dither = true;

            mBarPaint.Color = Color.Aqua;



            canvas.DrawRect(0, 0, this.Width, this.Height, mBarPaint);

        }


    }
}