using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ReceiptBook
{
	public class Recipe
	{
		public int RecipeId { get; set; }
		public string RecipeName { get; set; }
		public string RecipeDescription { get; set; }
		public string RecipeInstructions { get; set; }
		private List<RecipeIngredient> _ingredients = new List<RecipeIngredient>();

		public Recipe() { }
		public Recipe(int id, string recipeName, string recipeDescription, string recipeInstructions)
		{
			RecipeId = id;
			RecipeName = recipeName;
			RecipeDescription = recipeDescription;
			RecipeInstructions = recipeInstructions;
		}

		public async Task<Ingredient> GetIngredientAsync(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Ingredient>("SELECT * FROM Ingredient WHERE IngredientId = @id", new { id });
				return output.FirstOrDefault();
			}
		}

		public async Task<List<IngredientInfo>> GetIngredientListAsync()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<IngredientInfo>("SELECT I.IngredientName, RI.Amount, RI.Unit " +
					"FROM RecipeIngredient RI " +
					"JOIN Ingredient I ON I.IngredientId = RI.IngredientId " +
					"WHERE RecipeId = @RecipeId", new { RecipeId });
				return output.ToList();
			}
		}

		public async Task AddIngredientAsync(RecipeIngredient ingredient)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO RecipeIngredient " +
							"VALUES (@RecipeId, @IngredientId, @Amount, @Unit)";

				await cnn.ExecuteAsync(sqlQuery, ingredient);
			}
		}

		public async Task EditIngredient(int id, Ingredient newIngredient)
		{

			Ingredient ingredient = await GetIngredientAsync(id);
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Ingredient " +
								"SET IngredientName = @IngredientName " +
								"WHERE IngredientId = @IngredientId";
				await cnn.ExecuteAsync(sqlQuery, newIngredient);
			}
		}

		public void DeleteIngredient(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM RecipeIngredient WHERE IngredientId = @IngredientId AND RecipeId = @RecipeId";
				cnn.Execute(sqlQuery, new { IngredientId = id, RecipeId = RecipeId });
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
