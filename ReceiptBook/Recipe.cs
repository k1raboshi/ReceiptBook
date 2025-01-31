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

		public Ingredient GetIngredient(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Ingredient>("SELECT * FROM Ingredient WHERE IngredientId = @id", new { id });
				return output.FirstOrDefault();
			}
		}

		public List<IngredientInfo> GetIngredientList()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<IngredientInfo>("SELECT Ingredient.IngredientName, RI.Amount, RI.Unit " +
					"FROM RecipeIngredient as RI " +
					"JOIN Ingredient ON Ingredient.IngredientId = RI.IngredientId " +
					"WHERE RecipeId = @RecipeId", new { RecipeId });
				return output.ToList();
			}
		}

		public void AddIngredient(RecipeIngredient ingredient)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO RecipeIngredient " +
							"VALUES (@RecipeId, @IngredientId, @Amount, @Unit)";

				cnn.Execute(sqlQuery, ingredient);
			}
		}

		public void EditIngredient(int id, Ingredient newIngredient)
		{

			Ingredient ingredient = GetIngredient(id);
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Ingredient " +
								"SET IngredientName = @IngredientName " +
								"WHERE IngredientId = @IngredientId";
				cnn.Execute(sqlQuery, newIngredient);
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
