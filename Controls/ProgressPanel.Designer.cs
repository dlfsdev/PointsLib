namespace PointsLibUi.Controls
{
    partial class ProgressPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._timeRemainingLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.LinkLabel();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._nameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _timeRemainingLabel
            // 
            this._timeRemainingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._timeRemainingLabel.AutoEllipsis = true;
            this._timeRemainingLabel.Location = new System.Drawing.Point(127, 3);
            this._timeRemainingLabel.Name = "_timeRemainingLabel";
            this._timeRemainingLabel.Size = new System.Drawing.Size(53, 13);
            this._timeRemainingLabel.TabIndex = 18;
            this._timeRemainingLabel.Text = "1hr 23min";
            this._timeRemainingLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.AutoSize = true;
            this._cancelButton.Location = new System.Drawing.Point(186, 4);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(40, 13);
            this._cancelButton.TabIndex = 17;
            this._cancelButton.TabStop = true;
            this._cancelButton.Text = "Cancel";
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(41, 3);
            this._progressBar.Maximum = 1000;
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(80, 14);
            this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this._progressBar.TabIndex = 16;
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Location = new System.Drawing.Point(3, 4);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(25, 13);
            this._nameLabel.TabIndex = 15;
            this._nameLabel.Text = "Pair";
            // 
            // ProgressPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._timeRemainingLabel);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._nameLabel);
            this.Name = "ProgressPanel";
            this.Size = new System.Drawing.Size(229, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _timeRemainingLabel;
        private System.Windows.Forms.LinkLabel _cancelButton;
        private System.Windows.Forms.ProgressBar _progressBar;
        private System.Windows.Forms.Label _nameLabel;
    }
}
