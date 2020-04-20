namespace FuckingERP.重写控件_弹窗类
{
    partial class frm_ShowControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ShowControl));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.kzxSimpleButton2 = new Kzx.UserControl.KzxSimpleButton();
            this.kzxSearchComboboxEdit1 = new Kzx.UserControl.KzxSearchComboboxEdit();
            this.kzxTextBox1 = new Kzx.UserControl.KzxTextBox();
            this.kzxSimpleButton1 = new Kzx.UserControl.KzxSimpleButton();
            this.kzxGridControl1 = new Kzx.UserControl.KzxGridControl();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemGridLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kzxGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTipController1
            // 
            this.toolTipController1.ToolTipLocation = DevExpress.Utils.ToolTipLocation.TopRight;
            this.toolTipController1.ToolTipStyle = DevExpress.Utils.ToolTipStyle.Windows7;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            this.toolTipController1.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController1_GetActiveObjectInfo);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.kzxSimpleButton2);
            this.groupControl1.Controls.Add(this.kzxSearchComboboxEdit1);
            this.groupControl1.Controls.Add(this.kzxTextBox1);
            this.groupControl1.Controls.Add(this.kzxSimpleButton1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(800, 81);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "自定义控件测试";
            // 
            // kzxSimpleButton2
            // 
            this.kzxSimpleButton2.DesigeCaption = "网格状态设置";
            this.kzxSimpleButton2.DesigeEnabled = true;
            this.kzxSimpleButton2.DesigeVisible = true;
            this.kzxSimpleButton2.DllName = "";
            this.kzxSimpleButton2.Key = "kzxSimpleButton2";
            this.kzxSimpleButton2.KzxButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxSimpleButton2.KzxImage = null;
            this.kzxSimpleButton2.KzxImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.kzxSimpleButton2.LabelForeColor = System.Drawing.Color.Empty;
            this.kzxSimpleButton2.LayoutColumn = 0;
            this.kzxSimpleButton2.LayoutColumnSpan = 1;
            this.kzxSimpleButton2.LayoutRow = 0;
            this.kzxSimpleButton2.LayoutRowSpan = 1;
            this.kzxSimpleButton2.Location = new System.Drawing.Point(678, 37);
            this.kzxSimpleButton2.MessageCode = "0";
            this.kzxSimpleButton2.Name = "kzxSimpleButton2";
            this.kzxSimpleButton2.Size = new System.Drawing.Size(92, 23);
            this.kzxSimpleButton2.TabIndex = 4;
            this.kzxSimpleButton2.Text = "网格状态设置";
            this.kzxSimpleButton2.ToolTipMessageCode = "";
            this.kzxSimpleButton2.ToolTipText = "";
            this.kzxSimpleButton2.Click += new System.EventHandler(this.kzxSimpleButton2_Click);
            // 
            // kzxSearchComboboxEdit1
            // 
            this.kzxSearchComboboxEdit1.AllowEdit = true;
            this.kzxSearchComboboxEdit1.AllowValueRange = false;
            this.kzxSearchComboboxEdit1.CaptionLabelWidth = 75;
            this.kzxSearchComboboxEdit1.Columns = "";
            this.kzxSearchComboboxEdit1.DataType = Kzx.UserControl.KzxDataType.Str;
            this.kzxSearchComboboxEdit1.DefaultValue = "";
            this.kzxSearchComboboxEdit1.DesigeCaption = "labelControl1";
            this.kzxSearchComboboxEdit1.DesigeEnabled = true;
            this.kzxSearchComboboxEdit1.DesigeVisible = true;
            this.kzxSearchComboboxEdit1.DisplayMemberPath = "";
            this.kzxSearchComboboxEdit1.DllName = "";
            this.kzxSearchComboboxEdit1.Field = "";
            this.kzxSearchComboboxEdit1.FilterString = "";
            this.kzxSearchComboboxEdit1.FormatString = "";
            this.kzxSearchComboboxEdit1.IsNull = true;
            this.kzxSearchComboboxEdit1.IsUnique = false;
            this.kzxSearchComboboxEdit1.IsValidation = true;
            this.kzxSearchComboboxEdit1.Key = "KzxSearchComboboxEdit";
            this.kzxSearchComboboxEdit1.KzxBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxSearchComboboxEdit1.KzxFormatString = "";
            this.kzxSearchComboboxEdit1.KzxFormatType = DevExpress.Utils.FormatType.None;
            this.kzxSearchComboboxEdit1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxSearchComboboxEdit1.LayoutColumn = 0;
            this.kzxSearchComboboxEdit1.LayoutColumnSpan = 1;
            this.kzxSearchComboboxEdit1.LayoutRow = 0;
            this.kzxSearchComboboxEdit1.LayoutRowSpan = 1;
            this.kzxSearchComboboxEdit1.Location = new System.Drawing.Point(436, 37);
            this.kzxSearchComboboxEdit1.MaxLength = 0;
            this.kzxSearchComboboxEdit1.MaxValue = "";
            this.kzxSearchComboboxEdit1.MessageCode = "0";
            this.kzxSearchComboboxEdit1.MinValue = "";
            this.kzxSearchComboboxEdit1.Name = "kzxSearchComboboxEdit1";
            this.kzxSearchComboboxEdit1.NullText = "";
            this.kzxSearchComboboxEdit1.ParentTable = null;
            this.kzxSearchComboboxEdit1.ReadOnly = false;
            this.kzxSearchComboboxEdit1.RegexString = "";
            this.kzxSearchComboboxEdit1.SelectedValuePath = "";
            this.kzxSearchComboboxEdit1.Size = new System.Drawing.Size(221, 21);
            this.kzxSearchComboboxEdit1.SourceTable = "";
            this.kzxSearchComboboxEdit1.TabIndex = 3;
            this.kzxSearchComboboxEdit1.Table = "";
            this.kzxSearchComboboxEdit1.TextBackColor = System.Drawing.Color.White;
            this.kzxSearchComboboxEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.kzxSearchComboboxEdit1.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxSearchComboboxEdit1.ToolTipMaxLengthText = "";
            this.kzxSearchComboboxEdit1.ToolTipMessageCode = "";
            this.kzxSearchComboboxEdit1.ToolTipText = "";
            this.kzxSearchComboboxEdit1.ValidateGroup = "";
            this.kzxSearchComboboxEdit1.ValueDependencyField = "";
            // 
            // kzxTextBox1
            // 
            this.kzxTextBox1.AllowEdit = true;
            this.kzxTextBox1.AllowValueRange = false;
            this.kzxTextBox1.CaptionLabelWidth = 50;
            this.kzxTextBox1.DataSource = null;
            this.kzxTextBox1.DataType = Kzx.UserControl.KzxDataType.Str;
            this.kzxTextBox1.DefaultValue = "";
            this.kzxTextBox1.DesigeCaption = "用户名";
            this.kzxTextBox1.DesigeEnabled = true;
            this.kzxTextBox1.DesigeVisible = true;
            this.kzxTextBox1.DisplayMemberPath = "";
            this.kzxTextBox1.DllName = "";
            this.kzxTextBox1.Field = "";
            this.kzxTextBox1.FormatString = "";
            this.kzxTextBox1.IsNull = true;
            this.kzxTextBox1.IsUnique = false;
            this.kzxTextBox1.IsValidation = true;
            this.kzxTextBox1.Key = "KzxTextBox";
            this.kzxTextBox1.KzxBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxTextBox1.KzxEditMask = "";
            this.kzxTextBox1.KzxFormatString = "";
            this.kzxTextBox1.KzxFormatType = DevExpress.Utils.FormatType.None;
            this.kzxTextBox1.KzxMaskType = "None";
            this.kzxTextBox1.LabelForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxTextBox1.LayoutColumn = 0;
            this.kzxTextBox1.LayoutColumnSpan = 1;
            this.kzxTextBox1.LayoutRow = 0;
            this.kzxTextBox1.LayoutRowSpan = 1;
            this.kzxTextBox1.Location = new System.Drawing.Point(130, 38);
            this.kzxTextBox1.MaxLength = 0;
            this.kzxTextBox1.MaxValue = "";
            this.kzxTextBox1.MessageCode = "0";
            this.kzxTextBox1.MinValue = "";
            this.kzxTextBox1.Name = "kzxTextBox1";
            this.kzxTextBox1.ParentTable = null;
            this.kzxTextBox1.PasswordChar = '\0';
            this.kzxTextBox1.ReadOnly = false;
            this.kzxTextBox1.RegexString = "";
            this.kzxTextBox1.SelectedValuePath = "";
            this.kzxTextBox1.Size = new System.Drawing.Size(284, 21);
            this.kzxTextBox1.SourceTable = "";
            this.kzxTextBox1.TabIndex = 2;
            this.kzxTextBox1.Table = "";
            this.kzxTextBox1.TextBackColor = System.Drawing.Color.White;
            this.kzxTextBox1.TextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(31)))), ((int)(((byte)(53)))));
            this.kzxTextBox1.ToolTipMaxLengthText = "";
            this.kzxTextBox1.ToolTipMessageCode = "";
            this.kzxTextBox1.ToolTipText = "";
            this.kzxTextBox1.ValidateGroup = "";
            this.kzxTextBox1.Value = "";
            this.kzxTextBox1.ValueDependencyField = "";
            // 
            // kzxSimpleButton1
            // 
            this.kzxSimpleButton1.DesigeCaption = "测试弹窗提示";
            this.kzxSimpleButton1.DesigeEnabled = true;
            this.kzxSimpleButton1.DesigeVisible = true;
            this.kzxSimpleButton1.DllName = "";
            this.kzxSimpleButton1.Key = "kzxSimpleButton1";
            this.kzxSimpleButton1.KzxButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.kzxSimpleButton1.KzxImage = null;
            this.kzxSimpleButton1.KzxImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.kzxSimpleButton1.LabelForeColor = System.Drawing.Color.Empty;
            this.kzxSimpleButton1.LayoutColumn = 0;
            this.kzxSimpleButton1.LayoutColumnSpan = 1;
            this.kzxSimpleButton1.LayoutRow = 0;
            this.kzxSimpleButton1.LayoutRowSpan = 1;
            this.kzxSimpleButton1.Location = new System.Drawing.Point(12, 38);
            this.kzxSimpleButton1.MessageCode = "0";
            this.kzxSimpleButton1.Name = "kzxSimpleButton1";
            this.kzxSimpleButton1.Size = new System.Drawing.Size(75, 23);
            this.kzxSimpleButton1.TabIndex = 1;
            this.kzxSimpleButton1.Text = "测试弹窗提示";
            this.kzxSimpleButton1.ToolTipController = this.toolTipController1;
            this.kzxSimpleButton1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.kzxSimpleButton1.ToolTipMessageCode = "";
            this.kzxSimpleButton1.ToolTipText = "点击后，会弹出重写的弹出提示框！";
            // 
            // kzxGridControl1
            // 
            this.kzxGridControl1.AllowEdit = true;
            this.kzxGridControl1.AllowValueRange = false;
            this.kzxGridControl1.CanAutoWidth = false;
            this.kzxGridControl1.ColorSet = "";
            this.kzxGridControl1.ColumnAutoWidth = false;
            this.kzxGridControl1.Columns = "";
            this.kzxGridControl1.CurrentRowIndex = -1;
            this.kzxGridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kzxGridControl1.DataType = Kzx.UserControl.KzxDataType.Str;
            this.kzxGridControl1.DefaultValue = "";
            this.kzxGridControl1.DesigeCaption = "显示标题";
            this.kzxGridControl1.DesigeEnabled = true;
            this.kzxGridControl1.DesigeVisible = true;
            this.kzxGridControl1.DisplayMemberPath = "";
            this.kzxGridControl1.DisplayRightClickMenu = true;
            this.kzxGridControl1.DisplayRightExportExcel = true;
            this.kzxGridControl1.DllName = "";
            this.kzxGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Append.ImageIndex = 0;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.CancelEdit.ImageIndex = 8;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Edit.ImageIndex = 6;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.EndEdit.ImageIndex = 7;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.First.ImageIndex = 2;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.First.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Last.ImageIndex = 5;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Last.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Next.ImageIndex = 4;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Next.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.NextPage.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Prev.ImageIndex = 3;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Prev.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
            this.kzxGridControl1.EmbeddedNavigator.Buttons.Remove.ImageIndex = 1;
            this.kzxGridControl1.EmbeddedNavigator.TextLocation = DevExpress.XtraEditors.NavigatorButtonsTextLocation.None;
            this.kzxGridControl1.Field = "";
            this.kzxGridControl1.FooterCellDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("kzxGridControl1.FooterCellDictionary")));
            this.kzxGridControl1.FormatString = "";
            this.kzxGridControl1.IsBandedGridView = false;
            this.kzxGridControl1.IsNull = true;
            this.kzxGridControl1.IsUnique = false;
            this.kzxGridControl1.IsValidation = true;
            this.kzxGridControl1.IsValidationOther = true;
            this.kzxGridControl1.Key = "";
            this.kzxGridControl1.KzxUseEmbeddedNavigator = true;
            this.kzxGridControl1.LayoutColumn = 0;
            this.kzxGridControl1.LayoutColumnSpan = 1;
            this.kzxGridControl1.LayoutRow = 0;
            this.kzxGridControl1.LayoutRowSpan = 1;
            gridLevelNode1.RelationName = "Level1";
            this.kzxGridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.kzxGridControl1.Location = new System.Drawing.Point(0, 81);
            this.kzxGridControl1.MainView = this.gridView1;
            this.kzxGridControl1.MaxValue = "";
            this.kzxGridControl1.MessageCode = "0";
            this.kzxGridControl1.MinValue = "";
            this.kzxGridControl1.Name = "kzxGridControl1";
            this.kzxGridControl1.ParentTable = "";
            this.kzxGridControl1.ReadOnly = false;
            this.kzxGridControl1.RegexString = "";
            this.kzxGridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemGridLookUpEdit1});
            this.kzxGridControl1.SelectedValuePath = "";
            this.kzxGridControl1.ShowAutoFilterRow = false;
            this.kzxGridControl1.ShowFooter = false;
            this.kzxGridControl1.ShowGroupPanel = false;
            this.kzxGridControl1.Size = new System.Drawing.Size(800, 369);
            this.kzxGridControl1.SourceTable = "";
            this.kzxGridControl1.TabIndex = 1;
            this.kzxGridControl1.Table = "";
            this.kzxGridControl1.ToolTipMaxLengthText = "";
            this.kzxGridControl1.ToolTipMessageCode = "";
            this.kzxGridControl1.ToolTipText = "";
            this.kzxGridControl1.UseEmbeddedNavigator = true;
            this.kzxGridControl1.ValidateGroup = "";
            this.kzxGridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemGridLookUpEdit1
            // 
            this.repositoryItemGridLookUpEdit1.AutoHeight = false;
            this.repositoryItemGridLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEdit1.Name = "repositoryItemGridLookUpEdit1";
            this.repositoryItemGridLookUpEdit1.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.MultiSelect = true;
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.AntiqueWhite;
            this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.gridView1.GridControl = this.kzxGridControl1;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gridView1.IndicatorWidth = 40;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView1.OptionsView.EnableAppearanceOddRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // frm_ShowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kzxGridControl1);
            this.Controls.Add(this.groupControl1);
            this.HideOnClose = true;
            this.Name = "frm_ShowControl";
            this.Text = "重写控件-弹窗类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_ShowControl_FormClosed);
            this.Load += new System.EventHandler(this.frm_ShowControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kzxGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.Utils.ToolTipController toolTipController1;
        private Kzx.UserControl.KzxSimpleButton kzxSimpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Kzx.UserControl.KzxTextBox kzxTextBox1;
        private Kzx.UserControl.KzxGridControl kzxGridControl1;
        private Kzx.UserControl.KzxSearchComboboxEdit kzxSearchComboboxEdit1;
        private Kzx.UserControl.KzxSimpleButton kzxSimpleButton2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}