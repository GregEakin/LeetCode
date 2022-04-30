//    Copyright 2022 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HeapChecks;

public class FindAllPossibleRecipesFromGivenSupplies
{
    public class Solution22
    {
        private readonly Dictionary<string, HashSet<string>> _recipes = new();
        private readonly HashSet<string> _suppliesGiven = new();
        private readonly Dictionary<string, bool> _recipesCreated = new();

        public bool CheckRecipes(string recipe)
        {
            _recipesCreated[recipe] = false;
            foreach (var ingredient in _recipes[recipe])
            {
                if (_suppliesGiven.Contains(ingredient)) continue;
                if (!_recipes.ContainsKey(ingredient)) return false;
                if (_recipesCreated.ContainsKey(ingredient)) continue;
                if (!CheckRecipes(ingredient)) return false;
            }

            _recipesCreated[recipe] = true;
            return true;
        }

        public IList<string> FindAllRecipes(string[] recipes, IList<IList<string>> ingredients, string[] supplies)
        {
            for (var i = 0; i < ingredients.Count; i++)
                _recipes.Add(recipes[i], new HashSet<string>(ingredients[i]));

            foreach (var supply in supplies)
                _suppliesGiven.Add(supply);

            return recipes
                .Where(CheckRecipes)
                .ToArray();
        }
    }

    public class SolutionXX
    {
        public IList<string> FindAllRecipes(string[] recipes, IList<IList<string>> ingredients, string[] supplies)
        {
            var ingredientLookup = new Dictionary<string, HashSet<string>>();
            for (var i = 0; i < recipes.Length; i++)
            {
                var recipe = recipes[i];
                foreach (var ingredient in ingredients[i])
                {
                    if (ingredientLookup.TryGetValue(ingredient, out var set))
                        set.Add(recipe);
                    else
                        ingredientLookup.Add(ingredient, new HashSet<string> { recipe });
                }
            }

            var deleteIngredients = new Queue<string>(ingredientLookup.Keys
                .Where(key => !supplies.Contains(key))
                .Where(key => !recipes.Contains(key)));

            while (deleteIngredients.Count > 0)
            {
                var ingredient = deleteIngredients.Dequeue();
                if (!ingredientLookup.TryGetValue(ingredient, out var recipesSet)) continue;
                foreach (var recipe in recipesSet)
                    deleteIngredients.Enqueue(recipe);

                ingredientLookup.Remove(ingredient);
            }

            return recipes.Where((_, i) => ingredients[i].All(ingredient => ingredientLookup.ContainsKey(ingredient)))
                .ToList();
        }
    }

    public class Solution12
    {
        private readonly Dictionary<string, IList<string>> _recipes = new();
        private readonly HashSet<string> _suppliesGiven = new();
        private readonly HashSet<string> _recipesCreated = new();
        private readonly HashSet<string> _recipesSeen = new();

        public bool CheckRecipes(string recipe)
        {
            if (_recipesSeen.Contains(recipe))
                return false;
            _recipesSeen.Add(recipe);

            var haveEverything = true;
            foreach (var ingredient in _recipes[recipe])
            {
                if (_suppliesGiven.Contains(ingredient)) continue;
                if (_recipesCreated.Contains(ingredient)) continue;
                haveEverything = CheckRecipes(ingredient);
                break;
            }

            if (!haveEverything) return false;
            _recipesCreated.Add(recipe);

            return true;
        }

        public IList<string> FindAllRecipes(string[] recipes, IList<IList<string>> ingredients, string[] supplies)
        {
            for (var i = 0; i < ingredients.Count; i++)
                _recipes.Add(recipes[i], ingredients[i]);

            foreach (var supply in supplies)
                _suppliesGiven.Add(supply);

            return recipes.Where(CheckRecipes).ToArray();
        }
    }

