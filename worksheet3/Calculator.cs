namespace comp101_ws3;

public enum Mode
{
    // use first recipe (default)
    First,

    // extension: use fastest possible alternative
    Fastest,

    // extension: use the smallest amount of raw ingredients
    Cheapest
}

public class Calculator
{
    /**
     * Check if it is possible to build an item with a given cookbook.
     * Items which don't exist, or use items which don't exist should return False.
     */
public static bool CanPossiblyBuild(Cookbook cb, string productName)
{
    // If the product is raw, we can always build it
    if (cb.IsRaw(productName))
    {
        return true;
    }

    // If the product has no known recipe, it cannot be built
    if (!cb.HasKnownRecipe(productName))
    {
        return false;
    }

    // Check all possible recipes for the product
    for (int i = 0; i < cb.KnownRecipeCount(productName); i++)
    {
        // Get the ingredients for the current recipe
        var ingredients = cb.GetIngredients(productName, i);

        bool canBuild = true;
        foreach (var ingredient in ingredients)
        {
            // If any ingredient cannot be built, this recipe is not valid
            if (!CanPossiblyBuild(cb, ingredient.Key))
            {
                canBuild = false;
                break; // No need to check further ingredients if one fails
            }
        }

        // If any recipe can be built, return true
        if (canBuild)
        {
            return true;
        }
    }

    // If no recipe can be built, return false
    return false;
}


    /**
     * Figure out the TOTAL time required to build a product.
     */
    public static int CalculateTimeRequired(Cookbook cb, string productName, Mode mode = Mode.First)
    {
        // If the product is raw, no time is required to build it
        if (cb.IsRaw(productName))
        {
            return 0;
        }

        // If the product is composite, handle according to the mode
        if (!cb.HasKnownRecipe(productName))
        {
            return -1; // No recipe found, return -1 (impossible to construct)
        }

        // Mode: First (default behavior)
        if (mode == Mode.First)
        {
            // Get the ingredients and time for the first valid recipe
            var ingredients = cb.GetFirstIngredients(productName);
            int totalTime = cb.GetFirstConstructionTime(productName);

            // Add the build time of the ingredients recursively
            foreach (var ingredient in ingredients)
            {
                totalTime += CalculateTimeRequired(cb, ingredient.Key, mode);
            }

            return totalTime;
        }

        // Mode: Fastest or Cheapest (extension tasks)
        // We need to evaluate all possible recipes for the product and calculate accordingly
        List<int> validTimes = new List<int>();
        List<int> validCosts = new List<int>();

        for (int i = 0; i < cb.KnownRecipeCount(productName); i++)
        {
            // Check if the recipe is possible
            if (CanPossiblyBuild(cb, productName))
            {
                var ingredients = cb.GetIngredients(productName, i);
                int totalTime = cb.GetConstructionTime(productName, i);
                int totalCost = 0;

                // Calculate the total build time and ingredient cost
                foreach (var ingredient in ingredients)
                {
                    totalCost += ingredient.Value; // Ingredient cost (assuming 1 unit of each ingredient)
                    totalTime += CalculateTimeRequired(cb, ingredient.Key, mode); // Recursively add the time for ingredients
                }

                // Store valid times and costs
                validTimes.Add(totalTime);
                validCosts.Add(totalCost);
            }
        }

        // Mode: Fastest
        if (mode == Mode.Fastest && validTimes.Count > 0)
        {
            return validTimes.Min(); // Return the minimum build time
        }

        // Mode: Cheapest
        if (mode == Mode.Cheapest && validCosts.Count > 0)
        {
            return validCosts.Min(); // Return the minimum cost in terms of raw ingredients
        }

        return -1; // If no valid recipe was found, return -1
    }

