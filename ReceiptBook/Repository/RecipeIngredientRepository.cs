using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ReceiptBook.Repository
{
	public class RecipeIngredientRepository
	{
		public async Task<IngredientInfo> GetIngredientAsync(int id, int recipeId)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<IngredientInfo>("SELECT I.IngredientName, RI.Amount, RI.Unit " +
					"FROM RecipeIngredient RI " +
					"JOIN Ingredient I ON I.IngredientId = RI.IngredientId " +
					"WHERE RecipeId = @RecipeId and IngredientId = @id", new { id, recipeId });
				return output.FirstOrDefault();
			}
		}

		public async Task<List<IngredientInfo>> GetIngredientListAsync(int recipeId)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<IngredientInfo>("SELECT I.IngredientName, RI.Amount, RI.Unit " +
					"FROM RecipeIngredient RI " +
					"JOIN Ingredient I ON I.IngredientId = RI.IngredientId " +
					"WHERE RecipeId = @RecipeId", new { recipeId });
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

		public async Task EditIngredient(IngredientInfo newIngredient)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE RecipeIngredient " +
								"SET Amount = @Amount, Unit = @Unit " +
								"WHERE RecipeId = @RecipeId AND IngredientId = @IngredientId";
				await cnn.ExecuteAsync(sqlQuery, newIngredient);
			}
		}

		public void DeleteIngredient(int id, int recipeId)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM RecipeIngredient WHERE IngredientId = @IngredientId AND RecipeId = @RecipeId";
				cnn.Execute(sqlQuery, new { IngredientId = id, RecipeId = recipeId });
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
