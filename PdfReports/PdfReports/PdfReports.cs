using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AxiomCoders.PdfReports.Exceptions;

namespace AxiomCoders.PdfReports
{
	/// <summary>
	/// Initialize data Stream
	/// </summary>
	/// <param name="parentDataStream"></param>
	/// <param name="dataStreamName"></param>
	/// <param name="itemsCount"></param>
	/// <returns></returns>
	public delegate bool InitializeDataStreamCallback(string parentDataStream, string dataStreamName, ref System.Int32 itemsCount);

	/// <summary>
	/// this is called to indicate you should move to next row. return true to indicate there are still more items
	/// </summary>
	/// <param name="dataStreamName"></param>
	/// <returns></returns> 
	public delegate bool ReadDataCallback(string dataStreamName);

	/// <summary>
	/// when this callback is called you need to return current value as byte array for datastream and column
	/// </summary>
	/// <param name="dataStreamName"></param>
	/// <param name="columnName"></param>
	/// <returns></returns>
	public delegate byte[] RequestBinaryDataCallback(string dataStreamName, string columnName);

    /// <summary>
    /// When this callback is called you need to return current value as string for datastream and column
    /// </summary>
    /// <param name="dataStreamName"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public delegate string RequestStringDataCallback(string dataStreamName, string columnName);
	
	/// <summary>
	/// when this callback is called you need to reset data stream
	/// </summary>
	/// <param name="dataStreamName"></param>	
	public delegate void ResetDataStreamCallback(string dataStreamName);

	
	/*[LicenseProvider(typeof(AxiomLicenser)) /*ToolboxBitmap("AxiomCoders.PdfReports.PdfReporter.bmp") ]	*/
    /// <summary>
    /// PdfREports class is used to generate PDF from given data and template set.
    /// </summary>
	public sealed class PdfReports: IDisposable
	{
        /// <summary>
        /// Serial number used 
        /// </summary>
        private string serialNumber = "";
        private string companyName = "";
        
		/// <summary>
		/// Constructor
		/// </summary>
		public PdfReports()
		{
			InitializeNativeDll();
		}

        /// <summary>
        /// Constructor with serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        public PdfReports(string companyName, string serialNumber): this()
        {
            this.companyName = companyName;
            this.serialNumber = serialNumber;
        }

        /// <summary>
        /// Constructor with serial number and logging
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="enableLogging"></param>
        public PdfReports(string companyName, string serialNumber, bool enableLogging)
            : this()
        {
            this.companyName = companyName;
            this.serialNumber = serialNumber;
            this.Logging = enableLogging;
            try
            {
                InitializeNativeDll();
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }
                Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// Constructor with logging parameter
        /// </summary>
        /// <param name="enableLogging"></param>
        public PdfReports(bool enableLogging)
        {
            this.Logging = enableLogging;
            try
            {
                InitializeNativeDll();
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }
                Dispose();
                throw ex;
            }
        }


		static class NativeMethods
		{
			[DllImport("kernel32.dll")]
			public static extern IntPtr LoadLibrary(string dllToLoad);

			[DllImport("kernel32.dll")]
			public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

			[DllImport("kernel32.dll")]
			public static extern bool FreeLibrary(IntPtr hModule);
		}

