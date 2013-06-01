using System;
using System.Collections.Generic;
using System.Text;
using AxiomCoders.PdfReports;




namespace PdfReportsTest
{
    class Program
    {
        /// <summary>
        /// This is simple application that will generate output file "PdfReportsTest.pdf" in app\bin\debug folder
        /// from template file "PdfReportsTest.prtp" located in project folder.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Write("\nStarting generator...");
            PdfReports Reporter = new PdfReports(false);
            Console.Write("\nAdding template file: PdfReportsTest.prtp");
            Reporter.TemplateFileName = "PdfReportsTest.prtp";
            Console.Write("\nAdding data...");
            Reporter.DataSources.Add(GeneratePersons());
            Console.Write("\nGenerating pdf file: PdfReportsTest.pdf");
            Reporter.GeneratePdf("PdfReportsTest.pdf");
            Console.Write("\nFinished!\n");
        }

        /// <summary>
        /// This function add some data in list that will be used as "Data Stream" for this test.
        /// Data Stream represents simple phone book database.
        /// </summary>
        /// <returns></returns>
        private static List<Person> GeneratePersons()
        {
            List<Person> persons = new List<Person>();
            persons.Add(new Person("Nicolas Bangallow", "Mike Arandjica 4/13"));
            persons.Add(new Person("Maria Schefield", "SunSet Boulevard 11034"));
            persons.Add(new Person("Kenya Woodtrust", "London Street 12C"));
            persons.Add(new Person("Nathan Molland", "Avenija 5"));
            persons.Add(new Person("Sonya Presgraves", "Bulevar Oslobodjenja 10"));
            persons.Add(new Person("Jeorge Milkinson", "Stefana Sremca 13"));

            return persons;
        }
    }



#region Generation Classes, used to hold data needed for this test
    
    
    /// <summary>
    /// Simple class that have properties like Name, Address, List of phone numbers.
    /// </summary>
    class Person
    {
        public Person(string inName, string inAddress)
        {
            Name = inName;
            Address = inAddress;
            Phones = new List<Phone>();

            for(int i = 0; i < 4; i++ )
            {
                Phones.Add(new Phone(string.Format("{0}\\{1}-{2}", rnd.Next(100, 300).ToString(), rnd.Next(100, 1000).ToString(), rnd.Next(0, 1000).ToString())));
            }
        }

        private Random rnd = new Random();

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
       
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
       
        private List<Phone> phones;
        public List<Phone> Phones
        {
            get { return phones; }
            set { phones = value; }
        }      
    }

    /// <summary>
    /// Class used for holding phone number
    /// </summary>
    class Phone
    {
        public Phone(string inNumber)
        {
            Number = inNumber;
        }


        private string number;
        public string Number
        {
            get { return number; }
            set { number = value; }
        }
    }
#endregion
}
