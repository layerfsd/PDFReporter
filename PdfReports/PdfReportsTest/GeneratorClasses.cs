using System;
using System.Collections.Generic;
using System.Text;

namespace axiomPdfTest
{
	public class Company
	{
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string logo;
		public string Logo
		{
			get { return logo; }
			set { logo = value; }
		}
		private byte[] binaryLogo;
		public byte[] BinaryLogo
		{
			get { return binaryLogo; }
			set { binaryLogo = value; }
		}
		private string description;
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public override string ToString()
		{
			return string.Format("{0} {1} [{2}]", id, name, description);
		}
	}

	public class Manufacturer
	{
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		private string description;
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		private List<Product> products = new List<Product>();
		public List<Product> Products
		{
			get { return products; }			
		}

		public override string ToString()
		{
			return string.Format("{0} {1} [{2}]", id, name, description);
		}
	}

	public class Product
	{
		private int manufacturerId;
		public int ManufacturerId
		{
			get { return manufacturerId; }
			set { manufacturerId = value; }
		}

		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string description;
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
		private float quantity;
		public float Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		private float price = 0;
		public float Price
		{
			get { return price; }
			set { price = value; }
		}

        private List<Price> prices = new List<Price>();

        public List<Price> Prices
        {
          get { return prices; }
          set { prices = value; }
        }

		public override string ToString()
		{
			return string.Format("{0}, {1} - Qnt:{2}, Price:{3}, [{4}]", id, name, quantity, price, description);
		}
	}

	public class Price
	{
		private int productId;
		public int ProductId
		{
			get { return productId; }
			set { productId = value; }
		}
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
        private float value = 0;
		public float Value
		{
			get { return value; }
			set { this.value = value; }
		}
		
		private string date;
		public string Date
		{
			get { return date; }
			set { date = value; }
		}
		private float quantity;
		public float Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		public override string ToString()
		{
			return string.Format("{0} Date:{1}, Qnt:{2}, Price:{3}", id, date, quantity, value);
		}     

	}

	public class Bank
	{
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string description;
		public string Description
		{
			get { return description; }
			set { description = value; }
		}
		private string address;
		public string Address
		{
			get { return address; }
			set { address = value; }
		}
		private List<Client> clients = new List<Client>();
		public List<Client> Clients
		{
			get { return clients; }			
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, Addr:{2} [{3}]", id, name, address, description);
		}
	}

	public class Client
	{
		private int bankId;
		public int BankId
		{
			get { return bankId; }
			set { bankId = value; }
		}
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private byte[] photoInline;
		public byte[] PhotoInline
		{
			get { return photoInline; }
			set { photoInline = value; }
		}
		private string description;
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		private List<Account> accounts = new List<Account>();
		public List<Account> Accounts
		{
			get { return accounts; }			
		}

		public override string ToString()
		{
			return string.Format("{0} {1} [{2}]", id, name, description);
		}
	}

	public class Account
	{
		private int id;
		public int Id
		{
			get { return id; }
			set { id = value; }
		}
		private int clientId;
		public int ClientId
		{
			get { return clientId; }
			set { clientId = value; }
		}
		private string accNumber;
		public string AccNumber
		{
			get { return accNumber; }
			set { accNumber = value; }
		}
		private string lastChange;
		public string LastChange
		{
			get { return lastChange; }
			set { lastChange = value; }
		}
		private float balance;
		public float Balance
		{
			get { return balance; }
			set { balance = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, LastChange:{2}, Balance:{3}", id, AccNumber, lastChange, Balance);
		}
	}
	
	/// <summary>
	/// This will generate data
	/// </summary>
	public static class Generator
	{
		private static string[] firstNames = new string[] { "Mathew", "Carmella", "Kurt", "Serena", "Zelma", "Penelope", "Javier", 
			"Jeorge", "Nicolas", "David", "Ted", "Christian", "Ana", "Darren", "Kenya", "Johana", "Sebastian", "Ditrich", 
			"Emanuel", "Eluise", "Elise", "Elenore", "Maria", "Janice", "Luke", "Drake", "Will", "Wendy", "Sara", 
			"Sonya", "Crendy", "Cody", "Lance", "Lucy", "Anabel", "George", "Hugh", "Tia", "Nathan", "Florian" };

		private static string[] lastNames = new string[] { "Bangallow", "Edelen", "Sharber", "Gleeson", "Woodtrust", "Seearch", "Cohan", "Rusum", "Molland", "Holly", "Heidreich", 
            "Liskey", "Oncly", "Pavlak", "Islen", "Tully", "Abbe", "Lady", "Schefield", 
            "Milkinson", "Presgraves", "Gravelword", "Dismukes", "Hogan" };

