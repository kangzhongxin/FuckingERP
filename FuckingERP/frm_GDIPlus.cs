using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace FuckingERP
{
    public partial class frm_GDIPlus : DockContent
    {
        //背景网格属性初始化
        Graphics m_picture;
        int Row = 20;
        int colums = 20;
 

        //用于存储需要直线连接的元素
        List<Tuple<Label, Label>> lines = new List<Tuple<Label, Label>>();
        private static frm_GDIPlus fInstance;
        public frm_GDIPlus()
        {
            InitializeComponent();
            //初始化画布大小
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            this.FormClosed += Frm_GDIPlus_FormClosed;
            //画布绘制俩元素之间的直线
            this.pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Tuple<Label, Label> t in lines)
            {
                Point p1 = new Point(t.Item1.Left + t.Item1.Width / 2,
                                     t.Item1.Top + t.Item1.Height / 2);
                Point p2 = new Point(t.Item2.Left + t.Item2.Width / 2,
                                     t.Item2.Top + t.Item2.Height / 2);

                e.Graphics.DrawLine(Pens.Black, p1, p2);
            }
        }

        public static frm_GDIPlus GetInstance()
        {
            if (fInstance == null)
            {
                fInstance = new frm_GDIPlus();
            }
            return fInstance;
        }
        private void Frm_GDIPlus_FormClosed(object sender, FormClosedEventArgs e)
        {
            fInstance = null;
        }

        private Point fPoint;
        //俩元素上下间隔
        private int iPosition = 0;
        /// <summary>
        /// 绘制元素，此处以Label为例
        /// </summary>
        /// <returns></returns>
        private Label createBlock(string lblName)
        {
            try
            {
                Label label = new Label();
                label.AutoSize = false;
                //TODO：如需动态生成每个标签元素位置，请根据实际情况，初始化标签的Location即可。此处默认X=150，Y 以75间隔递增
                label.Location = new Point(150, iPosition); 
                iPosition = iPosition + 75;
                label.Size = new Size(89, 36);
                label.BackColor = Color.DarkOliveGreen;
                label.ForeColor = Color.Black;
                label.FlatStyle = FlatStyle.Flat;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Text = lblName;
                //TODO;可以绑定标签元素的右键事件
                //label.ContextMenuStrip = contextBlock;
                pictureBox1.Controls.Add(label);
                //拖拽移动
                MoveBlock(label);
                return label;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 初始化网格
        /// </summary>
        private void InitGridLine()
        {
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.Focus();
            m_picture = pictureBox1.CreateGraphics();
            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gp = Graphics.FromImage(canvas);
            DrawGrid(gp);
            pictureBox1.BackgroundImage = canvas;
            pictureBox1.Refresh();
        }
        //绘制网格
        private void DrawGrid(Graphics gp)
        {
            for (int i = 0; i < Row; i++)
            {
                gp.DrawLine(new Pen(Color.LightCyan), (i + 1) * pictureBox1.Width / Row, 0, (i + 1) * pictureBox1.Width / Row, pictureBox1.Height);
            }
            for (int i = 0; i < colums; i++)
            {
                gp.DrawLine(new Pen(Color.LightCyan), 0, (i + 1) * pictureBox1.Height / colums, pictureBox1.Width, (i + 1) * pictureBox1.Height / colums);
            }
        }

      
        //初始化
        private void frm_GDIPlus_Load(object sender, EventArgs e)
        {
            InitGridLine(); 
            iPosition = 0;
            Label l1 = createBlock("标签1");
            Label l2 = createBlock("标签2");
            Label l3 = createBlock("标签3");
            Label l4 = createBlock("标签4");
            Label l5 = createBlock("标签5");
            //标签1连接标签2和3
            lines.Add(new Tuple<Label, Label>(l1, l2));
            lines.Add(new Tuple<Label, Label>(l1, l3));
            //标签3和5连接标签4
            lines.Add(new Tuple<Label, Label>(l3, l4));
            lines.Add(new Tuple<Label, Label>(l5, l4));
        }
        //标签移动效果
        private void MoveBlock(Label block, Label endBlock = null)
        {
            block.MouseDown += (ss, ee) =>
            {
                if (ee.Button == System.Windows.Forms.MouseButtons.Left)
                    fPoint = Control.MousePosition;
            };
            block.MouseMove += (ss, ee) =>
            {
                if (ee.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point temp = Control.MousePosition;
                    Point res = new Point(fPoint.X - temp.X, fPoint.Y - temp.Y);

                    block.Location = new Point(block.Location.X - res.X,
                                               block.Location.Y - res.Y);
                    fPoint = temp;
                    pictureBox1.Invalidate();   // <------- draw the new lines
                }
            };
        }
    }
}