        /// <summary>
        /// This will initialize usage of NativeDll as dll is embedded into this assembly as resource
        /// </summary>
		private void InitializeNativeDll()
		{
            if (this.Logging)
            {
                Logger.LogNotice("Initializing native dll usage");
            }

			// Get a temporary directory in which we can store the unmanaged DLL, with
			// this assembly's version number in the path in order to avoid version
			// conflicts in case two applications are running at once with different versions
            AssemblyName asmName = Assembly.GetExecutingAssembly().GetName();

            string dirName = Path.Combine(Path.GetTempPath(), string.Format("AxiomCoders\\PdfReports{0}-{1}-{2}",
                asmName.Version.Major, asmName.Version.Minor, asmName.Version.Revision));

            // try deleting directory with temp PdfFactory.dll files
            try
            {
                if (Directory.Exists(dirName))
                {
                    foreach (string fileName in Directory.GetFiles(dirName))
                    {
                        // try deleting file and if cannot go to next file
                        try
                        {
                            File.Delete(fileName);
                        }
                        catch { }
                    }
                }
                Directory.Delete(dirName);
            }
            catch { }

			if (!Directory.Exists(dirName))
			{
                if (this.Logging)
                {
                    Logger.LogNotice("Creating temp folder for loading pdffactory.dll", dirName);
                }
				Directory.CreateDirectory(dirName);
			}
            Random rnd = new Random();

			string dllPath = Path.Combine(dirName, string.Format("PdfFactory.dll", rnd.Next().ToString()));

			// Get the embedded resource stream that holds the Internal DLL in this assembly.
			// The name looks funny because it must be the default namespace of this project
			// (MyAssembly.) plus the name of the Properties subdirectory where the
			// embedded resource resides (Properties.) plus the name of the file.
			using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(
			  "AxiomCoders.PdfReports.Resources.PdfFactory.dll"))
			{
				// Copy the assembly to the temporary file
				try
				{
                    if (!File.Exists(dllPath))
                    {
                        // remove old temp file if exists                                        
					    using (Stream outFile = File.Create(dllPath))
					    {
						    const int sz = 4096;
						    byte[] buf = new byte[sz];
						    while (true)
						    {
							    int nRead = stm.Read(buf, 0, sz);
							    if (nRead < 1)
								    break;
							    outFile.Write(buf, 0, nRead);
						    }
					    }
                    }
				}
				catch (Exception ex)
				{
					// This may happen if another process has already created and loaded the file.
					// Since the directory includes the version number of this assembly we can
					// assume that it's the same bits, so we just ignore the excecption here and
					// load the DLL. 
                    // If logging is turned on log issue
                    if (this.Logging)
                    {
                        Logger.LogException(ex);
                    }                    
                    throw ex;
				}
			}

			// We must explicitly load the DLL here because the temporary directory 
			// is not in the PATH.
			// Once it is loaded, the DllImport directives that use the DLL will use
			// the one that is already loaded into the process.

            Logger.LogNotice("Loading PdfFactory.dll from {0}", dllPath);
			pdfFactoryModule = NativeMethods.LoadLibrary(dllPath);
            Debug.Assert(pdfFactoryModule != IntPtr.Zero, "Unable to load library " + dllPath);
            Logger.LogNotice("Loading PdfFactory.dll from {0} - Successfully Loaded", dllPath);
		}

        private IntPtr pdfFactoryModule;

        /// <summary>
        /// Stream used to read template file
        /// </summary>
        private Stream templateStream;

        /// <summary>
        /// Stream containing template file. Set this if you want to use template from memory, otherwise use TemplateFileName.
        /// TemplateStream and TemplateFileName will cancel each other
        /// </summary>
        public Stream TemplateStream
        {
            set
            {
                templateStream = value;
                templateFileName = string.Empty;
            }
            get
            {
                return templateStream;
            }
        }

		/// <summary>
		/// template file name
		/// </summary>
		private string templateFileName = "";

		/// <summary>
		/// Template file name. Use this to indicate template is stored in filename. You can also use TemplateStream if your 
        /// template is in memory. Setting this will cancel TemplateStream and set it to null
		/// </summary>		
		public string TemplateFileName
		{
			get { return templateFileName; }
			set
            {
                templateFileName = value;
                templateStream = null;
            }
		}

		public delegate void OnProgreeCallback(int itemProcessed, int totalCount, ref bool cancel);

		/// <summary>
		/// Data sources field used
		/// </summary>
        private List<object> dataSources = new List<object>();

		/// <summary>
		/// DataSource object. You can use reporter with data source or by using appropriate events
		/// </summary>		
		public List<object> DataSources
		{
			get { return dataSources; }			
		}		

