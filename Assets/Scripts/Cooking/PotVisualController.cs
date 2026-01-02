using UnityEngine;

public class PotVisualController : MonoBehaviour
{
    [SerializeField] private Pot pot;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Empty")]
    public Sprite emptyPot;

    [Header("Raw States")]
    public Sprite milkRaw;
    public Sprite chocolateRaw;
    public Sprite milkChocolateRaw;

    [Header("Perfect States")]
    public Sprite hotMilk;
    public Sprite meltedChocolate;
    public Sprite hotChocolate;

    [Header("Burnt")]
    public Sprite burnt;

    void Update()
    {
        UpdateSprite();
    }

    void UpdateSprite()
    {
        // ?? tamamen boþ
        if (pot.ingredients.Count == 0 && pot.producedProduct == ProductType.None)
        {
            spriteRenderer.sprite = emptyPot;
            return;
        }

        // ?? yanmýþ
        if (pot.cookResult == CookResult.Overcooked)
        {
            spriteRenderer.sprite = burnt;
            return;
        }

        bool hasMilk = pot.ingredients.Contains(IngredientType.Milk);
        bool hasChocolate = pot.ingredients.Contains(IngredientType.Chocolate);

        // ?? piþiyor / az piþmiþ
        if (pot.cookResult == CookResult.None || pot.cookResult == CookResult.Undercooked)
        {
            if (hasMilk && hasChocolate)
                spriteRenderer.sprite = milkChocolateRaw;
            else if (hasMilk)
                spriteRenderer.sprite = milkRaw;
            else if (hasChocolate)
                spriteRenderer.sprite = chocolateRaw;

            return;
        }

        // ?? tam kývam
        if (pot.cookResult == CookResult.Perfect)
        {
            switch (pot.producedProduct)
            {
                case ProductType.HotMilk:
                    spriteRenderer.sprite = hotMilk;
                    break;

                case ProductType.MeltedChocolate:
                    spriteRenderer.sprite = meltedChocolate;
                    break;

                case ProductType.HotChocolate:
                    spriteRenderer.sprite = hotChocolate;
                    break;
            }
        }
    }
}
