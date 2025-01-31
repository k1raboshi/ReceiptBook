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
	public class IngredientRepository
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

		public async Task AddIngredientAsync(Ingredient ingredient)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Ingredient (IngredientName)" +
								"VALUES (@IngredientName)";

				await cnn.ExecuteAsync(sqlQuery, ingredient);
			}
		}

		public async Task<Ingredient> GetIngredientAsync(string ingredientName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = await cnn.QueryAsync<Ingredient>("SELECT * FROM Ingredient WHERE IngredientName = @ingredientName", new { ingredientName });
				return output.FirstOrDefault();
			}
		}
	}
}
