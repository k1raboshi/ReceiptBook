using Dapper;
using Microsoft.Extensions.Configuration;
using ReceiptBook.DTO;
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

		public Recipe GetRecipe(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Recipe>("SELECT * FROM Recipe WHERE RecipeId = @id", new { id });
				return output.FirstOrDefault();
			}
		}

		public Recipe GetRecipe(string recipeName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Recipe>("SELECT * FROM Recipe WHERE RecipeName = @recipeName", new { recipeName });
				return output.FirstOrDefault();
			}
		}

		public List<Recipe> GetRecipeList()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Recipe>("SELECT * FROM Recipe", new DynamicParameters());
				return output.ToList();
			}		
		}

		public void CreateRecipe(CreateRecipeDTO recipe)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Recipe (RecipeName, RecipeDescription, RecipeInstructions)" +
								"VALUES (@RecipeName, @RecipeDescription, @RecipeInstructions)";
				
				cnn.Execute(sqlQuery, recipe);
			}
		}

		public void EditRecipe(Recipe newRecipe)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Recipe " +
								"SET RecipeName = @RecipeName, RecipeDescription = @RecipeDescription, RecipeInstructions = @RecipeInstructions " +
								"WHERE RecipeId = @RecipeId";
				cnn.Execute(sqlQuery, newRecipe);
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
		public void PrintRecipe(Recipe recipe)
		{
			Console.WriteLine($"Recipe: {recipe.RecipeName}\n Description: {recipe.RecipeDescription}; {recipe.RecipeInstructions}");

			foreach(var ingredient in recipe.GetIngredientList())
			{
				Console.WriteLine($"{ingredient.IngredientName}: {ingredient.Amount} {ingredient.Unit}");
			}
		}
		public void PrintAllRecipes()
		{
			List<Recipe> recipes = GetRecipeList();

			if (recipes.Count == 0)
			{
				Console.WriteLine("No recepes yet.");
				return;
			}

			foreach(Recipe recipe in recipes)
			{
				PrintRecipe(recipe);
			}
		}

		public static void AddIngredient(Ingredient ingredient)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Ingredient (IngredientName)" +
								"VALUES (@IngredientName)";

				cnn.Execute(sqlQuery, ingredient);
			}
		}

		public static Ingredient GetIngredient(string ingredientName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Ingredient>("SELECT * FROM Ingredient WHERE IngredientName = @ingredientName", new { ingredientName });
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
