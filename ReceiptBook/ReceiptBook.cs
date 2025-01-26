using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class ReceiptBook
	{
		private List<Receipt> _receipts;

		public ReceiptBook()
		{
			_receipts = new List<Receipt>();
		}

		public Receipt GetReceipt(int id)
		{
			return _receipts.ElementAt(id);
		}

		public List<Receipt> GetReceiptList(int id)
		{
			return _receipts.ToList();
		}

		public void AddReceipt(Receipt receipt)
		{
			_receipts.Add(receipt);
		}

		public void EditReceipt(int id, Receipt newReceipt)
		{
			Receipt receipt = GetReceipt(id);

		}

		public void DeleteReceipt(int id) 
		{
			_receipts.RemoveAt(id);
		}

		
	}
}
