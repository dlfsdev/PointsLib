using System.ComponentModel;
using System.Windows.Forms;

namespace PointsLibUi.Controls
{
    public sealed partial class SliderControl : UserControl
    {
        public SliderControl()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("ChildControls")]
        public TrackBar Slider
        {
            get { return _slider; }
        }
    }
}
