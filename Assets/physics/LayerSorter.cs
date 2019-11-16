using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LayerSorter : MonoBehaviour
{
    private SpriteRenderer ms;

    public int change_need = 0;
    public float size_y;

    public int SortDown = 0;
    Transform Child;
    // Use this for initialization
    IEnumerator Start()
    {
        ms = GetComponent<SpriteRenderer>();

        while(ms.sprite == null)
            yield return new WaitForSeconds(Time.deltaTime * 5);
        try
        {
            Child = transform.GetChild(0);
        }
        catch
        {
            Child = this.transform;
        }


        while (TGMap.instance == null)
            yield return null;

        TDTile t = TGMap.instance.TileFromWorldPoint(Child.position);
        ms.sortingOrder = 32767 - Mathf.Abs((t.x * -20) + (t.y * 30 * TGMap.instance.size_x)) - SortDown;
        //StartCoroutine(WaitAndPrint());
        if(SortDown <= 20) 
        {
            int sortd = (int)(transform.position.y % TGMap.instance.tileSize);
            ms.sortingOrder += sortd;
            if (!transform.name.Contains("tree"))      
                ms.sortingLayerName = "all";
            else
            {
                //transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = ms.sortingOrder;
            }
            
        }
        else
            ms.sortingLayerName = "ground";
        if(Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }
    // Update is called once per frame
}

