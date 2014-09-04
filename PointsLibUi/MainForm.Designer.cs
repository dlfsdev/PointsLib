namespace PointsLibUi
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mouseLocationLabel = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findClosestPairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveConvexHullToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveKMeansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solveTravellingSalesmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._optionsTable = new System.Windows.Forms.PropertyGrid();
            this.boundsLabel = new System.Windows.Forms.Label();
            this._canvas = new PointsLibUi.Controls.Canvas();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // mouseLocationLabel
            // 
            this.mouseLocationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseLocationLabel.Location = new System.Drawing.Point(884, 498);
            this.mouseLocationLabel.Name = "mouseLocationLabel";
            this.mouseLocationLabel.Size = new System.Drawing.Size(192, 13);
            this.mouseLocationLabel.TabIndex = 7;
            this.mouseLocationLabel.Text = "(50, 50)";
            this.mouseLocationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointsToolStripMenuItem,
            this.algorithmToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1091, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "MainMenu";
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPointsToolStripMenuItem,
            this.savePointsToolStripMenuItem,
            this.clearPointsToolStripMenuItem,
            this.placeManuallyToolStripMenuItem});
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.pointsToolStripMenuItem.Text = "&Points";
            // 
            // loadPointsToolStripMenuItem
            // 
            this.loadPointsToolStripMenuItem.Name = "loadPointsToolStripMenuItem";
            this.loadPointsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.loadPointsToolStripMenuItem.Text = "&Load...";
            this.loadPointsToolStripMenuItem.Click += new System.EventHandler(this.loadPointsToolStripMenuItem_Click);
            // 
            // savePointsToolStripMenuItem
            // 
            this.savePointsToolStripMenuItem.Name = "savePointsToolStripMenuItem";
            this.savePointsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.savePointsToolStripMenuItem.Text = "&Save...";
            this.savePointsToolStripMenuItem.Click += new System.EventHandler(this.savePointsToolStripMenuItem_Click);
            // 
            // clearPointsToolStripMenuItem
            // 
            this.clearPointsToolStripMenuItem.Name = "clearPointsToolStripMenuItem";
            this.clearPointsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearPointsToolStripMenuItem.Text = "Clear All";
            this.clearPointsToolStripMenuItem.Click += new System.EventHandler(this.clearPointsToolStripMenuItem_Click);
            // 
            // placeManuallyToolStripMenuItem
            // 
            this.placeManuallyToolStripMenuItem.Name = "placeManuallyToolStripMenuItem";
            this.placeManuallyToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.placeManuallyToolStripMenuItem.Text = "Place &Manually...";
            this.placeManuallyToolStripMenuItem.Click += new System.EventHandler(this.placeManuallyToolStripMenuItem_Click);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findClosestPairToolStripMenuItem,
            this.solveConvexHullToolStripMenuItem,
            this.solveKMeansToolStripMenuItem,
            this.solveTravellingSalesmanToolStripMenuItem});
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.algorithmToolStripMenuItem.Text = "&Algorithms";
            this.algorithmToolStripMenuItem.DropDownOpening += new System.EventHandler(this.algorithmToolStripMenuItem_DropDownOpening);
            // 
            // findClosestPairToolStripMenuItem
            // 
            this.findClosestPairToolStripMenuItem.Name = "findClosestPairToolStripMenuItem";
            this.findClosestPairToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.findClosestPairToolStripMenuItem.Text = "Find Closest &Pair";
            this.findClosestPairToolStripMenuItem.Click += new System.EventHandler(this.findClosestPairToolStripMenuItem_Click);
            // 
            // solveConvexHullToolStripMenuItem
            // 
            this.solveConvexHullToolStripMenuItem.Name = "solveConvexHullToolStripMenuItem";
            this.solveConvexHullToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.solveConvexHullToolStripMenuItem.Text = "Solve Convex &Hull";
            this.solveConvexHullToolStripMenuItem.Click += new System.EventHandler(this.solveConvexHullToolStripMenuItem_Click);
            // 
            // solveKMeansToolStripMenuItem
            // 
            this.solveKMeansToolStripMenuItem.Name = "solveKMeansToolStripMenuItem";
            this.solveKMeansToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.solveKMeansToolStripMenuItem.Text = "Solve &k-means";
            this.solveKMeansToolStripMenuItem.Click += new System.EventHandler(this.solveKMeansToolStripMenuItem_Click);
            // 
            // solveTravellingSalesmanToolStripMenuItem
            // 
            this.solveTravellingSalesmanToolStripMenuItem.Name = "solveTravellingSalesmanToolStripMenuItem";
            this.solveTravellingSalesmanToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.solveTravellingSalesmanToolStripMenuItem.Text = "Solve &Travelling Salesman";
            this.solveTravellingSalesmanToolStripMenuItem.Click += new System.EventHandler(this.solveTravellingSalesmanToolStripMenuItem_Click);
            // 
            // _optionsTable
            // 
            this._optionsTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this._optionsTable.CanShowVisualStyleGlyphs = false;
            this._optionsTable.CategorySplitterColor = System.Drawing.SystemColors.ControlLight;
            this._optionsTable.LineColor = System.Drawing.SystemColors.ControlLight;
            this._optionsTable.Location = new System.Drawing.Point(12, 27);
            this._optionsTable.Name = "_optionsTable";
            this._optionsTable.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this._optionsTable.Size = new System.Drawing.Size(229, 390);
            this._optionsTable.TabIndex = 13;
            this._optionsTable.ToolbarVisible = false;
            // 
            // boundsLabel
            // 
            this.boundsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.boundsLabel.AutoSize = true;
            this.boundsLabel.Location = new System.Drawing.Point(244, 498);
            this.boundsLabel.Name = "boundsLabel";
            this.boundsLabel.Size = new System.Drawing.Size(82, 13);
            this.boundsLabel.TabIndex = 14;
            this.boundsLabel.Text = "(0,0) - (100,100)";
            // 
            // _canvas
            // 
            this._canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._canvas.Cursor = System.Windows.Forms.Cursors.Cross;
            this._canvas.Location = new System.Drawing.Point(247, 27);
            this._canvas.Name = "_canvas";
            this._canvas.Size = new System.Drawing.Size(832, 468);
            this._canvas.TabIndex = 15;
            this._canvas.TabStop = false;
            this._canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this._canvas.MouseLeave += new System.EventHandler(this.Canvas_MouseLeave);
            this._canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1091, 520);
            this.Controls.Add(this.boundsLabel);
            this.Controls.Add(this._optionsTable);
            this.Controls.Add(this.mouseLocationLabel);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this._canvas);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "PointsLib";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.MouseLeave += new System.EventHandler(this.Canvas_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label mouseLocationLabel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearPointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveConvexHullToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveTravellingSalesmanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem placeManuallyToolStripMenuItem;
        private System.Windows.Forms.PropertyGrid _optionsTable;
        private System.Windows.Forms.Label boundsLabel;
        private System.Windows.Forms.ToolStripMenuItem findClosestPairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solveKMeansToolStripMenuItem;
        private Controls.Canvas _canvas;
    }
}

