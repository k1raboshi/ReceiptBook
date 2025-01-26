using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class Receipt
	{
		public int ReceiptId { get; set; }
		public string ReceiptName { get; set; }
		public string ReceiptDescription { get; set; }
		public string ReceiptInstruction { get; set; }

		public Receipt(int id, string receiptName, string receiptDescription, string receipInstruction)
		{
			ReceiptId = id;
			ReceiptName = receiptName;
			ReceiptDescription = receiptDescription;
			ReceiptInstruction = receipInstruction;
		}
	}
}
