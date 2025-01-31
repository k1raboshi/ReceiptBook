using Dapper;
using Microsoft.Extensions.Configuration;
using ReceiptBook.DTO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook.Repository
{
	public class RecipeRepository
	{
		private static string LoadConnectionString()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

			IConfiguration config = builder.Build();
			string conString = config.GetConnectionString("Default");
			return conString;
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

				await cnn.ExecuteAsync(sqlQuery, recipe);
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
				cnn.Execute(sqlQuery, new { id });
			}
		}
	}
}
