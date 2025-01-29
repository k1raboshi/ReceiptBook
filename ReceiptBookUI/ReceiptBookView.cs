using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptBook;
using ReceiptBook.DTO;

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

		public void Start()
		{
			while (true)
			{
				Console.WriteLine("Hello! Look at your recipies:");
				receiptBook.PrintAllReceipts();
				Console.WriteLine("What do you wanna do?");
				Console.WriteLine("1. Add new recipy");
				Console.WriteLine("2. Edit recipy");
				Console.WriteLine("3. Delete recipy");
				Console.WriteLine("4. Exit");
				ChoiceMenu();
			}
		}

		public void ChoiceMenu()
		{
			string choice = Console.ReadLine();
			switch (choice)
			{
				case "1":
					AddNewRecipy();
					break;
				case "2":
					EditRecipy();
					break;
				case "3":
					DeleteRecipy();
					break;
				case "4":
					Console.WriteLine("Exiting");
					return;
				default:
					break;
			}
		}
		public void AddNewRecipy()
		{
			Console.WriteLine("Let's add new recipy.");
			Console.Write("Recipy name: ");
			string receiptName = Console.ReadLine();
			Console.Write("Description: ");
			string receiptDescription = Console.ReadLine();
			//AddIngredients();
			Console.Write("Instructions: ");
			string receiptInstructions = Console.ReadLine();
			//Show info
			receiptBook.CreateReceipt(new CreateReceiptDTO(receiptName, receiptDescription, receiptInstructions));
		}
		//Change later
		private Receipt ChooseRecipy()
		{
			while (true)
			{
				Console.WriteLine("Choose one recipy or type q to go back");
				string choice = Console.ReadLine();
				switch (choice)
				{
					case "q":
						Console.WriteLine("Going back");
						return null;
					default:
						return receiptBook.GetReceipt(choice);
						break;
				}
			}
		}
		private void EditRecipy()
		{
			Receipt receipt = ChooseRecipy();
			Console.WriteLine("Let's edit recipy.");
			Console.Write("New recipy name: ");
			string receiptName = Console.ReadLine();
			Console.Write("New recipy description: ");
			string receiptDescription = Console.ReadLine();
			//AddIngredients();
			Console.Write("New instructions: ");
			string receiptInstructions = Console.ReadLine();
			receiptBook.EditReceipt(new Receipt() { ReceiptId = receipt.ReceiptId, ReceiptName = receiptName, ReceiptDescription = receiptDescription, ReceiptInstructions = receiptInstructions });
		}

		private void DeleteRecipy()
		{
			Receipt receipt = ChooseRecipy();
			receiptBook.DeleteReceipt(receipt.ReceiptId);
		}
	}
}
