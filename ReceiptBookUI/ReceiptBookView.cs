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
		private bool keepRunning = true; 
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

			while (keepRunning)
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
			Console.Clear();
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
					keepRunning = false;
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
			receiptBook.PrintReceipt(new Receipt() { ReceiptName = receiptName, ReceiptDescription = receiptDescription, ReceiptInstructions = receiptInstructions});
			Console.WriteLine("Would you like to save changes? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				receiptBook.CreateReceipt(new CreateReceiptDTO(receiptName, receiptDescription, receiptInstructions));
				Console.WriteLine("Receipt was edited successfully.");
				return;
			}

			
		}
		//Change later
		private Receipt ChooseRecipy()
		{
			while (true)
			{
				Console.WriteLine("Choose one recipy by its name or type q to go back");
				string choice = Console.ReadLine();
				switch (choice)
				{
					case "q":
						Console.WriteLine("Going back");
						return null;
					default:
						return receiptBook.GetReceipt(choice);
				}
			}
		}
		private void EditRecipy()
		{
			Receipt receipt = ChooseRecipy();
			Console.WriteLine("Let's edit recipy your receipt.");
			receiptBook.PrintReceipt(receipt);
			var t = receipt.ReceiptName;
			ChangeFieldValue(ref t, "name");
			receipt.ReceiptName = t;

			t = receipt.ReceiptDescription;
			ChangeFieldValue(ref t, "description");
			receipt.ReceiptDescription = t;

			//AddIngredients();
			t = receipt.ReceiptInstructions;
			ChangeFieldValue(ref t, "instructions");
			receipt.ReceiptInstructions = t;

			Console.WriteLine("");
			receiptBook.PrintReceipt(receipt);
			Console.WriteLine("Would you like to save changes? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				receiptBook.EditReceipt(receipt);
				Console.WriteLine("Receipt was edited successfully.");
				return;
			}
			
			Console.WriteLine("All changes rolled back.");
		}

		private void DeleteRecipy()
		{
			Receipt receipt = ChooseRecipy();
			receiptBook.DeleteReceipt(receipt.ReceiptId);
		}

		private void ChangeFieldValue(ref string field, string fieldName)
		{
			Console.WriteLine($"Would you choose a {fieldName}? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				Console.Write("New recipy name: ");
				field = Console.ReadLine();
			}
		}
	}
}
