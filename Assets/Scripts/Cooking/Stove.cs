using UnityEngine;

public class Stove : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Pot pot;
    [SerializeField] StoveProgressBar progressBar;

    [Header("Cooking Settings")]
    [SerializeField] float cookDuration = 5f;
    [SerializeField] float perfectMin = 0.45f;
    [SerializeField] float perfectMax = 0.7f;

    void Update()
    {
        if (!pot.isCooking) return;

        pot.cookProgress += Time.deltaTime / cookDuration;
        pot.cookProgress = Mathf.Clamp01(pot.cookProgress);

        progressBar.SetProgress(pot.cookProgress);
    }

    public void ToggleStove()
    {
        // ÞU ANDA PÝÞÝYORSA ? DURDUR
        if (pot.isCooking)
        {
            StopCooking();
            return;
        }

        // DAHA ÖNCE BAÞLATILMIÞ AMA ARTIK PÝÞMÝYORSA ? HÝÇBÝR ÞEY YAPMA
        if (pot.hasStartedCooking)
            return;

        // HÝÇ BAÞLAMAMIÞSA ? BAÞLAT
        StartCooking();
    }


    void StartCooking()
    {
        if (!pot.HasIngredients()) return;

        pot.isCooking = true;
        pot.hasStartedCooking = true;
        pot.cookProgress = 0f;

        progressBar.Show(); // ?? burada açýlýr
    }

    void StopCooking()
    {
        pot.isCooking = false;

        DetermineCookResult();

        progressBar.Hide(); // ?? burada kapanýr

        pot.DetermineProduct();
        Debug.Log("Ürün: " + pot.producedProduct);

    }

    void DetermineCookResult()
    {
        if (pot.cookProgress < perfectMin)
            pot.cookResult = CookResult.Undercooked;
        else if (pot.cookProgress <= perfectMax)
            pot.cookResult = CookResult.Perfect;
        else
            pot.cookResult = CookResult.Overcooked;

        Debug.Log("Piþirme sonucu: " + pot.cookResult);
    }
}
