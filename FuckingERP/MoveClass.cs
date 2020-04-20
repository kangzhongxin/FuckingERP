using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms; 

namespace FuckingERP
{
    class MoveClass
    {
        #region public

        public const int MIN_SIZE = 10; //对控件缩放的最小值 
        public const int BOX_SIZE = 7;  //调整大小触模柄方框大小 

        public static bool IsCtrlKey = false;
        public static TextBox textbox;
        public static Control MControl = null;
        public static bool IsMouseDown = false;
        public static Point onPointClicked;
        public static Color BOX_COLOR = Color.White;
        public static Label[] lbl = new Label[8];
        public static int startl, startt, startw, starth;
        public static bool dragging;
        public static Point mouse_offset;
        public static Point original_pos;
        public static Cursor[] arrArrow = new Cursor[] {Cursors.SizeNWSE, Cursors.SizeNS, 
                                                    Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeNS, 
                                                    Cursors.SizeNESW, Cursors.SizeWE};

        #endregion

        #region 构造函数

        /// <summary> 
        /// 构造控件拖动对象 
        /// </summary> 
        /// <param name="moveControl">需要拖动的控件 </param> 
        public static void BarcodeControl(Control moveControl)
        {
            // 
            // TODO: 在此处添加构造函数逻辑 
            // 

            MControl = moveControl;
            MControl.MouseDown += Control_MouseDown;
            MControl.MouseUp += Control_MouseUp;
            MControl.MouseMove += Control_MouseMove;
            MControl.Click += Control_Click;
        }

        /// <summary> 
        /// 构造8个 调整大小触模柄 
        /// </summary> 
        public static void BarcodeCreate()
        {
            //构造8个调整大小触模柄 
            for (int i = 0; i < 8; i++)
            {
                lbl[i] = new Label();
                lbl[i].TabIndex = i;
                lbl[i].FlatStyle = 0;
                lbl[i].BorderStyle = BorderStyle.FixedSingle;
                lbl[i].BackColor = BOX_COLOR;
                lbl[i].Cursor = arrArrow[i];
                lbl[i].Name = "lbl" + i.ToString();
                lbl[i].Text = "";
                lbl[i].BringToFront();

                lbl[i].MouseDown += handle_MouseDown;
                lbl[i].MouseMove += handle_MouseMove;
                lbl[i].MouseUp += handle_MouseUp;
            }

            CreateTextBox();
            Create();
        }

        /// <summary> 
        /// 构造控件取消拖动对象 
        /// </summary> 
        /// <param name="moveControl">需要删除的控件 </param> 
        public static void BarcodeCancel(Control moveControl)
        {
            // 
            // TODO: 在此处删除构造函数逻辑 
            // 
            MControl = moveControl;
            MControl.MouseDown -= Control_MouseDown;
            MControl.MouseUp -= Control_MouseUp;
            MControl.MouseMove -= Control_MouseMove;
            MControl.Click -= Control_Click;

            for (int i = 0; i < 8; i++)
            {
                if (MControl.Parent == null)
                {
                    continue;
                }
                MControl.Parent.Controls.Remove(lbl[i]);
            }
        }

        /// <summary> 
        /// 删除textbox控件
        /// </summary> 
        public static void DelTextBox()
        {
            textbox.Parent.Controls.Remove(textbox);
        }

        #endregion

        #region 需拖动控件键盘事件

        public static void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //上下左右调节
            if (e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40)
            {
                if (e.KeyValue == 37)
                    MControl.Left -= 1;
                if (e.KeyValue == 38)
                    MControl.Top -= 1;
                if (e.KeyValue == 39)
                    MControl.Left += 1;
                if (e.KeyValue == 40)
                    MControl.Top += 1;
                MoveHandles();
                ControlLocality();
                MControl.Visible = true;
            } 
        }

        #endregion

        #region 需拖动控件鼠标事件

        public static void Control_Click(object sender, System.EventArgs e)
        {
            
            MControl = (sender as Control);
            for (int i = 0; i < 8; i++)
            {
                MControl.Parent.Controls.Add(lbl[i]);
                lbl[i].BringToFront();
            }
            textbox.Focus();
            MoveHandles();
             
        }

