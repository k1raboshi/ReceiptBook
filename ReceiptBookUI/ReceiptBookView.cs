using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBookUI
{
	public class ReceiptBookView
	{
		private ReceiptBookView() { }

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
						break;
					case "2":
						break;
					case "3":
						break;
					case "4":
						break;
					case "5":
						Console.WriteLine("Exiting");
						return;
					default:
						break;
				}
			}
		}
	}
}
