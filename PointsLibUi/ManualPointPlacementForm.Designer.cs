namespace PointsLibUi
{
    partial class ManualPointPlacementForm
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
            System.Windows.Forms.Label _caption;
            this._pointsText = new System.Windows.Forms.TextBox();
            this._ok = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            _caption = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _pointsText
            // 
            this._pointsText.AcceptsReturn = true;
            this._pointsText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._pointsText.Location = new System.Drawing.Point(13, 25);
            this._pointsText.Multiline = true;
            this._pointsText.Name = "_pointsText";
            this._pointsText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._pointsText.Size = new System.Drawing.Size(235, 155);
            this._pointsText.TabIndex = 1;
            this._pointsText.WordWrap = false;
            // 
            // _ok
            // 
            this._ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ok.Location = new System.Drawing.Point(172, 197);
            this._ok.Name = "_ok";
            this._ok.Size = new System.Drawing.Size(75, 23);
            this._ok.TabIndex = 3;
            this._ok.Text = "OK";
            this._ok.UseVisualStyleBackColor = true;
            // 
            // _cancel
            // 
            this._cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancel.Location = new System.Drawing.Point(91, 197);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 2;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            // 
            // _caption
            // 
            _caption.AutoSize = true;
            _caption.Location = new System.Drawing.Point(12, 9);
            _caption.Name = "_caption";
            _caption.Size = new System.Drawing.Size(129, 13);
            _caption.TabIndex = 0;
            _caption.Text = "Enter one x,y pair per line:";
            // 
            // ManualPointPlacementForm
            // 
            this.AcceptButton = this._ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancel;
            this.ClientSize = new System.Drawing.Size(258, 232);
            this.ControlBox = false;
            this.Controls.Add(_caption);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._ok);
            this.Controls.Add(this._pointsText);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManualPointPlacementForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Point Placement";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _pointsText;
        private System.Windows.Forms.Button _ok;
        private System.Windows.Forms.Button _cancel;
    }
}