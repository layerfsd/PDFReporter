using System;
using System.Collections.Generic;
using System.Text;
using AxiomCoders.PdfReports.Exceptions;
using System.Runtime.InteropServices;
using System.IO;

namespace AxiomCoders.PdfReports
{
	/// <summary>
	/// Main class that implements pdfFactoryInterface methods
	/// </summary>
	internal sealed class PdfFactory
	{
		public PdfFactory()
		{

		}

		/// <summary>
		/// Sets reset data callback
		/// </summary>
		/// <param name="resetDataStreamCallback"></param>
		public static void SetResetDataStreamCallback(PdfFactoryInterface.ResetDataStreamCallback resetDataStreamCallback)
		{
			PdfFactoryInterface.SetResetDataStreamCallback(resetDataStreamCallback);
		}

		/// <summary>
		/// Set request data callback
		/// </summary>
		/// <param name="requestDataCallback"></param>
		public static void SetRequestDataCallback(PdfFactoryInterface.RequestDataCallback requestDataCallback)
		{
			PdfFactoryInterface.SetRequestDataCallback(requestDataCallback);
		}

		/// <summary>
		/// Set readDataCallback
		/// </summary>
		/// <param name="readDataCallback"></param>
		public static void SetReadDataCallback(PdfFactoryInterface.ReadDataCallback readDataCallback)
		{
			PdfFactoryInterface.SetReadDataCallback(readDataCallback);
		}

		/// <summary>
		/// Set Initialize DataStream callback
		/// </summary>
		/// <param name="initializeDataStreamCallback"></param>
		public static void SetInitializeDataStreamCallback(PdfFactoryInterface.InitializeDataStreamCallback initializeDataStreamCallback)
		{
			PdfFactoryInterface.SetInitializeDataStreamCallback(initializeDataStreamCallback);
		}

		/// <summary>
		/// This will initialize generator
		/// </summary>
		public static void InitializeGenerator(string companyName, string serialNumber)
		{
			try 
			{
                PdfFactoryInterface.InitializeGenerator(companyName, serialNumber);				
			} 
            catch (Exception ex)
			{
                throw new PdfFactoryEngineException(Strings.FailedGeneratorInitializing, ex);
			}           
		}

		/// <summary>
		/// This will shutdown generator
		/// </summary>
		public static void ShutdownGenerator()
		{
			try 
			{
				PdfFactoryInterface.ShutdownGenerator();
			} 
            catch (Exception ex)
			{
                throw new PdfFactoryEngineException(Strings.FailedGeneratorShutdown, ex);
			}
		}

		/// <summary>
		/// Attach template from File
		/// </summary>
		/// <param name="templateName"></param>
		public static void AttachTemplate(string templateName)
		{
            try
            {
                if (PdfFactoryInterface.AttachTemplateFromFile(templateName) == 0)
                {
                    throw new TemplateUsageException(Strings.FailedAttachingTemplateFromFile);
                }
            }
            catch (TemplateUsageException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new TemplateUsageException(Strings.FailedAttachingTemplateFromFile, ex);
            }
           
		}

        /// <summary>
        /// Attach template from memory
        /// </summary>
        /// <param name="templateName"></param>
        public static void AttachTemplate(Stream templateStream)
        {
            try
            {
                byte[] templateData = new byte[templateStream.Length];
                int templateSize = 0;
                templateStream.Read(templateData, 0, (int)templateStream.Length);

                if (PdfFactoryInterface.AttachTemplateFromMemory(templateData, templateSize) == 0)
                {
                    throw new TemplateUsageException(Strings.FailedAttachingTemplateFromMemory);
                }
            }
            catch (TemplateUsageException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new TemplateUsageException(Strings.FailedAttachingTemplateFromMemory, ex);
            }

        }

        /// <summary>
        /// Set Logging level
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="logLevel"></param>
        public static void SetLogging(bool enable, LoggingLevel logLevel)
        {
            PdfFactoryInterface.SetLogging(enable ? (short)1:(short)0, (short)logLevel);
        }

        /// <summary>
        /// Generate to memory
        /// </summary>
        /// <param name="outputFileName"></param>
        public static byte[] Generate(ref int dataSize)
        {
            try
            {
                IntPtr bufferResult = PdfFactoryInterface.GenerateToMemory(ref dataSize);
                if (bufferResult == IntPtr.Zero)
                {
                    throw new GeneratorFailureException();
                }
                else 
                {
                    byte[] byteResult = new byte[dataSize];
                    Marshal.Copy(bufferResult, byteResult, 0, dataSize);
                    return byteResult;
                }
            }
            catch (PdfFactoryEngineException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PdfFactoryEngineException(Strings.FailedGeneratingPdf, ex);
            }

        }


		/// <summary>
		/// Generate to File 
		/// </summary>
		/// <param name="outputFileName"></param>
		public static void Generate(string outputFileName)
		{
            try
            {
                if (PdfFactoryInterface.GenerateToFile(outputFileName) == 0)
                {
                    throw new GeneratorFailureException();
                }
            }
            catch (PdfFactoryEngineException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PdfFactoryEngineException(Strings.FailedGeneratingPdf, ex);
            }
           
		}

	}
}
