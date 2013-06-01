using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace AxiomCoders.PdfTemplateEditor.Common
{
	internal class MyDeviceContext : IDeviceContext
	{
		private Graphics graphics;
		private IntPtr hdc = IntPtr.Zero;
		private Font selectedFont = null;
		private IntPtr hfont = IntPtr.Zero;


		public MyDeviceContext() : this(Graphics.FromHwnd(IntPtr.Zero))
		{
		}


		public MyDeviceContext(Graphics g)
		{
			this.graphics = g;
		}

		#region IDeviceContext Members

		public Font SelectedFont
		{
			get
			{
				return selectedFont;
			}
			set
			{
				if (selectedFont != value)
				{
					selectedFont = value;
					if (selectedFont != null)
					{
						hfont = selectedFont.ToHfont();
						IntPtr prevGDIObject = UnsafeNativeMethods.SelectObject(new HandleRef(this, GetHdc()), new
						HandleRef(this, hfont));
						if (prevGDIObject != IntPtr.Zero)
						{
							UnsafeNativeMethods.DeleteObject(new HandleRef(this, prevGDIObject));
						}
					}
					else if (hfont != IntPtr.Zero)
					{
						UnsafeNativeMethods.DeleteObject(new HandleRef(this, hfont));
					}
				}
			}		
		}


		public IntPtr GetHdc()
		{
			if (hdc == IntPtr.Zero)
			{
				hdc = this.graphics.GetHdc();
			}
			return hdc;
		}

		public void ReleaseHdc()
		{
			SelectedFont = null;
			if (this.hdc != IntPtr.Zero)
			{
				this.graphics.ReleaseHdc();
				hdc = IntPtr.Zero;
			}
		}

		#endregion


		#region IDisposable Members
		
		void IDisposable.Dispose()
		{
			ReleaseHdc();
		}

		#endregion
	}		
	
	
	class UnsafeNativeMethods
	{
		[DllImport("Gdi32", SetLastError = true, ExactSpelling = true,	EntryPoint = "SelectObject", CharSet = CharSet.Auto)]
		public static extern IntPtr SelectObject(HandleRef hdc,	HandleRef obj);
		[DllImport("Gdi32", SetLastError = true, ExactSpelling = true,	EntryPoint = "DeleteObject", CharSet = CharSet.Auto)]
		public static extern bool DeleteObject(HandleRef hObject);
	}
	
    /// <summary>
    /// Embedding license for font
    /// </summary>
    public enum EmbeddingLicense
    {
        NotAllowed,
        Allowed,
        AllowedReadOnly
    }
	
	internal class FontMetrics
	{
		[DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern uint GetOutlineTextMetrics(IntPtr hdc, uint cbData, IntPtr ptrZero);

		[DllImport("gdi32.dll")]
		static extern uint GetGlyphOutline(IntPtr hdc, uint uChar, uint uFormat, out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);

        [DllImport("gdi32.dll")]
        static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, IntPtr lvpBuffer, uint cbData);

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYGONHEADER
		{
			public int cb;
			public int dwType;
			[MarshalAs(UnmanagedType.Struct)]
			public POINTFX pfxStart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYCURVEHEADER
		{
			public short wType;
			public short cpfx;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FIXED
		{
			public short fract;
			public short value;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MAT2
		{
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM11;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM12;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM21;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM22;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTFX
		{
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED x;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct GLYPHMETRICS
		{
			public int gmBlackBoxX;
			public int gmBlackBoxY;
			[MarshalAs(UnmanagedType.Struct)]
			public POINT gmptGlyphOrigin;
			public short gmCellIncX;
			public short gmCellIncY;
		}


		public const int GGO_GLYPH_INDEX = 0x80;
		public const int GGO_GLYPH_METRICS = 0;
		public const long GDI_ERROR = 0xFFFFFFFF;

		// Parse a glyph outline in native format
		public int GetGlyphWidth(Font pIncFont, int charIndex)
		{
			GLYPHMETRICS metrics = new GLYPHMETRICS();
			MAT2 matrix = new MAT2();
			matrix.eM11.value = 1;
			matrix.eM12.value = 0;
			matrix.eM21.value = 0;
			matrix.eM22.value = 1;

			using (MyDeviceContext dc = new MyDeviceContext())
			{
				Font newFont = new Font(pIncFont.FontFamily, pIncFont.FontFamily.GetEmHeight(pIncFont.Style), pIncFont.Style, GraphicsUnit.Pixel);							
				dc.SelectedFont = newFont;

				if (GetGlyphOutline(dc.GetHdc(), (uint)charIndex, (uint)GGO_GLYPH_METRICS, out metrics, 0, IntPtr.Zero, ref matrix) != GDI_ERROR)
				{					
					return (int)((float)metrics.gmCellIncX * 1000.0f / (float)pIncFont.FontFamily.GetEmHeight(pIncFont.Style));
					//return metrics.gmBlackBoxX;
				}				
			}
			return 0;
		}				

		[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]		
		struct TEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public byte tmFirstChar;
			public byte tmLastChar;
			public byte tmDefaultChar;
			public byte tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}


		[StructLayout(LayoutKind.Sequential)]
		struct OUTLINETEXTMETRIC
		{
			public uint otmSize;
			public TEXTMETRIC otmTextMetrics;
			public byte otmFiller;
			public PANOSE otmPanoseNumber;
			public uint otmfsSelection;
			public uint otmfsType;
			public int otmsCharSlopeRise;
			public int otmsCharSlopeRun;
			public int otmItalicAngle;
			public uint otmEMSquare;
			public int otmAscent;
			public int otmDescent;
			public uint otmLineGap;
			public uint otmsCapEmHeight;
			public uint otmsXHeight;
			public RECT otmrcFontBox;
			public int otmMacAscent;
			public int otmMacDescent;
			public uint otmMacLineGap;
			public uint otmusMinimumPPEM;
			public POINT otmptSubscriptSize;
			public POINT otmptSubscriptOffset;
			public POINT otmptSuperscriptSize;
			public POINT otmptSuperscriptOffset;
			public uint otmsStrikeoutSize;
			public int otmsStrikeoutPosition;
			public int otmsUnderscoreSize;
			public int otmsUnderscorePosition;
			public uint otmpFamilyName;
			public uint otmpFaceName;
			public uint otmpStyleName;
			public uint otmpFullName;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct PANOSE
		{
			public byte bFamilyType;
			public byte bSerifStyle;
			public byte bWeight;
			public byte bProportion;
			public byte bContrast;
			public byte bStrokeVariation;
			public byte bArmStyle;
			public byte bLetterform;
			public byte bMidline;
			public byte bXHeight;
		}

		[Serializable, StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
		}
		
		private string m_sFontName;
		private string m_sFlags;
		private string m_sBoundsBox;
		private string m_sAscent;

		private int capHeight;
		public int CapHeight
		{
			get { return capHeight; }		
		}
		

		private int italicAngle;
		public int ItalicAngle
		{
			get { return italicAngle; }			
		}

		private RECT fontBBox;

		public AxiomCoders.PdfTemplateEditor.Common.FontMetrics.RECT FontBBox
		{
			get { return fontBBox; }
			set { fontBBox = value; }
		}


		private int stemV;
		public int StemV
		{
			get { return stemV; }
			set { stemV = value; }
		}

		private int flags;
		public int Flags
		{
			get { return flags; }
			set { flags = value; }
		}

		private byte firstChar;
		public byte FirstChar
		{
			get { return firstChar; }
			set { firstChar = value; }
		}

		private byte lastChar;
		public byte LastChar
		{
			get { return lastChar; }
			set { lastChar = value; }
		}

        private EmbeddingLicense embeddingLicense;

        /// <summary>
        /// Embedding license for font
        /// </summary>
        public EmbeddingLicense EmbeddingLicense
        {
            get { return embeddingLicense; }
        }

        private string fullName;
        private string faceName;
        private string familyName;
        private string styleName;

        /// <summary>
        /// Get font data for embedding
        /// </summary>
        /// <param name="pIncFont"></param>
        /// <returns></returns>
        public static byte[] GetFontData(Font pIncFont)
        {
            using (MyDeviceContext dc = new MyDeviceContext())
			{
				Font newFont = new Font(pIncFont.FontFamily, pIncFont.FontFamily.GetEmHeight(pIncFont.Style), pIncFont.Style, GraphicsUnit.Pixel);				
				dc.SelectedFont = newFont;
				
                uint cbSize = GetFontData(dc.GetHdc(), 0, 0, IntPtr.Zero, 0);
                if (cbSize == 0) throw new Win32Exception();
                IntPtr buffer = Marshal.AllocHGlobal((int)cbSize);

                cbSize = GetFontData(dc.GetHdc(), 0, 0, buffer, cbSize);
                byte[] resultingBuffer = new byte[cbSize];
                Marshal.Copy(buffer, resultingBuffer, 0, (int)cbSize);                
                return resultingBuffer;
            }            
        }

        /// <summary>
        /// Get Font Metrics
        /// </summary>
        /// <param name="pIncFont"></param>
		public FontMetrics(Font pIncFont)
		{
			using (MyDeviceContext dc = new MyDeviceContext())
			{
				Font newFont = new Font(pIncFont.FontFamily, pIncFont.FontFamily.GetEmHeight(pIncFont.Style), pIncFont.Style, GraphicsUnit.Pixel);				
				dc.SelectedFont = newFont;

				uint cbSize = GetOutlineTextMetrics(dc.GetHdc(), 0, IntPtr.Zero);
				if (cbSize == 0) throw new Win32Exception();
				IntPtr buffer = Marshal.AllocHGlobal((int)cbSize);
				try
				{
					if (GetOutlineTextMetrics(dc.GetHdc(), cbSize, buffer) != 0)
					{
						OUTLINETEXTMETRIC otm;
						otm = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(buffer,	typeof(OUTLINETEXTMETRIC));
              			this.italicAngle = otm.otmItalicAngle;
                        IntPtr newPtr;
                        try
                        {
                            newPtr = new IntPtr(buffer.ToInt32() + otm.otmpFullName);
                            fullName = Marshal.PtrToStringAuto(newPtr);
                        }
                        catch { }
                        try
                        {
                            newPtr = new IntPtr(buffer.ToInt32() + otm.otmpFaceName);
                            faceName = Marshal.PtrToStringAuto(newPtr);
                        }
                        catch { }
                        try
                        {
                            newPtr = new IntPtr(buffer.ToInt32() + otm.otmpFamilyName);
                            familyName = Marshal.PtrToStringAuto(newPtr);
                        }
                        catch { }
                        try
                        {
                            newPtr = new IntPtr(buffer.ToInt32() + otm.otmpStyleName);
                            styleName = Marshal.PtrToStringAuto(newPtr);
                        }
                        catch { }


						this.capHeight = (int)otm.otmsCapEmHeight;
						this.fontBBox = otm.otmrcFontBox;
						this.stemV = otm.otmTextMetrics.tmMaxCharWidth;
						this.firstChar = otm.otmTextMetrics.tmFirstChar;
						this.lastChar = otm.otmTextMetrics.tmLastChar;
						if (this.LastChar == 0)
						{
							this.LastChar = 255;
						}

                        /// load embedding license
                        /// If bit 1 of otmfsType is set, embedding is not permitted for the font. 
                        /// If bit 1 is clear, the font can be embedded. 
                        /// If bit 2 is set, the embedding is read-only.
                        if ((otm.otmfsType & 0x01) == 0x01)
                        {
                            embeddingLicense = EmbeddingLicense.NotAllowed;
                        }
                        else
                        {
                            embeddingLicense = EmbeddingLicense.Allowed;
                        }
                        if ((otm.otmfsType & 0x02) == 0x02)
                        {
                            embeddingLicense = EmbeddingLicense.AllowedReadOnly;
                        }


						// make flag
						// taken from http://msdn.microsoft.com/en-us/library/dd162774(VS.85).aspx
						if (otm.otmPanoseNumber.bProportion == 0x04)
						{
							flags |= 1;
						}
						if (otm.otmPanoseNumber.bFamilyType == 0x03)
						{
							flags |= 8;
						}
						if (otm.otmPanoseNumber.bFamilyType == 0x02)
						{
							flags |= 32; // nonsymbolic font
						}
						else 
						{
							flags |= 4; // symbolic font
						}

						if (!(otm.otmPanoseNumber.bSerifStyle == 0x0b || otm.otmPanoseNumber.bSerifStyle == 0x0c || otm.otmPanoseNumber.bSerifStyle == 0x0d))
						{
							flags |= 2;
						}                        

					}
				}
				finally
				{
					Marshal.FreeHGlobal(buffer);
				}	
			}
		}
	}		
}
