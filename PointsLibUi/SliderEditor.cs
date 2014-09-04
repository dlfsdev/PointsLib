using PointsLibUi.Controls;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PointsLibUi
{
    /// <summary>
    /// A <see cref="System.ComponentModel.UITypeEditor"/> that allows the user to select a
    /// number using a <see cref="System.Windows.Forms.TrackBar"/>
    /// </summary>
    /// <typeparam name="T">The type of number that will be provided and returned. For example, int.</typeparam>
    /// <typeparam name="P">A <see cref="PointsLibUi.SliderEditorProperties"/> object that describes
    ///     the properties of the slider</typeparam>
    internal sealed class SliderEditor<T, P> : UITypeEditor
        where P : SliderEditorProperties, new()
    {
        P _properties = new P();

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes =
                provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (wfes == null)
                return value;

            SliderControl control = new SliderControl();
            control.Slider.Minimum = _properties.MinValue;
            control.Slider.Maximum = _properties.MaxValue;

            int intValue = (int)Convert.ChangeType(value, typeof(int));
            control.Slider.Value = intValue;
            control.Slider.Click += (s, e) => wfes.CloseDropDown();

            wfes.DropDownControl(control);
            
            T newValue = (T)Convert.ChangeType(control.Slider.Value, typeof(T));
            return newValue;
        }
    }



    /// <summary>
    /// An interface for objects that provide properties to a <see cref="PointsLibUi.SliderEditor"/>
    /// </summary>
    public interface SliderEditorProperties
    {
        int MinValue { get; }
        int MaxValue { get; }
    }
}
