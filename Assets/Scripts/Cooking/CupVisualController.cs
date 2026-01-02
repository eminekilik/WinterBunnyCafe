using UnityEngine;

public class CupVisualController : MonoBehaviour
{
    [SerializeField] private Cup cup;
    [SerializeField] private SpriteRenderer cupRenderer;

    [Header("Empty")]
    public Sprite emptyCup;

    [Header("Base Products")]
    public Sprite hotMilk;
    public Sprite meltedChocolate;
    public Sprite hotChocolate;

    [Header("Toppings")]
    public Sprite hotMilk_Marshmallow;
    public Sprite meltedChocolate_Marshmallow;
    public Sprite hotChocolate_Marshmallow;

    void Update()
    {
        UpdateCupSprite();
    }

    void UpdateCupSprite()
    {
        // ?? boþ kupa
        if (!cup.HasProduct())
        {
            cupRenderer.sprite = emptyCup;
            return;
        }

        bool hasMarshmallow = cup.toppings.Contains(ToppingType.Marshmallow);
        // ?? ürün + topping kombinasyonu
        switch (cup.currentProduct)
        {
            case ProductType.HotMilk:
                cupRenderer.sprite = hasMarshmallow
                    ? hotMilk_Marshmallow
                    : hotMilk;
                break;

            case ProductType.MeltedChocolate:
                cupRenderer.sprite = hasMarshmallow
                    ? meltedChocolate_Marshmallow
                    : meltedChocolate;
                break;

            case ProductType.HotChocolate:
                cupRenderer.sprite = hasMarshmallow
                    ? hotChocolate_Marshmallow
                    : hotChocolate;
                break;
        }
    }
}
