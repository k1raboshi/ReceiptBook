using ReceiptBook.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook.Mappers
{
	public static class RecipeMappers
	{
		public static PrintRecipeDTO ToPrintRecipeDTO(this Recipe recipe)
		{
			return new PrintRecipeDTO
			{
				RecipeName = recipe.RecipeName
			};
		}
	}
}