		private static string[] descriptions = new string[] { "This is awesome thing", "Exceptional to have this", "Must join this club", 
            "Invitations to this are not required", "This is the best thing to see", "Something worth of having", "There is nothing like home", "Always check this twice", "Comes in different variaties", 
		"Always enjoy it, never lose it", "Its nice", "A MUST HAVE thing", "Very bad non reliable", "Entering level is really hard for this", "Uses 10% discount",
		"Uses 20% discount", "Buy one, get one free", "This is B class", "This is A Class" };

		private static string[] addresses = new string[] { "Mike Arandjica 4/13", "Bulevar Oslobodjenja 10", "Mite Andrica 10B", "Stefana Sremca 13", 
			"SunSet Boulevard 11034", "River Street AKF190", "London Street 12C", "Trg Majke Jevrosime 1", "Avenija 5", "Bil Klintona 34" };

		private static string[] companyNames = new string[] { "Astral Ltd", "AxiomCoders", "Azirious Soft", "TechnoMarket", "Bee Ltd", "Megasoft", 
			"Ziro Ltd.", "Nitesa Technologies", "Funky Music Instruments Ltd.", "Exotic Tech", "Furious Lid", "Nikees", "Oukland Public Producers", "Bennetony", "McLaren",
			"Communication Globe", "Word Press Studios", "General Electronics" };

		private static string[] productNames = new string[] { "Tooth Brush 100NB", "Mouse Genius A4", "Samsung LCD 21\"", "Jeans Ripley XNO", 
		"XBOXY 360ZX", "Sony Playstation 3", "Nitesa Washing Machine LK2300", "Nivea Hand Cream 100g", "Pistol CZ33", "Ice cream Mochito", "Bacardi Dark Rum 1l",
		"Grand Piano System 10LE", "Toshiba Satellite A345N", "Dell ISOK223", "Hybernation armour 10cc", "Chocolate Milk 1l", "Sour cream 0.5l", 
		"Minced pork meat 1kg", "Fuji Apples", "Casio CTK810", "Casio CTK4000 Keyboard", "Shower soup MaxFactor", "Dinning table IKEA", 
        "SF Oranges", "Sparkling Water 1l", "Strawberry icecream 1kg", "Entertainment studio LK10", "Panosanoic Phone 3004K", 
        "Gentlmens Hat", "Dinning Plate Green", "Snorkling full gear KW1"};

		private static Random rnd = new Random();
		private static string GenerateName()
		{
			int fid = rnd.Next(firstNames.Length-1);
			int lid = rnd.Next(lastNames.Length-1);
			return firstNames[fid] + " " + lastNames[lid];
		}

		private static string GenerateProductName()
		{
			return productNames[rnd.Next(productNames.Length - 1)];
		}

		private static string GenerateCompanyName()
		{
			return companyNames[rnd.Next(companyNames.Length - 1)];
		}

		private static string GenerateDescription()
		{
			return descriptions[rnd.Next(descriptions.Length-1)];
		}

		private static float GenerateQuantity()
		{
			return (float)(rnd.NextDouble() * 100 + 5.0);
		}

		private static float GeneratePrice()
		{
			return (float)rnd.Next(50, 10000) / 2.0f;
		}

		private static string GenerateLogo()
		{
			return "";
		}

		private static string GenerateAddress()
		{
			return addresses[rnd.Next(addresses.Length - 1)];
		}

		private static Company GenerateCompany()
		{
			Company newCompany = new Company();
			newCompany.Description = GenerateDescription();
			newCompany.Name = GenerateCompanyName();
			newCompany.Id = lastCompanyId++;
			newCompany.Logo = GenerateLogo();
			return newCompany;
		}

		private static Manufacturer GenerateManufacturer()
		{
			Manufacturer newMan = new Manufacturer();
			newMan.Name = GenerateCompanyName();
			newMan.Id = lastManufacturerId++;
			newMan.Description = GenerateDescription();
			return newMan;
		}

		private static Product GenerateProduct(Manufacturer manufacturer)
		{
			Product newProduct = new Product();
			if (manufacturer != null)
			{
				newProduct.ManufacturerId = manufacturer.Id;
			}
			newProduct.Name = GenerateProductName();
			newProduct.Description = GenerateDescription();
			newProduct.Id = lastProductId++;
			newProduct.Price = GeneratePrice();
			newProduct.Quantity = GenerateQuantity();			
			return newProduct;
		}

		private static Price GeneratePrice(Product owner)
		{
			Price newPrice = new Price();
			if (owner != null)
			{
				newPrice.ProductId = owner.Id;
			}
			newPrice.Id = lastPriceId++;
			newPrice.Value = GeneratePrice();
			newPrice.Quantity = GenerateQuantity();
			newPrice.Date = DateTime.FromBinary(rnd.Next()).ToString();
			return newPrice;
		}

