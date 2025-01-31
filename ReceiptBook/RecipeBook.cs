using Dapper;
using Microsoft.Extensions.Configuration;
using ReceiptBook.DTO;
using ReceiptBook.Mappers;
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
		public RecipeBook()
		{
			
		}

		public async Task<Recipe> GetRecipeAsync(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Recipe>("SELECT * FROM Recipe WHERE RecipeId = @id", new { id });
				return output.FirstOrDefault();
			}
		}

		public async Task<Recipe> GetRecipeAsync(string recipeName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Recipe>("SELECT * FROM Recipe WHERE RecipeName = @recipeName", new { recipeName });
				return output.FirstOrDefault();
			}
		}

		public async Task<List<Recipe>> GetRecipeListAsync()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Recipe>("SELECT * FROM Recipe", new DynamicParameters());
				return output.ToList();
			}		
		}

		public async Task CreateRecipeAsync(CreateRecipeDTO recipe)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Recipe (RecipeName, RecipeDescription, RecipeInstructions)" +
								"VALUES (@RecipeName, @RecipeDescription, @RecipeInstructions)";
				
				cnn.ExecuteAsync(sqlQuery, recipe);
			}
		}

		public async Task EditRecipeAsync(Recipe newRecipe)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Recipe " +
								"SET RecipeName = @RecipeName, RecipeDescription = @RecipeDescription, RecipeInstructions = @RecipeInstructions " +
								"WHERE RecipeId = @RecipeId";
				await cnn.ExecuteAsync(sqlQuery, newRecipe);
			}
		}

		public void DeleteRecipe(int id) 
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM Recipe WHERE RecipeId = @id";
				cnn.Execute(sqlQuery, new {id});
			}
		}

		public void DeleteRecipe(string recipeName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM Recipe WHERE RecipeName = @id";
				cnn.Execute(sqlQuery, new { recipeName });
			}
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
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Ingredient (IngredientName)" +
								"VALUES (@IngredientName)";

				await cnn.ExecuteAsync(sqlQuery, ingredient);
			}
		}

		public static async Task<Ingredient> GetIngredientAsync(string ingredientName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Ingredient>("SELECT * FROM Ingredient WHERE IngredientName = @ingredientName", new { ingredientName });
				return output.FirstOrDefault();
			}
		}

		private static string LoadConnectionString()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			IConfiguration config = builder.Build();
			string conString = config.GetConnectionString("Default");
			return conString;
		}
	}
}
