using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class ReceiptBook
	{
		public ReceiptBook()
		{
			
		}

		public Receipt GetReceipt(int id)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Receipt>("SELECT * FROM Receipt WHERE ReceiptId = @id", new { id });
				return output.FirstOrDefault();
			}
		}

		public List<Receipt> GetReceiptList()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Receipt>("SELECT * FROM Receipt", new DynamicParameters());
				return output.ToList();
			}		
		}

		public void AddReceipt(Receipt receipt)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Receipt (ReceiptName, ReceiptDescription, ReceiptInstruction)" +
								"VALUES (@ReceiptName, @ReceiptDescription, @ReceiptInstruction";
				cnn.Execute(sqlQuery, receipt);
			}

		}

		public void EditReceipt(int id, Receipt newReceipt)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Receipt " +
								"SET ReceiptName = @ReceiptName, ReceiptDescription = @RececeiptDescription, ReceiptInstruction = @ReceiptInstruction";
				cnn.Execute(sqlQuery, newReceipt);
			}
				

		}

		public void DeleteReceipt(int id) 
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM Receipt WHERE ReceiptId = @id";
				cnn.Execute(sqlQuery, new {id});
			}
		}

		public void PrintAllReceipts()
		{
			foreach(Receipt receipt in GetReceiptList())
			{
				Console.WriteLine($"{receipt.ReceiptName}: {receipt.ReceiptDescription}; {receipt.ReceiptDescription}");
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