		private static Bank GenerateBank()
		{
			Bank newBank = new Bank();
			newBank.Address = GenerateAddress();
			newBank.Description = GenerateDescription();
			newBank.Id = lastBankId++;
			newBank.Name = GenerateCompanyName();
			return newBank;
		}

		private static Client GenerateClient(Bank owner)
		{
			Client newClient = new Client();
			newClient.Name = GenerateName();
			newClient.Description = GenerateDescription();
			newClient.BankId = owner.Id;
			newClient.Id = lastClientId++;			
			return newClient;
		}

		private static Account GenerateAccount(Client owner)
		{
			Account newAccount = new Account();
			newAccount.AccNumber = string.Format("{0}-{1}-{2}", rnd.Next(10, 10000).ToString(), rnd.Next(100, 1000).ToString(), rnd.Next(0, 1000000).ToString());
			newAccount.Id = lastAccountId++;
			newAccount.ClientId = owner.Id;
			newAccount.Balance = GeneratePrice();
			newAccount.LastChange = string.Format("{0}/{1}/{2} {3}:{4}:{5}", rnd.Next(1, 28), rnd.Next(1,12), rnd.Next(1996, 2009), rnd.Next(0,23), rnd.Next(0,59), rnd.Next(0,59)); 
			return newAccount;
		}

		private static int lastAccountId = 1;
		private static int lastClientId = 1;
		private static int lastBankId = 1;
		private static int lastPriceId = 1;
		private static int lastProductId = 1;
		private static int lastCompanyId = 1;
		private static int lastManufacturerId = 1;


		private static Company generatedCompany;
		public static axiomPdfTest.Company GeneratedCompany
		{
			get { return generatedCompany; }			
		}

		private static List<Manufacturer> manufacturers = new List<Manufacturer>();
		public static List<Manufacturer> Manufacturers
		{
			get { return manufacturers; }			
		}


        private static List<Product> products = new List<Product>();

        public static List<Product> Products
        {
            get { return Generator.products; }
            set { Generator.products = value; }
        }

        private static List<Bank> banks = new List<Bank>();

        public static List<Bank> Banks
        {
          get { return Generator.banks; }
          set { Generator.banks = value; }
        }

      

		/// <summary>
		/// Generate demo 1
		/// </summary>
		public static void GenerateDemo1(int numberOfRows)
		{
			generatedCompany = GenerateCompany();
            int tmp = rnd.Next(numberOfRows, (int)(numberOfRows * 1.1));			
			for(int i = 0; i < tmp; i++)
			{
				Manufacturer man = GenerateManufacturer(); 
				manufacturers.Add(man);
				// create products
				for (int j = 0; j < numberOfRows; j++)
				{
					Product product = GenerateProduct(man);
					man.Products.Add(product);
				}
			}
		}

        /// <summary>
        /// Generate Demo2
        /// </summary>
        /// <param name="numberOfRows"></param>
		public static void GenerateDemo2(int numberOfRows)
		{
            generatedCompany = GenerateCompany();
            for (int i = 0; i < numberOfRows; i++)
            {
                Product product = GenerateProduct(null);
                products.Add(product);
                // create prices (each product have 1 - 10% of number of rows prices
                int tmp = rnd.Next(numberOfRows, (int)(numberOfRows * 1.1));
                for (int j = 0; j < tmp; j++)
                {
                    Price price = GeneratePrice(product);
                    product.Prices.Add(price);                    
                }
            }
		}

        /// <summary>
        /// Generate Demo3
        /// </summary>
        /// <param name="numberOfRows"></param>
        public static void GenerateDemo3(int numberOfRows)
		{
            int noOfBanks = numberOfRows;// rnd.Next(1, numberOfRows / 10);
            for(int i = 0; i < noOfBanks; i++)
            {
                Bank bank = GenerateBank();
                banks.Add(bank);
                // generate 10% - 300% of clients
                int noOfClients = rnd.Next(noOfBanks, (int)(noOfBanks * 1.1));
                for(int j = 0; j < noOfClients; j++)
                {
                    Client client = GenerateClient(bank);
                    bank.Clients.Add(client);
                    // for each client generate (1 - 10% accounts)
                    int noOfAccounts = rnd.Next(noOfClients, (int)(noOfClients * 1.1));
                    for(int k = 0; k < noOfAccounts; k++)
                    {
                        Account account = GenerateAccount(client);
                        client.Accounts.Add(account);
                    }
                }
            }
		}
	}
}
