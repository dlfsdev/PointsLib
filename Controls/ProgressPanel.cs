using PointsLibUi.Extensions;
using System;
using System.Windows.Forms;

namespace PointsLibUi.Controls
{
    public sealed partial class ProgressPanel : UserControl
    {
        private readonly int _horizontalPadding;

        public event EventHandler CancelClick;

        public ProgressPanel()
        {
            InitializeComponent();

            _horizontalPadding = _cancelButton.Left - _timeRemainingLabel.Right;
            IndefiniteCompletion = false;

            _cancelButton.Click += (s,e) => CancelClick.RaiseSafe(s, e);
        }

        public string AlgorithmName
        {
            get { return _nameLabel.Text; }
            
            set
            {
                _nameLabel.Text = value;
                PositionProgressBar();
            }
        }

        public string TimeRemainingText
        {
            get { return _timeRemainingLabel.Text; }
            set { _timeRemainingLabel.Text = value; }
        }

        public double FractionComplete
        {
            // could implement get if needed

            set
            {
                int progress = _progressBar.Minimum +
                    (int)Math.Round(value * (_progressBar.Maximum - _progressBar.Minimum + 1));
                _progressBar.SafeSetValueNoAnimate(progress);
            }
        }

        public bool IndefiniteCompletion
        {
            get
            {
                return _progressBar.Style == ProgressBarStyle.Marquee;
            }

            set
            {
                if (value == IndefiniteCompletion)
                    return;
                _progressBar.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
                _timeRemainingLabel.Visible = !value;
                PositionProgressBar();
            }
        }

        private void PositionProgressBar()
        {
            int newLeft = _nameLabel.Right + _horizontalPadding;
            int newRight = IndefiniteCompletion ? _timeRemainingLabel.Right : _timeRemainingLabel.Left - _horizontalPadding;

            if (newLeft == _progressBar.Left && newRight == _progressBar.Right)
                return;

            _progressBar.SetBounds(newLeft, _progressBar.Top, newRight - newLeft, _progressBar.Height);
            
        }
    }

}
