using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Kzx.UserControl
{
    public partial class KzxLine : KzxBaseControl
    {
        private Color lineColor = Color.Black;
        //private int lineHeight = 1;

        public KzxLine()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 线条的宽度
        /// </summary>
        /// <return>void</return>
        private int LWidth = 1;
        [Category("自定义"), Description("lineWidth,线条的宽度"), Browsable(true)]
        [McDisplayName("lineWidth")]
        public int lineWidth
        {
            get
            {
                return this.LWidth;
            }
            set
            {

                this.LWidth = value;
            }
        }

        /// <summary>
        /// 列类型
        /// </summary>
        public enum ArrowType
        {
            None = 0,
            Start = 1,
            End = 2,
            All = 3
        }

        /// <summary>
        /// 箭头的方向
        /// </summary>
        /// <return>void</return>
        private ArrowType ArrowP = ArrowType.None;
        [Category("自定义"), Description("ArrowPosition,箭头所属"), Browsable(true)]
        [McDisplayName("ArrowType")]
        public ArrowType ArrowPosition
        {
            get
            {
                return this.ArrowP;
            }
            set
            {

                this.ArrowP = value;
            }
        }

        /// <summary>
        /// 直线的类型 //实线  虚线
        /// </summary>
        /// <return>void</return>
        private Boolean Solid = true;
        [Category("自定义"), Description("IsSolid,是否是实线"), Browsable(true)]
        [McDisplayName("IsSolid")]
        public Boolean IsSolid
        {
            get
            {
                return this.Solid;
            }
            set
            {

                this.Solid = value;
            }
        }

        /// <summary>
        /// 直线的方向 //横线  竖线
        /// </summary>
        public enum LineType
        {
            Horizontal = 0,
            Vertical = 1
        }

        /// <summary>
        /// 直线的类型 //横线  竖线
        /// </summary>
        /// <return>void</return>
        private LineType sTyle = LineType.Horizontal;
        [Category("自定义"), Description("LineStyle,Horizontal(横线)  Vertical(竖线)"), Browsable(true)]
        [McDisplayName("LineStyle")]
        public LineType LineStyle
        {
            get
            {
                return this.sTyle;
            }
            set
            {

                this.sTyle = value;
            }
        }

        /// <summary>
        /// 线条的颜色 
        /// </summary>
        public enum ColorType
        {
            Black = 0,
            Red = 1,
            Yellow = 2,
            Blue = 3,
            Green = 4,
            Lime = 5
        }

        /// <summary>
        /// 线条的颜色 
        /// </summary>
        /// <return>void</return>
        private ColorType color = ColorType.Black;
        [Category("自定义"), Description("LineColor,线条颜色"), Browsable(true)]
        [McDisplayName("LineColor")]
        public ColorType LineColor
        {
            get
            {
                return this.color;
            }
            set
            {

                this.color = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //e.Graphics.DrawLine(new Pen(lineColor, lineHeight), 1, 1, this.Width, lineHeight);

            AdjustableArrowCap lineCap = new AdjustableArrowCap(5, 5, true);
            Pen p = new Pen(lineColor, 1);

            string sColor = "";
            if ((int)color == 0) 
            {
                sColor = "Black";
            }
            if ((int)color == 1)
            {
                sColor = "Red";
            }
            if ((int)color == 2)
            {
                sColor = "Yellow";
            }
            if ((int)color == 3)
            {
                sColor = "Blue";
            }
            if ((int)color == 4)
            {
                sColor = "Green";
            }
            if ((int)color == 5)
            {
                sColor = "Lime";
            }
           
            p.Color = Color.FromName(sColor);
            p.Width = LWidth;

            if ((int)ArrowP == 1)
            {
                p.CustomStartCap = lineCap;
            }
            else
                if ((int)ArrowP == 2)
                {
                    p.CustomEndCap = lineCap;
                }
                else
                    if ((int)ArrowP == 3)
                    {
                        p.CustomStartCap = lineCap;
                        p.CustomEndCap = lineCap;
                    }

            if (!Solid)
            {
                float[] dashValues = { 5, 2, 5, 2 };
                p.DashPattern = dashValues;
            }

            int iLeft = 1;
            int iTop = 1;
            int iWidth = 1;
            int iHeight = 1;

            if ((int)sTyle == 0)
            {
                iLeft = 1;
                iTop = LWidth * 2 + 5;
                iWidth = this.Width;
                iHeight = LWidth * 2 + 5;
            }
            else
            {
                iLeft = LWidth * 2 + 5;
                iTop = 1;
                iWidth = LWidth * 2 + 5;
                iHeight = this.Height;
            }
            e.Graphics.DrawLine(p, iLeft, iTop, iWidth, iHeight);
            p.Dispose();
        }
    }
}
