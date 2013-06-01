using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace AxiomCoders.PdfTemplateEditor.Common
{
	/// <summary>
	/// All possible measure units available
	/// </summary>
	public enum MeasureUnits
	{
		cm,
		mm,		
		inch,
		pixel, 
		point
	}

	/// <summary>
	/// Holds info on how resolution is used
	/// </summary>
	public enum ResolutionMeasure
	{
		PixelsPerInch,
		PixelsPerCm		
	}

	/// <summary>
	/// This is singleton class used to contain methods for manipulating units (cm, inch, mm, ...). It should contain unit coverters, etc.
	/// </summary>
	public class UnitsManager
	{
		private Dictionary<MeasureUnits, Dictionary<MeasureUnits, float>> conversionTable = new Dictionary<MeasureUnits, Dictionary<MeasureUnits, float>>();

		/// <summary>
		/// singleton stuff
		/// </summary>
		private static UnitsManager instance = null;
		public static UnitsManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new UnitsManager();
				}
				return instance;
			}
		}	

		/// <summary>
		/// Call this to initialize
		/// </summary>
		public UnitsManager()
		{			
		}

		private const float PointsPerInch = 72.0f;

		/// <summary>
		/// Initialize conversion table
		/// </summary>
		public void Initialize(float resolution, ResolutionMeasure resolutionMeasure)
		{
			// make conversion table
			conversionTable.Clear();
			Dictionary<MeasureUnits, float> table = new Dictionary<MeasureUnits, float>();

			table.Add(MeasureUnits.cm, 2.54f);
			table.Add(MeasureUnits.inch, 1);
			table.Add(MeasureUnits.mm, 25.4f);
			table.Add(MeasureUnits.pixel, resolutionMeasure == ResolutionMeasure.PixelsPerInch ? resolution : resolution / 2.54f);
			table.Add(MeasureUnits.point, PointsPerInch);
			conversionTable.Add(MeasureUnits.inch, table);


			table = new Dictionary<MeasureUnits, float>();
			table.Add(MeasureUnits.cm, 1);
			table.Add(MeasureUnits.inch, 0.394f);
			table.Add(MeasureUnits.mm, 10);
			table.Add(MeasureUnits.pixel, resolutionMeasure == ResolutionMeasure.PixelsPerCm ? resolution : resolution / 2.54f);
			table.Add(MeasureUnits.point, PointsPerInch * 0.394f);
			conversionTable.Add(MeasureUnits.cm, table);

			table = new Dictionary<MeasureUnits, float>();
			table.Add(MeasureUnits.cm, 0.1f);
			table.Add(MeasureUnits.inch, 0.0394f);
			table.Add(MeasureUnits.mm, 1);
			table.Add(MeasureUnits.pixel, resolutionMeasure == ResolutionMeasure.PixelsPerCm ? resolution / 10 : resolution / 25.4f);
			table.Add(MeasureUnits.point, PointsPerInch * 0.0394f);
			conversionTable.Add(MeasureUnits.mm, table);

			table = new Dictionary<MeasureUnits, float>();
			table.Add(MeasureUnits.cm, resolutionMeasure == ResolutionMeasure.PixelsPerCm ? 1.0f / resolution : 1.0f / (resolution / 2.54f));
			table.Add(MeasureUnits.inch, resolutionMeasure == ResolutionMeasure.PixelsPerInch ? 1.0f / resolution : 1.0f / (resolution));
			table.Add(MeasureUnits.mm, resolutionMeasure == ResolutionMeasure.PixelsPerCm ? 10.0f / resolution : 25.4f / resolution);
			table.Add(MeasureUnits.pixel, 1);
			table.Add(MeasureUnits.point, resolutionMeasure == ResolutionMeasure.PixelsPerInch ? resolution / PointsPerInch : resolution / PointsPerInch*2.54f);
			conversionTable.Add(MeasureUnits.pixel, table);

			table = new Dictionary<MeasureUnits, float>();
			table.Add(MeasureUnits.cm, 1.0f / (PointsPerInch*2.54f));
			table.Add(MeasureUnits.inch, 1.0f / PointsPerInch);
			table.Add(MeasureUnits.mm, 1.0f / (PointsPerInch*25.4f));
			table.Add(MeasureUnits.pixel, resolutionMeasure == ResolutionMeasure.PixelsPerInch ? PointsPerInch / resolution : PointsPerInch*2.54f / resolution );
			table.Add(MeasureUnits.point, 1);
			conversionTable.Add(MeasureUnits.point, table);


		}

		private float resolution = 72;
		public float Resolution
		{
			get { return resolution; }
			set { resolution = value; }
		}

		private ResolutionMeasure resolutionMeasure = ResolutionMeasure.PixelsPerInch;
		public AxiomCoders.PdfTemplateEditor.Common.ResolutionMeasure ResolutionMeasure
		{
			get { return resolutionMeasure; }
			set { resolutionMeasure = value; }
		}
		/// <summary>
		/// This will do conversion between units
		/// </summary>
		/// <param name="value"></param>
		/// <param name="sourceUnit"></param>
		/// <param name="resultingUnit"></param>
		/// <returns></returns>
		public float ConvertUnit(float value, MeasureUnits sourceUnit, MeasureUnits resultingUnit)
		{
			Initialize(this.resolution, this.resolutionMeasure);
			float rate = conversionTable[sourceUnit][resultingUnit];						
			return (float)Math.Round((double)(value*rate), 3);
		}

        public string UnitToString(MeasureUnits unit)
        {
            switch (unit)
            {
                case MeasureUnits.point: return "pt";
                case MeasureUnits.mm: return "mm";
                case MeasureUnits.cm: return "cm";
                case MeasureUnits.inch: return "in";
                case MeasureUnits.pixel: return "px";
                default:
                    return "";
            }
        }

        public MeasureUnits StringToUnit(string str)
        {
            int len = str.Length;
            string tmp = "";
			
			if (len < 3)
			{
				return MeasureUnits.point;
			}

            tmp = tmp + str[len-2] + str[len-1];
            switch (tmp)
            {
                case "in": return MeasureUnits.inch;
                case "cm": return MeasureUnits.cm;
                case "mm": return MeasureUnits.mm;
                case "pt": return MeasureUnits.point;
                case "px": return MeasureUnits.pixel;
                default:
                    return MeasureUnits.point;
            }
        }

		public MeasureUnits ConvertFromGraphicUnit(System.Drawing.GraphicsUnit gUnit)
		{			
			switch (gUnit)
			{
				case GraphicsUnit.Inch: return MeasureUnits.inch; 
				case GraphicsUnit.Millimeter: return  MeasureUnits.mm; 
				case GraphicsUnit.Pixel: return MeasureUnits.pixel; 
				case GraphicsUnit.Point: return MeasureUnits.point; 
				default:
					return MeasureUnits.inch;
			}
		}
	}
}
