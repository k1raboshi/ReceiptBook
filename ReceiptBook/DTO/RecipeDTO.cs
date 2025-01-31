using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook.DTO
{
	public class CreateRecipeDTO
	{
		public string RecipeName { get; set; }
		public string RecipeDescription { get; set; }
		public string RecipeInstructions { get; set; }

		public CreateRecipeDTO(string recipeName, string recipeNDescription, string recipeNInstructions) 
		{
			RecipeName = recipeName;
			RecipeDescription = recipeNDescription;
			RecipeInstructions = recipeNInstructions;
		}
	}
}
