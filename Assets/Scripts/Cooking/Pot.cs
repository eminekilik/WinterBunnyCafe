using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour, Trashable
{
    public List<IngredientType> ingredients = new List<IngredientType>();

    public bool isCooking;
    public float cookProgress; // 0–1 arasý
    public CookResult cookResult;

    // ?? ÜRETÝLEN ÜRÜN
    public ProductType producedProduct;

    public bool hasStartedCooking;

    public bool HasIngredients()
    {
        return ingredients.Count > 0;
    }

    public void ResetPot()
    {
        ingredients.Clear();
        cookProgress = 0f;
        cookResult = CookResult.None;
        producedProduct = ProductType.None;
        isCooking = false;
        hasStartedCooking = false;
    }

    // ?? ASIL ÜRÜN KARARI BURADA
    public void DetermineProduct()
    {
        bool hasMilk = ingredients.Contains(IngredientType.Milk);
        bool hasChocolate = ingredients.Contains(IngredientType.Chocolate);

        // ?? Yanmýþ
        if (cookResult == CookResult.Overcooked)
        {
            producedProduct = ProductType.Burnt;
            return;
        }

        // ?? Az piþmiþ
        if (cookResult == CookResult.Undercooked)
        {
            if (hasMilk && hasChocolate)
                producedProduct = ProductType.RawMilkAndChocolate;
            else if (hasMilk)
                producedProduct = ProductType.RawMilk;
            else if (hasChocolate)
                producedProduct = ProductType.RawChocolate;

            return;
        }

        // ?? Tam kývam
        if (cookResult == CookResult.Perfect)
        {
            if (hasMilk && hasChocolate)
                producedProduct = ProductType.HotChocolate;
            else if (hasMilk)
                producedProduct = ProductType.HotMilk;
            else if (hasChocolate)
                producedProduct = ProductType.MeltedChocolate;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ?? ÖNCE KUPA KONTROLÜ
        Cup cup = other.GetComponent<Cup>();
        if (cup != null)
        {
            DraggableItem draggable = GetComponent<DraggableItem>();

            if (draggable != null && draggable.IsDragging)
            {
                draggable.OnAddedToContainer();
                TryPourIntoCup(cup);
            }

            return;
        }

        // ?? SONRA INGREDIENT
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient == null) return;

        // ? piþirme baþladýysa eklenemez
        if (hasStartedCooking)
        {
            Debug.Log("Piþirme baþladý, malzeme eklenemez");
            return;
        }

        if (ingredient.type == IngredientType.Marshmallow)
        {
            Debug.Log("Marshmallow tencereye eklenemez");
            return;
        }

        if (ingredients.Contains(ingredient.type))
        {
            Debug.Log("Zaten var: " + ingredient.type);
            return;
        }

        AddIngredient(ingredient);
    }



    void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient.type);
        Debug.Log("Tencereye eklendi: " + ingredient.type);

        DraggableItem draggable = ingredient.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.OnAddedToContainer();
        }
    }

    void TryPourIntoCup(Cup cup)
    {
        if (producedProduct == ProductType.None)
        {
            Debug.Log("Tencerede ürün yok");
            return;
        }

        if (cookResult != CookResult.Perfect)
        {
            Debug.Log("Ürün uygun piþmemiþ, kupaya dökülemez");
            return;
        }

        // ? asýl kritik yer burasý
        bool poured = cup.Fill(producedProduct);

        if (!poured)
        {
            Debug.Log("Ürün kupaya aktarýlamadý");
            return;
        }

        ResetPot();
        Debug.Log("Ürün baþarýyla kupaya döküldü");
    }


    public void Trash()
    {
        if (ingredients.Count == 0 && producedProduct == ProductType.None)
        {
            Debug.Log("Tencere zaten boþ");
            return;
        }

        Debug.Log("Tencere çöpe atýldý");

        ResetPot();
    }

}
