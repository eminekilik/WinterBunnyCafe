using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Trashable trashable = other.GetComponent<Trashable>();
        if (trashable == null) return;
        DraggableItem draggable = other.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.OnAddedToContainer();
        }
        trashable.Trash();
    }
}
