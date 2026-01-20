using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    public static DecorationManager Instance;

    public bool HasMarshmallow;
    public bool HasCream;
    public bool HasChocolate;

    void Awake()
    {
        Instance = this;
    }
}
