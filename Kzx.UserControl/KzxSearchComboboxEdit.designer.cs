using System.ComponentModel;
namespace Kzx.UserControl
{
    partial class KzxSearchComboboxEdit
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CaptionControl = new DevExpress.XtraEditors.LabelControl();
            this.ValueControl = new DevExpress.XtraEditors.SearchLookUpEdit();
            this._SearchLookUpEditView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._SearchLookUpEditView)).BeginInit();
            this.SuspendLayout();
            // 
            // CaptionControl
            // 
            this.CaptionControl.Location = new System.Drawing.Point(-2, 3);
            this.CaptionControl.Name = "CaptionControl";
            this.CaptionControl.Size = new System.Drawing.Size(70, 14);
            this.CaptionControl.TabIndex = 2;
            this.CaptionControl.Text = "labelControl1";
            // 
            // ValueControl
            // 
            this.ValueControl.Location = new System.Drawing.Point(70, 0);
            this.ValueControl.Name = "ValueControl";
            this.ValueControl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ValueControl.Properties.NullText = "";
            this.ValueControl.Properties.View = this._SearchLookUpEditView;
            this.ValueControl.Size = new System.Drawing.Size(151, 20);
            this.ValueControl.TabIndex = 5;
            this.ValueControl.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.lookUpEdit_QueryPopUp);
            this.ValueControl.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.lookUpEdit_Closed);
            // 
            // _SearchLookUpEditView
            // 
            this._SearchLookUpEditView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this._SearchLookUpEditView.Name = "_SearchLookUpEditView";
            this._SearchLookUpEditView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this._SearchLookUpEditView.OptionsView.ShowGroupPanel = false;
            this._SearchLookUpEditView.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.searchLookUpEdit1View_RowClick);
            // 
            // KzxSearchComboboxEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.Controls.Add(this.ValueControl);
            this.Controls.Add(this.CaptionControl);
            this.Name = "KzxSearchComboboxEdit";
            this.Size = new System.Drawing.Size(221, 21);
            this.Load += new System.EventHandler(this.KzxLookUpEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ValueControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._SearchLookUpEditView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl CaptionControl;
        private DevExpress.XtraEditors.SearchLookUpEdit ValueControl;
        private DevExpress.XtraGrid.Views.Grid.GridView _SearchLookUpEditView;
    }
}
