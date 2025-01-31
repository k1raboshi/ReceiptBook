using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ReceiptBook.Repository;

namespace ReceiptBook
{
	public class Recipe
	{
		public int RecipeId { get; set; }
		public string RecipeName { get; set; }
		public string RecipeDescription { get; set; }
		public string RecipeInstructions { get; set; }
		private readonly RecipeIngredientRepository recipeIngredientRepository = new RecipeIngredientRepository();

		public Recipe() { }
		public Recipe(int id, string recipeName, string recipeDescription, string recipeInstructions)
		{
			RecipeId = id;
			RecipeName = recipeName;
			RecipeDescription = recipeDescription;
			RecipeInstructions = recipeInstructions;
		}

		public async Task<IngredientInfo> GetIngredientAsync(int id)
		{
			return await recipeIngredientRepository.GetIngredientAsync(id, RecipeId);
		}

		public async Task<List<IngredientInfo>> GetIngredientListAsync()
		{
			return await recipeIngredientRepository.GetIngredientListAsync(RecipeId);
		}

		public async Task AddIngredientAsync(RecipeIngredient ingredient)
		{
			await recipeIngredientRepository.AddIngredientAsync(ingredient);
		}

		public async Task EditIngredient(IngredientInfo newIngredient)
		{

			await recipeIngredientRepository.EditIngredient(newIngredient);
		}

		public void DeleteIngredient(int id)
		{
			recipeIngredientRepository.DeleteIngredient(id, RecipeId);
		}
	}
}
