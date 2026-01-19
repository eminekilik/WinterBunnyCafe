using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int totalMoney;
    TMP_Text totalMoneyText;

    const string MONEY_TEXT_NAME = "TotalMoneyText";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMoney();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // sadece menüde aramak istiyorsan aç
        // if (scene.name != "Menu") return;

        totalMoneyText = FindTMPTextDeep(MONEY_TEXT_NAME);

        UpdateUI();
    }

    TMP_Text FindTMPTextDeep(string name)
    {
        foreach (var canvas in FindObjectsOfType<Canvas>(true))
        {
            TMP_Text result = FindInChildren(canvas.transform, name);
            if (result != null)
                return result;
        }
        return null;
    }

    TMP_Text FindInChildren(Transform parent, string name)
    {
        if (parent.name == name)
            return parent.GetComponent<TMP_Text>();

        foreach (Transform child in parent)
        {
            TMP_Text result = FindInChildren(child, name);
            if (result != null)
                return result;
        }

        return null;
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        SaveMoney();
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (totalMoney < amount) return false;

        totalMoney -= amount;
        SaveMoney();
        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (totalMoneyText != null)
            totalMoneyText.text = totalMoney.ToString();
    }

    void SaveMoney()
    {
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
    }

    void LoadMoney()
    {
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
    }
}
