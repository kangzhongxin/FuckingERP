namespace Kzx.UserControl.UITypeEdit
{
    partial class frmKzxChartControlXColumnsUiTypeEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgGrid = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.column = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columncaption = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isselected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dgGrid
            // 
            this.dgGrid.AllowUserToAddRows = false;
            this.dgGrid.AllowUserToDeleteRows = false;
            this.dgGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgGrid.BackgroundColor = System.Drawing.Color.White;
            this.dgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.column,
            this.columncaption,
            this.isselected});
            this.dgGrid.GridColor = System.Drawing.Color.Blue;
            this.dgGrid.Location = new System.Drawing.Point(12, 12);
            this.dgGrid.Name = "dgGrid";
            this.dgGrid.RowHeadersWidth = 24;
            this.dgGrid.RowTemplate.Height = 23;
            this.dgGrid.Size = new System.Drawing.Size(474, 378);
            this.dgGrid.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(496, 22);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(496, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAll.Location = new System.Drawing.Point(496, 98);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 23);
            this.btnClearAll.TabIndex = 3;
            this.btnClearAll.Text = "全清";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(496, 127);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // column
            // 
            this.column.DataPropertyName = "column";
            this.column.HeaderText = "列名";
            this.column.Name = "column";
            this.column.ReadOnly = true;
            // 
            // columncaption
            // 
            this.columncaption.DataPropertyName = "columncaption";
            this.columncaption.HeaderText = "列标题";
            this.columncaption.Name = "columncaption";
            this.columncaption.ReadOnly = true;
            this.columncaption.Width = 150;
            // 
            // isselected
            // 
            this.isselected.DataPropertyName = "isselected";
            this.isselected.FalseValue = "0";
            this.isselected.HeaderText = "选中";
            this.isselected.IndeterminateValue = "0";
            this.isselected.Name = "isselected";
            this.isselected.TrueValue = "1";
            this.isselected.Width = 150;
            // 
            // frmKzxChartControlXColumnsUiTypeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 402);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgGrid);
            this.MaximizeBox = false;
            this.Name = "frmKzxChartControlXColumnsUiTypeEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择列";
            this.Load += new System.EventHandler(this.frmKzxChartControlXColumnsUiTypeEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgGrid;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn column;
        private System.Windows.Forms.DataGridViewTextBoxColumn columncaption;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isselected;
    }
}