    public class Solution
    {
        public IList<string> FindAllRecipes(string[] recipes, IList<IList<string>> ingredients, string[] supplies)
        {
            var answers = new HashSet<string>();
            var graph = new Dictionary<string, IList<string>>();
            var suppliesSet = new HashSet<string>(supplies);
            var inNodes = new Dictionary<string, int>();

            for (var i = 0; i < recipes.Length; i++)
            {
                var recipe = recipes[i];
                var count = 0;
                foreach (var ingredient in ingredients[i])
                {
                    if (suppliesSet.Contains(ingredient)) continue;
                    if (answers.Contains(ingredient)) continue;
                    count++;
                    if (!graph.TryGetValue(ingredient, out var list))
                    {
                        list = new List<string>();
                        graph.Add(ingredient, list);
                    }

                    list.Add(recipe);
                    if (!inNodes.TryAdd(recipe, 1))
                        inNodes[recipe]++;
                }

                if (count != 0) continue;
                answers.Add(recipe);
                if (!graph.ContainsKey(recipe)) continue;
                var q = new Queue<string>();
                q.Enqueue(recipe);
                while (q.Count > 0)
                {
                    var recipeQ = q.Dequeue();
                    if (!graph.TryGetValue(recipeQ, out var impacting)) continue;
                    foreach (var recipeI in impacting)
                    {
                        inNodes[recipeI]--;
                        if (inNodes[recipeI] != 0) continue;
                        answers.Add(recipeI);
                        q.Enqueue(recipeI);
                    }
                }
            }

            return answers.ToList();
        }
    }

    [Fact]
    public void Answer1()
    {
        var recipes = new[] { "ju", "fzjnm", "x", "e", "zpmcz", "h", "q" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "d" },
            new List<string> { "hveml", "f", "cpivl" },
            new List<string> { "cpivl", "zpmcz", "h", "e", "fzjnm", "ju" },
            new List<string> { "cpivl", "hveml", "zpmcz", "ju", "h" },
            new List<string> { "h", "fzjnm", "e", "q", "x" },
            new List<string> { "d", "hveml", "cpivl", "q", "zpmcz", "ju", "e", "x" },
            new List<string> { "f", "hveml", "cpivl" }
        };
        var supplies = new[] { "f", "hveml", "cpivl", "d" };
        var solution = new Solution();
        Assert.Equal(new[] { "ju", "fzjnm", "q" }, solution.FindAllRecipes(recipes, ingredients, supplies));
    }

    [Fact]
    public void Example1()
    {
        var recipes = new[] { "bread" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "yeast", "flour" }
        };
        var supplies = new[] { "yeast", "flour", "corn" };
        var solution = new Solution();
        Assert.Equal(new[] { "bread" }, solution.FindAllRecipes(recipes, ingredients, supplies));
    }

    [Fact]
    public void Example2()
    {
        var recipes = new[] { "bread", "sandwich" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "yeast", "flour" },
            new List<string> { "bread", "meat" }
        };
        var supplies = new[] { "yeast", "flour", "meat" };
        var solution = new Solution();
        Assert.Equal(new[] { "bread", "sandwich" }, solution.FindAllRecipes(recipes, ingredients, supplies));
    }

    [Fact]
    public void Example3()
    {
        var recipes = new[] { "bread", "sandwich", "burger" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "yeast", "flour" },
            new List<string> { "bread", "meat" },
            new List<string> { "sandwich", "meat", "bread" }
        };
        var supplies = new[] { "yeast", "flour", "meat" };
        var solution = new Solution();
        Assert.Equal(new[] { "bread", "sandwich", "burger" }, solution.FindAllRecipes(recipes, ingredients, supplies));
    }

    [Fact]
    public void Test1()
    {
        var recipes = new[] { "ra", "rb" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "ia", "rb" },
            new List<string> { "ib", "ra" }
        };
        var supplies = new[] { "ia", "ib" };
        var solution = new Solution();
        // Assert.Equal(new[] { "ra", "rb" }, solution.FindAllRecipes(recipes, ingredients, supplies));
        Assert.Equal(Array.Empty<string>(), solution.FindAllRecipes(recipes, ingredients, supplies));
    }

    [Fact]
    public void Test2()
    {
        var recipes = new[] { "rc", "ra", "rb" };
        var ingredients = new List<IList<string>>
        {
            new List<string> { "rc", "ra", "rb", "ic" },
            new List<string> { "rb", "ia", "rc" },
            new List<string> { "ra", "ib", "rc" },
        };
        var supplies = new[] { "ia", "ib" };
        var solution = new Solution();
        Assert.Equal(Array.Empty<string>(), solution.FindAllRecipes(recipes, ingredients, supplies));
    }
}