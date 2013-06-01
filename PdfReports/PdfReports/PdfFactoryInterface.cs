using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace AxiomCoders.PdfReports
{

    public enum LoggingLevel
    {
        LogNotice = 0,
        LogWarning = 1,
        LogError = 2
    }

	/// <summary>
	/// this is wrapper class for PdfFactory.dll
	/// </summary>
	internal sealed class PdfFactoryInterface
	{

		public delegate System.Int32 InitializeDataStreamCallback([In, MarshalAs(UnmanagedType.LPStr)] string parentDataStream,
														 [In, MarshalAs(UnmanagedType.LPStr)] string dataStreamName,
														 [In, Out] ref System.Int32 itemsCount);
		// this is called to indicate you should move to next row. return true to indicate there are still more items
		public delegate System.Int32 ReadDataCallback([MarshalAs(UnmanagedType.LPStr)] string dataStreamName);
		// when this callback is called you need to return current value for datastream and column
				
		public delegate IntPtr RequestDataCallback([MarshalAs(UnmanagedType.LPStr)]string dataStreamName, [MarshalAs(UnmanagedType.LPStr)]string columnName, [In, Out]ref System.Int32 dataSize);
		// when this callback is called you need to reset data stream
		public delegate void ResetDataStreamCallback([MarshalAs(UnmanagedType.LPStr)]string dataStreamName);
		public delegate void GenerateProgressCallback(float currentProgress);


		public PdfFactoryInterface()
		{

		}


		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetResetDataStreamCallback([In, MarshalAs(UnmanagedType.FunctionPtr)]ResetDataStreamCallback resetDataStreamCallback);

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetRequestDataCallback([In, MarshalAs(UnmanagedType.FunctionPtr)]RequestDataCallback requestDataCallback);

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetReadDataCallback([In, MarshalAs(UnmanagedType.FunctionPtr)]ReadDataCallback readDataCallback);

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetInitializeDataStreamCallback([In, MarshalAs(UnmanagedType.FunctionPtr)] InitializeDataStreamCallback initializeDataStreamCallback);

		[DllImport("PdfFactory.dll", CallingConvention=CallingConvention.Cdecl) ]
		public static extern short InitializeGenerator([In, MarshalAs(UnmanagedType.LPStr)] string companyName, [In, MarshalAs(UnmanagedType.LPStr)] string serialNumber);

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ShutdownGenerator();

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int AttachTemplateFromFile([In, MarshalAs(UnmanagedType.LPStr)] string templateName);

        [DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AttachTemplateFromMemory([In, MarshalAs(UnmanagedType.LPStr)] byte[] templateData, int templateSize);       

		[DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int GenerateToFile([In, MarshalAs(UnmanagedType.LPStr)] string outputFileName);

        [DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GenerateToMemory([In, Out] ref System.Int32 dataSize);

        [DllImport("PdfFactory.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLogging(short enable, short logLevel);

	}
}