		/// <summary>
		/// Called to advance generation progress. This is not yet implemented
		/// </summary>
		public event OnProgreeCallback ProgressChanged;

        /// <summary>
        /// Called internally to move progress
        /// </summary>
        /// <param name="itemsProcessed"></param>
        /// <param name="itemsCount"></param>
        /// <param name="canceled"></param>
        private void OnProgressChanged(int itemsProcessed, int itemsCount, ref bool canceled)
        {
            if (ProgressChanged != null)
            {
                this.ProgressChanged(itemsProcessed, itemsCount, ref canceled);
            }
        }

		private event PdfFactoryInterface.InitializeDataStreamCallback OnInitializeDataStream;      
		private event PdfFactoryInterface.ReadDataCallback OnReadData;       
		private event PdfFactoryInterface.RequestDataCallback OnRequestData;
                

        /// <summary>
        /// This event is called when data stream needs initializing. It is called only once for each data stream
        /// </summary>
		public event InitializeDataStreamCallback InitializeDataStream;

        /// <summary>
        /// This event is called when data should be read. Here you advance to next row in table or something similar.
        /// </summary>
		public event ReadDataCallback ReadData;

        /// <summary>
        /// This event is called when BinaryData is requested. If you return null from this event it is ignored and empty value is used
        /// </summary>
		public event RequestBinaryDataCallback RequestBinaryData;

        /// <summary>
        /// This event is claled when string data is requested. If you return null this is ignored. 
        /// This even and RequestBinaryData event are called for each row where RequestStringData is called first. If you return null from this
        /// then RequestBinaryData is called. 
        /// </summary>
        public event RequestStringDataCallback RequestStringData;


		private void SetDefaultCallbacks()
		{
            if (Logging)
            {
                Logger.LogNotice("Setting default callbacks");
            }
			OnInitializeDataStream += new PdfFactoryInterface.InitializeDataStreamCallback(PdfReporter_InitializeDataStream);
			OnReadData += new PdfFactoryInterface.ReadDataCallback(PdfReporter_ReadData);
			OnRequestData += new PdfFactoryInterface.RequestDataCallback(PdfReporter_RequestData);

            if (Logging)
            {
                Logger.LogNotice("Setting default callbacks - Success");
            }
		}

        /// <summary>
        /// Logging is set to false by default
        /// </summary>
        private bool logging = false;

        /// <summary>
        /// Should pdf reports write log files
        /// </summary>
        public bool Logging
        {
            get { return logging; }
            set { logging = value; }
        }

        private LoggingLevel loggingLevel = LoggingLevel.LogNotice;
        public LoggingLevel LoggingLevel
        {
            get { return loggingLevel; }
            set { loggingLevel = value; }
        }

