using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour, Trashable
{
    public ProductType currentProduct = ProductType.None;
    public List<ToppingType> toppings = new List<ToppingType>();

    public bool HasProduct()
    {
        return currentProduct != ProductType.None;
    }

    public bool CanAccept(ProductType product)
    {
        return product == ProductType.HotMilk
            || product == ProductType.MeltedChocolate
            || product == ProductType.HotChocolate;
    }

    public bool CanAddTopping(ToppingType topping)
    {
        if (!HasProduct())
            return false;

        if (toppings.Contains(topping))
            return false;

        // ileride ürün bazlý kurallar
        // if (currentProduct != ProductType.HotChocolate)
        //     return false;

        return true;
    }

    public void AddTopping(ToppingType topping)
    {
        if (!CanAddTopping(topping))
        {
            Debug.Log("Topping eklenemez: " + topping);
            return;
        }

        toppings.Add(topping);
        Debug.Log("Topping eklendi: " + topping);
    }

    public bool Fill(ProductType product)
    {
        if (HasProduct())
        {
            Debug.Log("Kupa zaten dolu");
            return false;
        }

        if (!CanAccept(product))
        {
            Debug.Log("Bu ürün kupaya konamaz: " + product);
            return false;
        }

        currentProduct = product;
        Debug.Log("Kupaya eklendi: " + product);

        return true;
        // ?? sprite / animasyon buraya
    }

    public void ResetCup()
    {
        currentProduct = ProductType.None;
        toppings.Clear();
    }

    public void Trash()
    {
        if (!HasProduct()) return;

        Debug.Log("Kupa çöpe atýldý: " + currentProduct);
        ResetCup();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ?? TOPPING (MARSHMALLOW) KONTROLÜ
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            if (ingredient.type != IngredientType.Marshmallow)
                return;

            DraggableItem draggable = ingredient.GetComponent<DraggableItem>();
            if (draggable != null && draggable.IsDragging)
            {
                draggable.OnAddedToContainer();
                AddTopping(ToppingType.Marshmallow);
            }

            return;
        }
    }


}
