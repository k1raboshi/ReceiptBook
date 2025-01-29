using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class Receipt
	{
		public int ReceiptId { get; set; }
		public string ReceiptName { get; set; }
		public string ReceiptDescription { get; set; }
		public string ReceiptInstructions { get; set; }
		private List<Ingredient> _ingredients = new List<Ingredient>();

		public Receipt() { }
		public Receipt(int id, string receiptName, string receiptDescription, string receipInstructions)
		{
			ReceiptId = id;
			ReceiptName = receiptName;
			ReceiptDescription = receiptDescription;
			ReceiptInstructions = receipInstructions;
		}

		public Ingredient GetIngredient(int id)
		{
			return _ingredients.ElementAt(id);
		}

		public List<Ingredient> GetIngredientList(int id)
		{
			return _ingredients.ToList();
		}

		public void AddIngredient(Ingredient ingredient)
		{
			ingredient.Receipts.Add(this);
			_ingredients.Add(ingredient);
		}

		public void EditIngredient(int id, Ingredient newIngredient)
		{
			Ingredient ingredient = GetIngredient(id);
		}

		public void DeleteIngredient(int id)
		{
			_ingredients.RemoveAt(id);
		}
	}
}
