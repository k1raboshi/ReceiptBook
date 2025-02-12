﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptBook
{
	public class Ingredient
	{
		public int IngredientID {  get; set; }
		public string IngredientName { get; set; }
		public List<Recipe> Receipes { get; } = new List<Recipe>();
		public Ingredient() { }
		public Ingredient(int ingredientID, string ingredientName)
		{
			IngredientID = ingredientID;
			IngredientName = ingredientName;
		}
	}
}