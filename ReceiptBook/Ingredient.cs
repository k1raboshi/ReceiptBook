using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class Ingredient
	{
		public int IngredientID {  get; set; }
		public string IngredientName { get; set; }
		public List<Receipt> Receipts { get; } = new List<Receipt>();
		public Ingredient(int ingredientID, string ingredientName)
		{
			IngredientID = ingredientID;
			IngredientName = ingredientName;
		}
	}
}
