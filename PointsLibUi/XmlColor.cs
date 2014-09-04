using System;
using System.Drawing;
using System.Xml.Serialization;

namespace PointsLibUi
{
    /// <summary>
    /// Utility class for serializing <see cref="System.Drawing.Color"/> to/from XML
    /// Thanks stackoverflow: http://stackoverflow.com/q/3280362/3549027
    /// </summary>
    public sealed class XmlColor
    {
        private Color _color = Color.Black;

        public XmlColor()
        {
        }

        public XmlColor(Color c)
        {
            _color = c;
        }

        public static implicit operator Color(XmlColor x)
        {
            return x._color;
        }

        public static implicit operator XmlColor(Color c)
        {
            return new XmlColor(c);
        }

        [XmlAttribute]
        public string WebColor
        {
            get
            {
                return ColorTranslator.ToHtml(_color);
            }

            set
            {
                try
                {
                    if (Alpha == 0xFF) // preserve named color value if possible
                        _color = ColorTranslator.FromHtml(value);
                    else
                        _color = Color.FromArgb(Alpha, ColorTranslator.FromHtml(value));
                }
                catch (Exception)
                {
                    _color = Color.Black;
                }
            }
        }

        [XmlAttribute]
        public byte Alpha
        {
            get
            {
                return _color.A;
            }

            set
            {
                // avoid hammering named color if no alpha change
                if (value == _color.A)
                    return;
                _color = Color.FromArgb(value, _color);
            }
        }

        public bool ShouldSerializeAlpha()
        {
            return Alpha < 0xFF;
        }
    }
}
