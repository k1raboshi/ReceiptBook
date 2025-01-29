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
	public class ReceiptBook
	{
		private int _pageLimit = 10;
		private int _currentPage = 1;
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

		public Receipt GetReceipt(string receiptName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<Receipt>("SELECT * FROM Receipt WHERE ReceiptName = @receiptName", new { receiptName });
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

		public void CreateReceipt(CreateReceiptDTO receipt)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "INSERT INTO Receipt (ReceiptName, ReceiptDescription, ReceiptInstructions)" +
								"VALUES (@ReceiptName, @ReceiptDescription, @ReceiptInstructions)";
				
				cnn.Execute(sqlQuery, receipt);
			}

		}

		public void EditReceipt(Receipt newReceipt)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "UPDATE Receipt " +
								"SET ReceiptName = @ReceiptName, ReceiptDescription = @ReceiptDescription, ReceiptInstructions = @ReceiptInstructions " +
								"WHERE ReceiptId = @ReceiptId";
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

		public void DeleteReceipt(string receiptName)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var sqlQuery = "DELETE FROM Receipt WHERE ReceiptName = @id";
				cnn.Execute(sqlQuery, new { receiptName });
			}
		}

		public void PrintAllReceipts()
		{
			List<Receipt> receipts = GetReceiptList();

			if (receipts.Count == 0)
			{
				Console.WriteLine("No recepies yet.");
				return;
			}

			foreach(Receipt receipt in receipts)
			{
				Console.WriteLine($"{receipt.ReceiptName}: {receipt.ReceiptDescription}; {receipt.ReceiptInstructions}");
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
