using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace RecipeConsole
{
    // Represents an ingredient with a name, quantity, calories, and food group.
    public class Ingredient
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }

        // Constructor to initialize a new ingredient with given properties.
        public Ingredient(string name, int quantity, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }

    // Represents a recipe with a name, list of ingredients, and list of steps.
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }
    }

    
    public class InputHandler // Handles user input for creating recipes and displaying options.
    {
        
        public static int GetOptionFromUser() // Prompts the user to choose an option and returns the chosen option.
        {
            Console.WriteLine("\nOption Chosen:");
            return Convert.ToInt32(Console.ReadLine());
        }
        public static Recipe CreateRecipe()
        {
            Console.WriteLine("\nEnter Recipe Name:");
            string name = Console.ReadLine();
            Recipe recipe = new Recipe(name);

            bool addIngredients = true;
            do
            {
                Console.WriteLine("\nEnter Ingredient Name:");
                string ingredientName = Console.ReadLine();

                Console.WriteLine("Enter Quantity:");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for quantity:");
                }

                Console.WriteLine("Enter Calories:");
                int calories;
                while (!int.TryParse(Console.ReadLine(), out calories))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for calories:");
                }

                Console.WriteLine("Enter Food Group:");
                string foodGroup = Console.ReadLine();

                recipe.Ingredients.Add(new Ingredient(ingredientName, quantity, calories, foodGroup));

                Console.WriteLine("Add another ingredient? (yes/no)");
                string addAnother = Console.ReadLine().ToLower();
                if (addAnother != "yes")
                {
                    addIngredients = false;
                }
            } while (addIngredients);

            return recipe;
        }

        public static void DisplayOptions()// Displays the available options to the user.
        {
            // Options
            Console.WriteLine("Options:");
            Console.WriteLine("1) Add Recipe");
            Console.WriteLine("2) Display Recipes");
            Console.WriteLine("3) Exit");
        }
    }

    // Manages a collection of recipes and provides methods
    public class RecipeManager
    {
        // List to store recipes
        private List<Recipe> recipes = new List<Recipe>();

        // Event to notify users when a recipe exceeds 300 calories
        public delegate void RecipeCalorieNotificationHandler(string recipeName, int totalCalories);
        public event RecipeCalorieNotificationHandler RecipeCalorieExceeded;

        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
            CheckRecipeCalories(recipe); // Check if added recipe exceeds 300 calories
        }

        public void DisplayRecipes()
        {
            Console.WriteLine("Recipes:");
            foreach (var recipe in recipes)
            {
                Console.WriteLine(recipe.Name);
            }
        }

        public void DisplayRecipeDetails(Recipe recipe)// Displays details of a specific recipe
        {
            Console.WriteLine($"Recipe: {recipe.Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.WriteLine($"{ingredient.Name}: {ingredient.Quantity} {ingredient.FoodGroup}, Calories: {ingredient.Calories}");
            }

            int totalCalories = CalculateTotalCalories(recipe);
            Console.WriteLine($"Total Calories: {totalCalories}");
            if (totalCalories > 300)
            {
                Console.WriteLine("Warning: Total calories exceed 300!");
            }
        }

        private int CalculateTotalCalories(Recipe recipe)
        {
            int totalCalories = 0;
            foreach (var ingredient in recipe.Ingredients)
            {
                totalCalories += ingredient.Calories;
            }
            return totalCalories;
        }

        // Checks if a recipe exceeds 300 calories and raises the event if it does
        private void CheckRecipeCalories(Recipe recipe)
        {
            int totalCalories = CalculateTotalCalories(recipe);
            if (totalCalories > 300)
            {
                RecipeCalorieExceeded?.Invoke(recipe.Name, totalCalories);
            }
        }

        public List<Recipe> Recipes => recipes;
    }

    // Entry point of program
    public class Program
    {
        // Main method
        static void Main(string[] args)
        {
            RecipeManager recipeManager = new RecipeManager();
            int option;

            do
            {
                InputHandler.DisplayOptions();
                option = InputHandler.GetOptionFromUser();

                switch (option)
                {
                    case 1:
                        Recipe recipe = InputHandler.CreateRecipe();
                        recipeManager.AddRecipe(recipe);
                        break;
                    case 2:
                        recipeManager.DisplayRecipes();
                        Console.WriteLine("\nEnter the name of the recipe to view details:");
                        string recipeName = Console.ReadLine();
                        Recipe selectedRecipe = recipeManager.Recipes.Find(r => r.Name == recipeName);
                        if (selectedRecipe != null)
                        {
                            recipeManager.DisplayRecipeDetails(selectedRecipe);
                        }
                        else
                        {
                            Console.WriteLine("Recipe not found.");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            } while (option != 3);
        }
    }
}
