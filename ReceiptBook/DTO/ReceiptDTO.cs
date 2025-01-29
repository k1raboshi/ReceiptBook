using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook.DTO
{
	public class CreateReceiptDTO
	{
		public string ReceiptName { get; set; }
		public string ReceiptDescription { get; set; }
		public string ReceiptInstructions { get; set; }

		public CreateReceiptDTO(string receiptName, string receiptDescription, string receiptInstructions) 
		{
			ReceiptName = receiptName;
			ReceiptDescription = receiptDescription;
			ReceiptInstructions = receiptInstructions;
		}
	}
}