        public static void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = true;
            onPointClicked = new Point(e.X, e.Y);
            mouse_offset = new Point(-e.X, -e.Y);
            original_pos = ((Control)sender).Location; 
            HideHandles();

        }

        public static void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = false;
            MoveHandles();
            ShowHandles();
            MControl.Visible = true;

            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                //检查是否超出背景图片边界，超出则位置不变；在图片范围内，则控件位置改变。

                if (((Control)sender).Parent.PointToClient(mousePos).X >= 0
                    && (((Control)sender).Parent.PointToClient(mousePos).X <= ((Control)sender).Parent.Width)
                    && (((Control)sender).Parent.PointToClient(mousePos).Y >= 0
                    && ((Control)sender).Parent.PointToClient(mousePos).Y <= ((Control)sender).Parent.Height))
                {
                    ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos); 
                }
                else
                {
                    ((Control)sender).Location = original_pos;
                }
            }
        }

        public static void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ((Control)sender).Cursor = Cursors.Arrow;
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                ((Control)sender).Location = ((Control)sender).Parent.PointToClient(mousePos);
            }

        }

        #endregion
        #region 调整大小触模柄鼠标事件

        public static void handle_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startl = MControl.Left;
            startt = MControl.Top;
            startw = MControl.Width;
            starth = MControl.Height;
            HideHandles();
        }
        // 通过触模柄调整控件大小 
        //    0  1  2 
        //  7      3 
        //  6  5  4 
        public static void handle_MouseMove(object sender, MouseEventArgs e)
        {
            int l = MControl.Left;
            int w = MControl.Width;
            int t = MControl.Top;
            int h = MControl.Height;
            if (dragging)
            {
                switch (((Label)sender).TabIndex)
                {
                    //l算法：控件左边X坐标 ＋ 鼠标在触模柄X坐标 < 控件左边X坐标 ＋ 父控件宽度 - 控件大小 ？控件左边X坐标 ＋ 鼠标在触模柄X坐标 ：控件左边X坐标 ＋ 父控件宽度 - 控件大小  
                    //t算法： 
                    //w算法： 
                    //h算法： 
                    case 0: // _dragging top-left sizing box 
                        l = startl + e.X < startl + startw - MIN_SIZE ? startl + e.X : startl + startw - MIN_SIZE;
                        t = startt + e.Y < startt + starth - MIN_SIZE ? startt + e.Y : startt + starth - MIN_SIZE;
                        w = startl + startw - MControl.Left;
                        h = startt + starth - MControl.Top;
                        break;
                    case 1: // _dragging top-center sizing box 
                        t = startt + e.Y < startt + starth - MIN_SIZE ? startt + e.Y : startt + starth - MIN_SIZE;
                        h = startt + starth - MControl.Top;
                        break;
                    case 2: // _dragging top-right sizing box 
                        w = startw + e.X > MIN_SIZE ? startw + e.X : MIN_SIZE;
                        t = startt + e.Y < startt + starth - MIN_SIZE ? startt + e.Y : startt + starth - MIN_SIZE;
                        h = startt + starth - MControl.Top;
                        break;
                    case 3: // _dragging right-middle sizing box 
                        w = startw + e.X > MIN_SIZE ? startw + e.X : MIN_SIZE;
                        break;
                    case 4: // _dragging right-bottom sizing box 
                        w = startw + e.X > MIN_SIZE ? startw + e.X : MIN_SIZE;
                        h = starth + e.Y > MIN_SIZE ? starth + e.Y : MIN_SIZE;
                        break;
                    case 5: // _dragging center-bottom sizing box 
                        h = starth + e.Y > MIN_SIZE ? starth + e.Y : MIN_SIZE;
                        break;
                    case 6: // _dragging left-bottom sizing box 
                        l = startl + e.X < startl + startw - MIN_SIZE ? startl + e.X : startl + startw - MIN_SIZE;
                        w = startl + startw - MControl.Left;
                        h = starth + e.Y > MIN_SIZE ? starth + e.Y : MIN_SIZE;
                        break;
                    case 7: // _dragging left-middle sizing box 
                        l = startl + e.X < startl + startw - MIN_SIZE ? startl + e.X : startl + startw - MIN_SIZE;
                        w = startl + startw - MControl.Left;
                        break;
                }
                l = (l < 0) ? 0 : l;
                t = (t < 0) ? 0 : t;
                MControl.SetBounds(l, t, w, h);
            }
        }

        public static void handle_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            MoveHandles();
            ShowHandles();
        }

        #endregion

        #region public方法

        public static void Create()
        {
            for (int i = 0; i < 8; i++)
            {
                if (MControl.Parent==null)
                {
                    continue;
                }
                MControl.Parent.Controls.Add(lbl[i]);
                lbl[i].BringToFront();
            }
            HideHandles();
        }

        public static void CreateTextBox()
        {
            textbox = new TextBox();
            textbox.CreateControl();
            textbox.Parent = MControl.Parent;
            textbox.Width = 0;
            textbox.Height = 0;
            textbox.TabStop = true;
            textbox.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox_KeyDown);
        }

        public static void ControlLocality()
        {
            if (MControl.Location.X < 0)
            {
                MControl.Visible = false;
                MControl.Left = 0;
            }
            if (MControl.Location.Y < 0)
            {
                MControl.Visible = false;
                MControl.Top = 0;
            }
            if (MControl.Location.X + MControl.Width > MControl.Parent.Width)
            {
                MControl.Visible = false;
                MControl.Left = MControl.Parent.Width - MControl.Width;
            }
            if (MControl.Location.Y + MControl.Height > MControl.Parent.Height)
            {
                MControl.Visible = false;
                MControl.Top = MControl.Parent.Height - MControl.Height;
            }
        }

        public static void HideHandles()
        {
            for (int i = 0; i < 8; i++)
            {
                lbl[i].Visible = false;
            }
        }

        public static void MoveHandles()
        {
            int sX = MControl.Left - BOX_SIZE;
            int sY = MControl.Top - BOX_SIZE;
            int sW = MControl.Width + BOX_SIZE;
            int sH = MControl.Height + BOX_SIZE;
            int hB = BOX_SIZE / 2;
            int[] arrPosX = new int[] {sX+hB, sX + sW / 2, sX + sW-hB, sX + sW-hB, 
                                          sX + sW-hB, sX + sW / 2, sX+hB, sX+hB};
            int[] arrPosY = new int[] {sY+hB, sY+hB, sY+hB, sY + sH / 2, sY + sH-hB, 
                                          sY + sH-hB, sY + sH-hB, sY + sH / 2};
            for (int i = 0; i < 8; i++)
            {
                lbl[i].SetBounds(arrPosX[i], arrPosY[i], BOX_SIZE, BOX_SIZE);
            }
        }

        public static void ShowHandles()
        {
            if (MControl != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    lbl[i].Visible = true;
                }
            }
        }

        #endregion
    }
}