        /// <summary>
        /// Generate Pdf. If data source is set it will use it. Otherwise it will use callbacks
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="PdfFactoryEngineException">Thrown when engine failed generating for some reason</exception>
        /// <exception cref="TemplateUsageException">Thrown when template file attached is not correct for some reason</exception>
        public byte[] GeneratePdfToMemory(ref int dataSize)
        {
            try
            {
                this.SetDefaultCallbacks();

                if (TemplateFileName.Length == 0)
                {
                    throw new TemplateUsageException(Strings.TemplateFileMissing);
                }

                if (Logging) Logger.LogNotice("SetLogging to pdffactory engine...");
                PdfFactory.SetLogging(this.Logging, this.LoggingLevel);
                
                if (Logging) Logger.LogNotice("Initialize Generator...");
                PdfFactory.InitializeGenerator(this.companyName, this.serialNumber);
                
                if (Logging) Logger.LogNotice("Attach template from file...");
                if (TemplateStream != null)
                {
                    PdfFactory.AttachTemplate(TemplateStream);
                }
                else
                {
                    PdfFactory.AttachTemplate(TemplateFileName);
                }
                
                if (Logging) Logger.LogNotice("Set InitializeDataStream callback...");
                PdfFactory.SetInitializeDataStreamCallback(OnInitializeDataStream);
                PdfFactory.SetReadDataCallback(OnReadData);
                PdfFactory.SetRequestDataCallback(OnRequestData);
                
                if (Logging) Logger.LogNotice("GenerateToFile call...");
                byte[] resultBuffer = PdfFactory.Generate(ref dataSize);
                
                if (Logging) Logger.LogNotice("Shutdown Generator...");                
                PdfFactory.ShutdownGenerator();

                if (Logging) Logger.LogNotice("GeneratePdf - Success...");
                return resultBuffer;
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }              
                throw ex;
            }
        }

		/// <summary>
		/// Generate Pdf. If data source is set it will use it. Otherwise it will use callbacks
		/// </summary>
		/// <param name="fileName"></param>
        /// <exception cref="PdfFactoryEngineException">Thrown when engine failed generating for some reason</exception>
        /// <exception cref="TemplateUsageException">Thrown when template file attached is not correct for some reason</exception>
		public void GeneratePdf(string fileName)
		{
            try
            {
                this.SetDefaultCallbacks();

                if (TemplateFileName.Length == 0)
                {
                    throw new TemplateUsageException(Strings.TemplateFileMissing);
                }

                if (Logging) Logger.LogNotice("SetLogging to pdffactory engine...");
                PdfFactory.SetLogging(this.Logging, this.LoggingLevel);
                if (Logging) Logger.LogNotice("Initialize Generator...");
                PdfFactory.InitializeGenerator(this.companyName.Trim(), this.serialNumber.Trim());
                if (Logging) Logger.LogNotice("Attach template from file...");
                if (TemplateStream != null)
                {
                    PdfFactory.AttachTemplate(TemplateStream);
                }
                else
                {
                    PdfFactory.AttachTemplate(TemplateFileName);
                }

                if (Logging) Logger.LogNotice("Set InitializeDataStream callback...");
                PdfFactory.SetInitializeDataStreamCallback(OnInitializeDataStream);
                PdfFactory.SetReadDataCallback(OnReadData);
                PdfFactory.SetRequestDataCallback(OnRequestData);
                if (Logging) Logger.LogNotice("GenerateToFile call...");
                PdfFactory.Generate(fileName);
                if (Logging) Logger.LogNotice("Shutdown Generator...");
                PdfFactory.ShutdownGenerator();
                if (Logging) Logger.LogNotice("GeneratePdf - Success...");
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }
                // re-throw exception               
                throw ex;
            }
        }

		/// <summary>
		/// This holds index of item in each initialized data stream
		/// </summary>
		private Dictionary<string, int> dataStreamCounter = new Dictionary<string, int>();

		/// <summary>
		/// Sizes of data streams
		/// </summary>
		private Dictionary<string, int> dataStreamSizes = new Dictionary<string, int>();

        /// <summary>
        /// This will get items could from data streams
        /// </summary>
        /// <param name="datasources"></param>
        /// <param name="parentStream"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        private int GetItemsCount(List<object> dataSources, string parentStream, string stream)
        {
            int res = 0;
            foreach (object obj in dataSources)
            {
                res = GetItemsCountForObject(obj, parentStream, stream);
                // if we found some count return that count
                if (res > 0)
                {
                    return res;
                }
            }
            return res;
        }

		/// <summary>
		/// Count how many items we have
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="stream"></param>
		/// <returns>this will return 0 if no stream is found</returns>
		private int GetItemsCountForObject(object obj, string parentStream, string stream)
		{
			// for IList
            if(obj == null)
            {
                return 0;
            }

			if (obj.GetType().GetInterface("IList") != null)
            {
                if (obj.GetType().IsGenericType)
                {
                    Type t = obj.GetType().GetGenericArguments()[0];
                    if (t.Name.ToLower() == stream.ToLower())
                    {
                        System.Collections.IList list = obj as System.Collections.IList;
                        if (dataStreamSizes.ContainsKey(stream))
                        {
                            dataStreamSizes[stream] = list.Count;
                        }
                        else
                        {
                            dataStreamSizes.Add(stream, list.Count);
                        }
                        return list.Count;
                    }
                    else
                    {
                        // maybe some property has this stream
                        System.Collections.IList list = obj as System.Collections.IList;

                        foreach (PropertyInfo pi in t.GetProperties())
                        {
                            int index = 0;
                            if (parentStream != null && dataStreamCounter.ContainsKey(parentStream))
                            {
                                index = dataStreamCounter[parentStream];
                                try
                                {
                                    int r = GetItemsCountForObject(pi.GetValue(list[index], null), parentStream, stream);
                                    if (r != 0)
                                    {
                                        return r;
                                    }
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }              
            }
			// for DataTable
			else if (obj is DataTable)
			{
				DataTable tbl = obj as DataTable;
				if (tbl.TableName.ToLower() == stream.ToLower())
				{
					if (dataStreamSizes.ContainsKey(stream))
					{
						dataStreamSizes[stream] = tbl.Rows.Count;
					}
					else
					{
						dataStreamSizes.Add(stream, tbl.Rows.Count);
					}
					return tbl.Rows.Count;
				}
			}
			// for DataSet
			else if (obj is DataSet)
			{
				DataSet ds = obj as DataSet;
				foreach (DataTable tbl in ds.Tables)
				{
					if (tbl.TableName.ToLower() == stream.ToLower())
					{
						if (dataStreamSizes.ContainsKey(stream))
						{
							dataStreamSizes[stream] = tbl.Rows.Count;
						}
						else
						{
							dataStreamSizes.Add(stream, tbl.Rows.Count);
						}
						return tbl.Rows.Count;
					}
				}
			}
            // if object is actual one item only stream
            else if (obj.GetType().Name.ToLower() == stream.ToLower())
            {
                if (dataStreamSizes.ContainsKey(stream))
                {
                    dataStreamSizes[stream] = 1;
                }
                else
                {
                    dataStreamSizes.Add(stream, 1);
                }
                return 1;
            }
			return 0;
		}

		private GCHandle handle;

        /// <summary>
        /// Convert to IntPtr
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
		private IntPtr ConvertToIntPtr(object obj, ref Int32 dataSize)
		{
			if (obj == null)
			{
				return IntPtr.Zero;
			}
			else
			{
				if (obj.GetType().Name.ToLower() == "byte[]")
				{
					dataSize = ((byte[])obj).Length;

					byte[] arr = (byte[])obj;

					// if we had handle release it to take new one
					if (handle.IsAllocated)
					{
						handle.Free();
					}

					handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
					return handle.AddrOfPinnedObject();
				}
                else
                {                    
                    byte[] res = System.Text.ASCIIEncoding.ASCII.GetBytes(obj.ToString());

                    if (handle.IsAllocated)
                    {
                        handle.Free();
                    }

                    handle = GCHandle.Alloc(res, GCHandleType.Pinned);
                    return handle.AddrOfPinnedObject();
                }
			}
		}

		/// <summary>
		/// Convert object value to string
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="dataSize"></param>
		/// <returns></returns>
		private string ConvertToString(object obj, ref Int32 dataSize)
		{
			if (obj.GetType().Name == "byte[]")
			{
				string res = Convert.ToBase64String((byte[])obj);
				dataSize = res.Length;
				return res;
			}
			else
			{
				string res = obj.ToString();
				dataSize = res.Length;
				return res;
			}
		}

        /// <summary>
        /// Get Value for specified stream in all data sources
        /// </summary>
        /// <param name="dataSources"></param>
        /// <param name="stream"></param>
        /// <param name="column"></param>
        /// <param name="index"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        private IntPtr GetValue(List<object> dataSources, string stream, string column, int index, ref System.Int32 dataSize)
        {
            IntPtr res = IntPtr.Zero;
            foreach (object obj in dataSources)
            {                          

                res = GetValueForStream(obj, stream, column, index, ref dataSize);
                if (res != IntPtr.Zero)
                {
                    return res;
                }
            }
            return res;
        }

		/// <summary>
		/// This will get value from data source specified in first parameter
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="stream"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private IntPtr GetValueForStream(object obj, string stream, string column, int index, ref System.Int32 dataSize)
		{
            if(obj == null)
            {
                return IntPtr.Zero;
            }

			// for IList
			if (obj.GetType().GetInterface("IList") != null)
			{
				if (obj.GetType().IsGenericType)
				{
					Type t = obj.GetType().GetGenericArguments()[0];
					if (t.Name.ToLower() == stream.ToLower())
					{
						System.Collections.IList list = obj as System.Collections.IList;
						foreach (PropertyInfo pi in t.GetProperties())
						{
							if (pi.Name.ToLower() == column.ToLower())
							{
                                try
                                {
                                    return ConvertToIntPtr(pi.GetValue(list[index], null), ref dataSize);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    return IntPtr.Zero;
                                }
																
							}
						}
					}
					else
					{
						// maybe some property has this stream
						System.Collections.IList list = obj as System.Collections.IList;

						foreach (PropertyInfo pi in t.GetProperties())
						{
                            // get index for this current stream
                            int currentIndex = this.dataStreamCounter[t.Name];

                            // check recursively
							IntPtr ptr = GetValueForStream(pi.GetValue(list[currentIndex], null), stream, column, index, ref dataSize);
							if (ptr != IntPtr.Zero)
							{
								return ptr;
							}
						}
					}
				}
			}		
			// for DataTable
			else if (obj is DataTable)
			{
				DataTable tbl = obj as DataTable;
				if (tbl.TableName.ToLower() == stream.ToLower())
				{					
					foreach(DataColumn col in tbl.Columns)
					{
						if (col.ColumnName.ToLower() == column.ToLower())
						{
							return ConvertToIntPtr(tbl.Rows[index][col], ref dataSize);																													
						}
					}
				}
			}
			// for DataSet
			else if (obj is DataSet)
			{
				DataSet ds = obj as DataSet;
				foreach(DataTable tbl in ds.Tables)
				{
					if (tbl.TableName.ToLower() == stream.ToLower())
					{
						foreach (DataColumn col in tbl.Columns)
						{
							if (col.ColumnName.ToLower() == column.ToLower())
							{
								return ConvertToIntPtr(tbl.Rows[index][col], ref dataSize);
								//string res = tbl.Rows[index][col].ToString();
								//dataSize = res.Length;
								//return res;
							}
						}
					}
				}
			}
            else if (obj.GetType().Name.ToLower() == stream.ToLower())
            {
                foreach (PropertyInfo pi in obj.GetType().GetProperties())
                {
                    if (pi.Name.ToLower() == column.ToLower())
                    {
                        return ConvertToIntPtr(pi.GetValue(obj, null), ref dataSize);
                    }
                }
            }
			return IntPtr.Zero;
		}
			
		/// <summary>
		/// This will get data from data source all call appropriate event to user
		/// </summary>
		/// <param name="dataStreamName"></param>
		/// <param name="columnName"></param>
		/// <returns></returns>
		IntPtr PdfReporter_RequestData(string dataStreamName, string columnName, ref System.Int32 dataSize)
		{
            try
            {
                if (DataSources.Count > 0)
                {
                    // 1. find value of index for data stream
                    if (dataStreamCounter.ContainsKey(dataStreamName))
                    {
                        int index = dataStreamCounter[dataStreamName];
                        IntPtr value = GetValue(DataSources, dataStreamName, columnName, index, ref dataSize);
                        return value;
                        //PropertyInfo pi = FindProperty(this.dataSource, dataStreamName);

                        // 2. find column in that object
                    }
                    else
                    {                    

                        dataSize = 0;
                        return IntPtr.Zero;
                    }
                }
                else
                {
                    // we first request string data and after that if null is returned we require binary data
                    if (RequestStringData != null)
                    {
                        string result = RequestStringData(dataStreamName, columnName);
                        if (result != null)
                        {
                            return ConvertToIntPtr(result, ref dataSize);
                        }
                    }
                    // call event
                    if (RequestBinaryData != null)
                    {
                        byte[] arr = this.RequestBinaryData(dataStreamName, columnName);
                        if (arr != null)
                        {
                            return ConvertToIntPtr(arr, ref dataSize);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logging)
                {
                    Logger.LogException(ex);
                }              
                throw;
            }

			dataSize = 0;
			return IntPtr.Zero;
		}

		/// <summary>
		/// This will read data from data stream or call appropriate event to user
		/// </summary>
		/// <param name="dataStreamName"></param>
		/// <returns></returns>
		int PdfReporter_ReadData(string dataStreamName)
		{
            bool cancel = false;
            try
            {
                if (DataSources.Count > 0)
                {
                    if (dataStreamCounter.ContainsKey(dataStreamName))
                    {
                        dataStreamCounter[dataStreamName]++;
                        if (dataStreamSizes.ContainsKey(dataStreamName))
                        {
                            OnProgressChanged(dataStreamCounter[dataStreamName], dataStreamSizes[dataStreamName], ref cancel);
                            if (dataStreamSizes[dataStreamName] <= dataStreamCounter[dataStreamName] || cancel)
                            {

                                return 0;
                            }
                            else
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }                  
                }
                else
                {
                    // call event
                    if (this.ReadData != null)
                    {
                        OnProgressChanged(0, 0, ref cancel);
                        if (cancel)
                        {
                            return 0;
                        }
                        return this.ReadData(dataStreamName) == true ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }                
                throw;
            }
			return 0;
		}

		

		int PdfReporter_InitializeDataStream(string parentDataStream, string dataStreamName, ref System.Int32 itemsCount)
		{
            bool cancel = false;
            try
            {                
                if (DataSources.Count > 0)
                {
                    if (dataStreamName != null && dataStreamName.Length > 0)
                    {
                        if (!dataStreamCounter.ContainsKey(dataStreamName))
                        {
                            dataStreamCounter.Add(dataStreamName, -1); // we start just before first item					
                            itemsCount = GetItemsCount(DataSources, parentDataStream, dataStreamName);
                            OnProgressChanged(0, itemsCount, ref cancel);

                        }
                        else
                        {
                            dataStreamCounter[dataStreamName] = -1;
                            itemsCount = GetItemsCount(DataSources, parentDataStream, dataStreamName);
                            OnProgressChanged(0, itemsCount, ref cancel);
                        }
                    }                   
                }
                else
                {                   
                    // call event
                    if (this.InitializeDataStream != null)
                    {
                        int res = this.InitializeDataStream(parentDataStream, dataStreamName, ref itemsCount) == true ? 1 : 0;
                        if (res == 1)
                        {
                            OnProgressChanged(0, itemsCount, ref cancel);
                        }
                        if (cancel)
                        {
                            return 0;
                        }
                        else
                        {
                            return res;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.Logging)
                {
                    Logger.LogException(ex);
                }              
                throw;              
            }

            if (cancel)
            {
                return 0;
            }
            else
            {
                return 1;
            }			
		}

        #region IDisposable Members

        public void Dispose()
        {
            Logger.LogNotice("Unloading pdffactory.dll");
            if (!NativeMethods.FreeLibrary(pdfFactoryModule))
            {
                Logger.LogNotice("Unloading pdffactory.dll - Failed");
            }
            else
            {
                Logger.LogNotice("Unloading pdffactory.dll - Success");
            }
        }

        #endregion
    }
}
