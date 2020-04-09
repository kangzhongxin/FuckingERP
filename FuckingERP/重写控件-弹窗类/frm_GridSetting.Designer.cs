namespace FuckingERP.重写控件_弹窗类
{
    partial class frm_GridSetting
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.kzxPanel1 = new Kzx.UserControl.KzxPanel();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.kzxPanel2 = new Kzx.UserControl.KzxPanel();
            this.gcSetting = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColumnsName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.isDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Freeze = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.ColumnsWidth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Order1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.Order2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.Top = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.Button = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.iOrder = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.kzxPanel1)).BeginInit();
            this.kzxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kzxPanel2)).BeginInit();
            this.kzxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // kzxPanel1
            // 
            this.kzxPanel1.Controls.Add(this.btnCancle);
            this.kzxPanel1.Controls.Add(this.btnSure);
            this.kzxPanel1.DesigeCaption = "显示标题";
            this.kzxPanel1.DesigeEnabled = true;
            this.kzxPanel1.DesigeVisible = true;
            this.kzxPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kzxPanel1.Key = "kzxPanel1";
            this.kzxPanel1.KzxBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxPanel1.LayoutColumn = 0;
            this.kzxPanel1.LayoutColumnSpan = 1;
            this.kzxPanel1.LayoutRow = 0;
            this.kzxPanel1.LayoutRowSpan = 1;
            this.kzxPanel1.Location = new System.Drawing.Point(0, 276);
            this.kzxPanel1.MessageCode = "0";
            this.kzxPanel1.Name = "kzxPanel1";
            this.kzxPanel1.Size = new System.Drawing.Size(511, 34);
            this.kzxPanel1.TabIndex = 0;
            this.kzxPanel1.ToolTipMessageCode = "";
            this.kzxPanel1.ToolTipText = "";
            // 
            // btnCancle
            // 
            this.btnCancle.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCancle.ForeColor = System.Drawing.Color.Black;
            this.btnCancle.Location = new System.Drawing.Point(432, -1);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(77, 34);
            this.btnCancle.TabIndex = 0;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = false;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnSure
            // 
            this.btnSure.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSure.ForeColor = System.Drawing.Color.Black;
            this.btnSure.Location = new System.Drawing.Point(349, -1);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(77, 34);
            this.btnSure.TabIndex = 0;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = false;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // kzxPanel2
            // 
            this.kzxPanel2.Controls.Add(this.gcSetting);
            this.kzxPanel2.DesigeCaption = "显示标题";
            this.kzxPanel2.DesigeEnabled = true;
            this.kzxPanel2.DesigeVisible = true;
            this.kzxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kzxPanel2.Key = "kzxPanel2";
            this.kzxPanel2.KzxBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxPanel2.LayoutColumn = 0;
            this.kzxPanel2.LayoutColumnSpan = 1;
            this.kzxPanel2.LayoutRow = 0;
            this.kzxPanel2.LayoutRowSpan = 1;
            this.kzxPanel2.Location = new System.Drawing.Point(0, 0);
            this.kzxPanel2.MessageCode = "0";
            this.kzxPanel2.Name = "kzxPanel2";
            this.kzxPanel2.Size = new System.Drawing.Size(511, 276);
            this.kzxPanel2.TabIndex = 1;
            this.kzxPanel2.ToolTipMessageCode = "";
            this.kzxPanel2.ToolTipText = "";
            // 
            // gcSetting
            // 
            this.gcSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSetting.Location = new System.Drawing.Point(2, 2);
            this.gcSetting.MainView = this.gridView1;
            this.gcSetting.Name = "gcSetting";
            this.gcSetting.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemButtonEdit1,
            this.repositoryItemButtonEdit2,
            this.repositoryItemButtonEdit3,
            this.repositoryItemButtonEdit4,
            this.repositoryItemCheckedComboBoxEdit1,
            this.repositoryItemComboBox1});
            this.gcSetting.Size = new System.Drawing.Size(507, 272);
            this.gcSetting.TabIndex = 0;
            this.gcSetting.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColumnsName,
            this.isDisplay,
            this.Freeze,
            this.ColumnsWidth,
            this.Order1,
            this.Order2,
            this.Top,
            this.Button,
            this.iOrder});
            this.gridView1.GridControl = this.gcSetting;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // ColumnsName
            // 
            this.ColumnsName.Caption = "列名";
            this.ColumnsName.FieldName = "ColumnsName";
            this.ColumnsName.Name = "ColumnsName";
            this.ColumnsName.Visible = true;
            this.ColumnsName.VisibleIndex = 0;
            // 
            // isDisplay
            // 
            this.isDisplay.Caption = "显示";
            this.isDisplay.ColumnEdit = this.repositoryItemCheckEdit1;
            this.isDisplay.FieldName = "isDisplay";
            this.isDisplay.Name = "isDisplay";
            this.isDisplay.Visible = true;
            this.isDisplay.VisibleIndex = 1;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // Freeze
            // 
            this.Freeze.Caption = "列冻结";
            this.Freeze.ColumnEdit = this.repositoryItemComboBox1;
            this.Freeze.FieldName = "Freeze";
            this.Freeze.Name = "Freeze";
            this.Freeze.Visible = true;
            this.Freeze.VisibleIndex = 2;
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEdit1.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.repositoryItemCheckedComboBoxEdit1.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "无"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "左边"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "右边")});
            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            this.repositoryItemCheckedComboBoxEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            // 
            // ColumnsWidth
            // 
            this.ColumnsWidth.Caption = "列宽";
            this.ColumnsWidth.FieldName = "ColumnsWidth";
            this.ColumnsWidth.Name = "ColumnsWidth";
            this.ColumnsWidth.Visible = true;
            this.ColumnsWidth.VisibleIndex = 3;
            // 
            // Order1
            // 
            this.Order1.Caption = "上移";
            this.Order1.ColumnEdit = this.repositoryItemButtonEdit1;
            this.Order1.FieldName = "Order1";
            this.Order1.Name = "Order1";
            this.Order1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.Order1.Visible = true;
            this.Order1.VisibleIndex = 4;
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "上移", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, true)});
            this.repositoryItemButtonEdit1.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            this.repositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // Order2
            // 
            this.Order2.Caption = "下移";
            this.Order2.ColumnEdit = this.repositoryItemButtonEdit2;
            this.Order2.FieldName = "Order2";
            this.Order2.Name = "Order2";
            this.Order2.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.Order2.Visible = true;
            this.Order2.VisibleIndex = 5;
            // 
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "下移", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, null, true)});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            this.repositoryItemButtonEdit2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // Top
            // 
            this.Top.Caption = "置顶";
            this.Top.ColumnEdit = this.repositoryItemButtonEdit3;
            this.Top.FieldName = "Top";
            this.Top.Name = "Top";
            this.Top.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.Top.Visible = true;
            this.Top.VisibleIndex = 6;
            // 
            // repositoryItemButtonEdit3
            // 
            this.repositoryItemButtonEdit3.AutoHeight = false;
            this.repositoryItemButtonEdit3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.repositoryItemButtonEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "置顶", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject7, "", null, null, true)});
            this.repositoryItemButtonEdit3.Name = "repositoryItemButtonEdit3";
            this.repositoryItemButtonEdit3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // Button
            // 
            this.Button.Caption = "置底";
            this.Button.ColumnEdit = this.repositoryItemButtonEdit4;
            this.Button.FieldName = "Button";
            this.Button.Name = "Button";
            this.Button.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.Button.Visible = true;
            this.Button.VisibleIndex = 7;
            // 
            // repositoryItemButtonEdit4
            // 
            this.repositoryItemButtonEdit4.AutoHeight = false;
            this.repositoryItemButtonEdit4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.repositoryItemButtonEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "置底", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject8, "", null, null, true)});
            this.repositoryItemButtonEdit4.Name = "repositoryItemButtonEdit4";
            this.repositoryItemButtonEdit4.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // iOrder
            // 
            this.iOrder.Caption = "排序";
            this.iOrder.FieldName = "iOrder";
            this.iOrder.Name = "iOrder";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // frm_GridSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 310);
            this.Controls.Add(this.kzxPanel2);
            this.Controls.Add(this.kzxPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_GridSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网格状态设置";
            this.Load += new System.EventHandler(this.frm_GridSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kzxPanel1)).EndInit();
            this.kzxPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kzxPanel2)).EndInit();
            this.kzxPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Kzx.UserControl.KzxPanel kzxPanel1;
        private Kzx.UserControl.KzxPanel kzxPanel2;
        private DevExpress.XtraGrid.GridControl gcSetting;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnSure;
        private DevExpress.XtraGrid.Columns.GridColumn ColumnsName;
        private DevExpress.XtraGrid.Columns.GridColumn isDisplay;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn Freeze;
        private DevExpress.XtraGrid.Columns.GridColumn ColumnsWidth;
        private DevExpress.XtraGrid.Columns.GridColumn Order1;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn Order2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn Top;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn Button;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn iOrder;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
    }
}