using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMovPoint : MonoBehaviour {
	public static GetMovPoint Main_Character;
	public MovePos GetPos
	{
		get 
		{
			if(pn.Move)
			{
				return pn;
			}
			return new MovePos(cc.target);
		}
	}

	public Transform EnemyCheck;
	public void Reset()
	{
		pn = MovePos.Empty;
	}
	private CharacterControl cc;
	// Use this for initialization

	Transform cam;
	void Start () {
		cc = transform.parent.GetComponent<CharacterControl>();
		cam = Camera.main.transform;	
		pn = new MovePos(false, Vector2.zero);

		if(cc.IsMine)
		{
			Main_Character = this;
			EnemyCheck = transform.GetChild(0);
		}

		
	}

	public void SetMovePoint(MovePos p)
	{
		pn = p;
		cc.ResetTarget();
		GetTarget();
	}
	private MovePos pn;


	void GetTarget()
    {
		StopCoroutine(GT());
		EnemyCheck.transform.position = pn.pos;
		StartCoroutine(GT());
    }

	IEnumerator GT()
	{
		EnemyCheck.gameObject.SetActive(true);
		//yield return new WaitForSeconds(Time.deltaTime * 1);
		yield return null;
		EnemyCheck.gameObject.SetActive(false);
	}
	// Update is called once per frame

}

public class MovePos
{
	public MovePos(bool m, Vector2 p)
	{
		Move = m;
		pos = p;
	}

	public MovePos(Transform target)
	{
		if(target != null)
		{
			Move = true;
			pos = target.position;
			return;
		}
	}

	public static MovePos Empty
	{
		get
		{
			return new MovePos(false, Vector2.zero);
		}
	}
	public bool Move = false;
	public Vector2 pos = Vector2.zero;
}