using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptBook;
using ReceiptBook.DTO;

namespace ReceiptBookUI
{
	public class RecipeBookView
	{
		private RecipeBookView() { }

		private RecipeBook recipeBook = new RecipeBook();
		private bool keepRunning = true; 
		private static RecipeBookView? s_instance;

		public static RecipeBookView Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new RecipeBookView();
				}

				return s_instance;
			}
		}

		public void Start()
		{

			while (keepRunning)
			{
				Console.WriteLine("Hello! Look at your recipes:");
				recipeBook.PrintAllRecipes();
				Console.WriteLine("What do you wanna do?");
				Console.WriteLine("1. Add new recipe");
				Console.WriteLine("2. Edit recipe");
				Console.WriteLine("3. Delete recipe");
				Console.WriteLine("4. Exit");
				ChoiceMenu();
			}
		}

		public void ChoiceMenu()
		{
			string choice = Console.ReadLine();
			Console.Clear();
			switch (choice)
			{
				case "1":
					AddNewRecipe();
					break;
				case "2":
					EditRecipe();
					break;
				case "3":
					DeleteRecipe();
					break;
				case "4":
					Console.WriteLine("Exiting");
					keepRunning = false;
					return;
				default:
					break;
			}
		}
		public void AddNewRecipe()
		{
			Console.WriteLine("Let's add new recipe.");
			Console.Write("Recipe name: ");
			string recipeName = Console.ReadLine();
			Console.Write("Description: ");
			string recipeDescription = Console.ReadLine();
			List <RecipeIngredient> ingredients = AddIngredients();
			Console.Write("Instructions: ");
			string recipeInstructions = Console.ReadLine();
			recipeBook.PrintRecipe(new Recipe() { RecipeName = recipeName, RecipeDescription = recipeDescription, RecipeInstructions = recipeInstructions });
			Console.WriteLine("Would you like to save changes? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				recipeBook.CreateRecipe(new CreateRecipeDTO(recipeName, recipeDescription, recipeInstructions));
				Console.WriteLine("Recipe was edited successfully.");
				Recipe recipe = recipeBook.GetRecipe(recipeName);
				foreach(var ingredient in ingredients)
				{
					ingredient.RecipeId = recipe.RecipeId;
					recipe.AddIngredient(ingredient);
				}
				return;
			}

			
		}
		//Change later
		private Recipe ChooseRecipe()
		{
			while (true)
			{
				Console.WriteLine("Choose one recipe by its name or type q to go back");
				string choice = Console.ReadLine();
				switch (choice)
				{
					case "q":
						Console.WriteLine("Going back");
						return null;
					default:
						return recipeBook.GetRecipe(choice);
				}
			}
		}
		private void EditRecipe()
		{
			Recipe recipe = ChooseRecipe();
			Console.WriteLine("Let's edit recipy your recipe.");
			recipeBook.PrintRecipe(recipe);
			var t = recipe.RecipeName;
			ChangeFieldValue(ref t, "name");
			recipe.RecipeName = t;

			t = recipe.RecipeDescription;
			ChangeFieldValue(ref t, "description");
			recipe.RecipeDescription = t;

			//AddIngredients();
			t = recipe.RecipeInstructions;
			ChangeFieldValue(ref t, "instructions");
			recipe.RecipeInstructions = t;

			Console.WriteLine("");
			recipeBook.PrintRecipe(recipe);
			Console.WriteLine("Would you like to save changes? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				recipeBook.EditRecipe(recipe);
				Console.WriteLine("Recipe was edited successfully.");
				return;
			}
			
			Console.WriteLine("All changes rolled back.");
		}

		private void DeleteRecipe()
		{
			Recipe recipe = ChooseRecipe();
			recipeBook.DeleteRecipe(recipe.RecipeId);
		}

		private void ChangeFieldValue(ref string field, string fieldName)
		{
			Console.WriteLine($"Would you choose a {fieldName}? Y/N");
			string answer = Console.ReadLine();

			if (answer.Equals("Y"))
			{
				Console.Write($"New recipe {fieldName}: ");
				field = Console.ReadLine();
			}
		}

		private List<RecipeIngredient> AddIngredients()
		{
			List<Ingredient> ingredients = new List<Ingredient>();
			List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();

			while (true)
			{
				Console.WriteLine("Here're your ingredients: ");

				if (recipeIngredients.Count == 0)
				{
					Console.WriteLine("No ingredients yet.");
				}
				else
				{
					for (int i = 0; i < recipeIngredients.Count; i++)
					{
						Console.WriteLine($"{ingredients.ElementAt(i).IngredientName}: {recipeIngredients.ElementAt(i).Amount} {recipeIngredients.ElementAt(i).Unit} ");
					}
				}
				
				Console.WriteLine("1. Add recipe");
				Console.WriteLine("2. Finish adding recipes");
				string choice = Console.ReadLine();
				switch(choice)
				{
					case "1":
						string ingredientName = Console.ReadLine();

						if (RecipeBook.GetIngredient(ingredientName) == null)
						{
							RecipeBook.AddIngredient(new Ingredient() { IngredientName = ingredientName });
						}

						int ingredientId = RecipeBook.GetIngredient(ingredientName).IngredientID;
						int amount = Convert.ToInt32(Console.ReadLine());
						string unit = Console.ReadLine();
						ingredients.Add(new Ingredient() { IngredientName = ingredientName });
						recipeIngredients.Add(new RecipeIngredient() { IngredientId = ingredientId, Amount = amount, Unit = unit });
						break;
					case "2":
						return recipeIngredients;
				}
				
			}
		}
	}
}
