namespace PointsLibUi.Controls
{
    partial class SliderControl
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
            this._slider = new PointsLibUi.Controls.BetterTrackBar();
            ((System.ComponentModel.ISupportInitialize)(this._slider)).BeginInit();
            this.SuspendLayout();
            // 
            // _slider
            // 
            this._slider.Location = new System.Drawing.Point(3, 3);
            this._slider.Name = "_slider";
            this._slider.Size = new System.Drawing.Size(202, 45);
            this._slider.TabIndex = 0;
            // 
            // SliderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._slider);
            this.Name = "SliderControl";
            this.Size = new System.Drawing.Size(209, 51);
            ((System.ComponentModel.ISupportInitialize)(this._slider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BetterTrackBar _slider;




    }
}
