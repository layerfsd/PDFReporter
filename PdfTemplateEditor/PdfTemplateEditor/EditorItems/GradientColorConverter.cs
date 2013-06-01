using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Collections;

namespace AxiomCoders.PdfTemplateEditor.EditorItems
{
    public class GradientColorConverter : TypeConverter
    {
        public GradientColorConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if (str == null)
            {
                return base.ConvertFrom(context, culture, value);
            }
            string text = str.Trim();
            if (text.Length == 0)
            {
                return null;
            }
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            string color1 = text.Split(' ')[0];
            string color2 = text.Split(' ')[1];

            color1 = color1.Replace("Color1=","");
            string[] vals1 = color1.Split(';');

            if (vals1 == null)
            {
                return null;
            }
            if (vals1.Length != 3)
            {
                return null;
            }

            color2 = color2.Replace("Color2=", "");
            string[] vals2 = color2.Split(';');

            if (vals2 == null)
            {
                return null;
            }
            if (vals2.Length != 3)
            {
                return null;
            }

            try
            {
                Color c1 = Color.FromArgb(int.Parse(vals1[0]), int.Parse(vals1[1]), int.Parse(vals1[2]));
                Color c2 = Color.FromArgb(int.Parse(vals2[0]), int.Parse(vals2[1]), int.Parse(vals2[2]));

                return new GradientDefinition(c1, c2,PointF.Empty,PointF.Empty,GradientType.Linear);
            }
            catch
            {
                return null;
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                GradientDefinition colors = value as GradientDefinition;
                string c1 = string.Format("Color1={0};{1};{2};", colors.Color1.R, colors.Color1.G, colors.Color1.B);
                string c2 = string.Format("Color2={0};{1};{2};", colors.Color2.R, colors.Color2.G, colors.Color2.B);
                return string.Format("{0} {1}", c1, c2);
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }



        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues");
            }

            // we allow user to change only next gradient values:
            Color c1 = (Color)propertyValues["Color1"];
            Color c2 = (Color)propertyValues["Color2"];
            GradientType gt = (GradientType)propertyValues["GradientType"];
            
            GradientDefinition gd = ((RectangleShape)context.Instance).GradientDefinition;

            //and another (Point1, Point2) are just copyed from instance item...
            return new GradientDefinition(c1, c2, gd.Point1, gd.Point2, gt);
        }

            

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(GradientDefinition), attributes).Sort(new string[] { "Color1", "Color2"});
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }


 

    }
}
