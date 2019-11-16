using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_BackGround : MonoBehaviour {

	// Use this for initialization

	public Sprite Ground;
	GameObject game_Object;
	IEnumerator Start () {
		game_Object = (GameObject)Resources.Load("Envitoment/Ground");
		int i = 0;
		for(int x = - 250; x < 250; x += 16)
			for(int y = - 250; y < 250; y += 16, i++)
			{
				CreateTexture(x,y,Ground);
				if(i % 7 == 0)	
					yield return null;
			}
		gameObject.isStatic = true;
	}
	
	public void CreateTexture(float x, float y, Sprite sprite)
    {
        Transform child = Instantiate(game_Object).transform;
        SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

		child.gameObject.layer = 10;
        sr.sprite = sprite;
        child.SetParent(transform);
        child.localPosition = new Vector3(x, y, 0);
        child.localScale = new Vector3(1, 1, 1);
		child.gameObject.isStatic = true;
        //print(gt);
    }
}
