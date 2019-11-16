using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class drag_ui : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    void Start()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.parent.position = eventData.position - ((Vector2)transform.localPosition / 2);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }
}

