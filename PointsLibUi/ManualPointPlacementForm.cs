using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PointsLibUi
{
    public partial class ManualPointPlacementForm : Form
    {
        public ManualPointPlacementForm()
        {
            InitializeComponent();
        }

        public IEnumerable<PointF> Points
        {
            get
            {
                return PointsSerializer.Parse(_pointsText.Text);
            }

            set
            {
                _pointsText.Text = PointsSerializer.ToString(value);
                _pointsText.Select(_pointsText.Text.Length, 0);
                _pointsText.ScrollToBottom();
            }
        }
    }
}