    /**
     * Get all the required raw ingredients for a given product, using a given cookbook. 
     */
    public static Dictionary<string, int> GetRawMaterialsForProduct(Cookbook cb, string product, Mode mode = Mode.First)
    {
        // If the product is raw, return an empty dictionary
        if (cb.IsRaw(product))
        {
            return new Dictionary<string, int>();
        }

        // If the product is composite, get the raw materials from the first valid recipe
        if (!cb.HasKnownRecipe(product))
        {
            return null; // No recipe found, return null (impossible to construct)
        }

        // Mode: First (default behavior)
        if (mode == Mode.First)
        {
            var ingredients = cb.GetFirstIngredients(product);
            var rawMaterials = new Dictionary<string, int>();

            // Recursively calculate raw materials for each ingredient
            foreach (var ingredient in ingredients)
            {
                // If the ingredient is raw, add it to the rawMaterials dictionary
                if (cb.IsRaw(ingredient.Key))
                {
                    if (rawMaterials.ContainsKey(ingredient.Key))
                    {
                        rawMaterials[ingredient.Key] += ingredient.Value;
                    }
                    else
                    {
                        rawMaterials.Add(ingredient.Key, ingredient.Value);
                    }
                }
                else
                {
                    // If the ingredient is not raw, get the raw materials recursively
                    var nestedRawMaterials = GetRawMaterialsForProduct(cb, ingredient.Key, mode);
                    if (nestedRawMaterials != null)
                    {
                        foreach (var nestedMaterial in nestedRawMaterials)
                        {
                            if (rawMaterials.ContainsKey(nestedMaterial.Key))
                            {
                                rawMaterials[nestedMaterial.Key] += nestedMaterial.Value * ingredient.Value;
                            }
                            else
                            {
                                rawMaterials.Add(nestedMaterial.Key, nestedMaterial.Value * ingredient.Value);
                            }
                        }
                    }
                    else
                    {
                        return null; // If any ingredient cannot be built, return null
                    }
                }
            }

            return rawMaterials;
        }

        // Mode: Fastest or Cheapest (extension tasks)
        // Similar approach as CalculateTimeRequired - consider all possible recipes
        Dictionary<string, int> bestRawMaterials = null;
        int bestTime = int.MaxValue;

        for (int i = 0; i < cb.KnownRecipeCount(product); i++)
        {
            if (CanPossiblyBuild(cb, product))
            {
                var ingredients = cb.GetIngredients(product, i);
                Dictionary<string, int> rawMaterials = new Dictionary<string, int>();

                // Calculate raw materials and time for each recipe
                foreach (var ingredient in ingredients)
                {
                    // Similar logic as Mode.First but optimized based on the mode (Fastest, Cheapest)
                    var nestedRawMaterials = GetRawMaterialsForProduct(cb, ingredient.Key, mode);
                    if (nestedRawMaterials != null)
                    {
                        foreach (var nestedMaterial in nestedRawMaterials)
                        {
                            if (rawMaterials.ContainsKey(nestedMaterial.Key))
                            {
                                rawMaterials[nestedMaterial.Key] += nestedMaterial.Value * ingredient.Value;
                            }
                            else
                            {
                                rawMaterials.Add(nestedMaterial.Key, nestedMaterial.Value * ingredient.Value);
                            }
                        }
                    }
                    else
                    {
                        return null; // If any ingredient cannot be built, return null
                    }
                }

                // Compare with the current best solution
                int time = CalculateTimeRequired(cb, product, mode);
                if (time < bestTime)
                {
                    bestRawMaterials = rawMaterials;
                    bestTime = time;
                }
            }
        }

        return bestRawMaterials;
    }

    //
    // Advanced Functions
    //

    /**
     * Check if a given product MAY contain an item.
     * 
     * If any recipe for the product contains the item, return true
     */
    public static bool MayContainItem(Cookbook cb, string productName, string item)
    {
        // Check if the item is in the ingredients of the product's recipe
        if (cb.HasKnownRecipe(productName))
        {
            var ingredients = cb.GetFirstIngredients(productName);
            if (ingredients.ContainsKey(item))
            {
                return true;
            }

            // Recursively check the ingredients
            foreach (var ingredient in ingredients)
            {
                if (MayContainItem(cb, ingredient.Key, item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /**
     * Check if it is possible to construct a product without an item.
     *
     * If the product can be constructed without using item, return true.
     * Completely solving this task efficiently can be complex, as it may involve backtracking.
     */
    public static bool CanConstructItemWithout(Cookbook cb, string productName, string item)
    {
        // Check if product can be built without the item by removing it from the ingredient list
        if (cb.HasKnownRecipe(productName))
        {
            var ingredients = cb.GetFirstIngredients(productName);
            if (!ingredients.ContainsKey(item))
            {
                return true;
            }

            // Try to construct the product without the item
            foreach (var ingredient in ingredients)
            {
                if (CanConstructItemWithout(cb, ingredient.Key, item))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
