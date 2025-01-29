using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptBook;

namespace ReceiptBookUI
{
	public class ReceiptBookView
	{
		private ReceiptBookView() { }

		private ReceiptBook.ReceiptBook receiptBook = new ReceiptBook.ReceiptBook();

		private static ReceiptBookView? s_instance;

		public static ReceiptBookView Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new ReceiptBookView();
				}

				return s_instance;
			}
		}

		public void ShowMenu()
		{
			
			while (true)
			{
				Console.WriteLine("Hello! Choose option:");
				Console.WriteLine("1. See all recipies");
				Console.WriteLine("2. Add new recipy");
				Console.WriteLine("3. Edit recipy");
				Console.WriteLine("4. Delete recipy");
				Console.WriteLine("5. Exit");

				string choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						receiptBook.PrintAllReceipts();
						break;
					case "2":
						AddNewRecipy();
						break;
					case "3":
						//EditRecipy()
						break;
					case "4":
						//DeleteRecipy()
						break;
					case "5":
						Console.WriteLine("Exiting");
						return;
					default:
						break;
				}
			}
		}

		public void AddNewRecipy()
		{
			Console.Write("Recipy name: ");
			string receiptName = Console.ReadLine();
			Console.Write("Description: ");
			string receiptDescription = Console.ReadLine();
			Console.Write("Instructions: ");
			string receiptInstructions = Console.ReadLine();
			receiptBook.AddReceipt(new Receipt(0, receiptName, receiptDescription, receiptInstructions));
		}
	}
}
