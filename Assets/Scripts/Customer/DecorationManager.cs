using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    public static DecorationManager Instance;

    [Header("Decoration Objects")]
    [SerializeField] GameObject marshmallowDecoration;
    [SerializeField] GameObject creamDecoration;
    [SerializeField] GameObject chocolateDecoration;

    public bool HasMarshmallow { get; private set; }
    public bool HasCream { get; private set; }
    public bool HasChocolate { get; private set; }

    void Awake()
    {
        Instance = this;
        UpdateStates();
    }

    public void UpdateStates()
    {
        HasMarshmallow = marshmallowDecoration != null && marshmallowDecoration.activeInHierarchy;
        HasCream = creamDecoration != null && creamDecoration.activeInHierarchy;
        HasChocolate = chocolateDecoration != null && chocolateDecoration.activeInHierarchy;
    }
}
