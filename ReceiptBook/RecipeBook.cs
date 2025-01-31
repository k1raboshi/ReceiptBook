using Dapper;
using Microsoft.Extensions.Configuration;
using ReceiptBook.DTO;
using ReceiptBook.Mappers;
using ReceiptBook.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class RecipeBook
	{
		private int _pageLimit = 10;
		private int _currentPage = 1;
		private readonly RecipeRepository recipeRepository = new RecipeRepository();
		private static readonly IngredientRepository ingredientRepository = new IngredientRepository();

		public async Task<Recipe> GetRecipeAsync(int id)
		{
			return await recipeRepository.GetRecipeAsync(id);
		}

		public async Task<Recipe> GetRecipeAsync(string recipeName)
		{
			return await recipeRepository.GetRecipeAsync(recipeName);
		}

		public async Task<List<Recipe>> GetRecipeListAsync()
		{
			return await recipeRepository.GetRecipeListAsync();
		}

		public async Task CreateRecipeAsync(CreateRecipeDTO recipe)
		{
			await recipeRepository.CreateRecipeAsync(recipe);
		}

		public async Task EditRecipeAsync(Recipe newRecipe)
		{
			await recipeRepository.EditRecipeAsync(newRecipe);
		}

		public void DeleteRecipe(int id) 
		{
			recipeRepository.DeleteRecipe(id);
		}
		public void PrintRecipe(PrintRecipeDTO printRecipeDTO)
		{
			Console.WriteLine($"Recipe: {printRecipeDTO.RecipeName}");
		}
		public async Task PrintRecipe(Recipe recipe)
		{
			Console.WriteLine($"Recipe: {recipe.RecipeName}\n Description: {recipe.RecipeDescription}; {recipe.RecipeInstructions}");
			var ingredients = await recipe.GetIngredientListAsync();
			foreach (var ingredient in ingredients)
			{
				Console.WriteLine($"{ingredient.IngredientName}: {ingredient.Amount} {ingredient.Unit}");
			}
		}
		public async Task PrintAllRecipes()
		{
			List<PrintRecipeDTO> recipes = (await GetRecipeListAsync()).Select(r => r.ToPrintRecipeDTO()).ToList();

			if (recipes.Count == 0)
			{
				Console.WriteLine("No recepes yet.");
				return;
			}

			foreach(PrintRecipeDTO recipe in recipes)
			{
				PrintRecipe(recipe);
			}
		}

		public static async Task AddIngredientAsync(Ingredient ingredient)
		{
			await ingredientRepository.AddIngredientAsync(ingredient);
		}

		public static async Task<Ingredient> GetIngredientAsync(string ingredientName)
		{
			return await ingredientRepository.GetIngredientAsync(ingredientName);
		}
	}
}
