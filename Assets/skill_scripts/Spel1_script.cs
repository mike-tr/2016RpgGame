 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spel1_script : MonoBehaviour {

    Attack_data ad;
    public float er = 1;

    public Vector2 rdl;
    private Rigidbody2D rd;
    private Vector2 temp;

    private float sp;
    public float speed
    {
        set
        {
            sp = Random.Range(value * 0.5f, value * 1.5f);
            if (sp > 300f)
                sp = 300f;
            else if (sp < 25f)
                sp = 25f;
        }
    }

    private Sprite originaly;
    private SpriteRenderer sr;
    private Vector3 scale_original;
    private List<Sprite> explode = new List<Sprite>();
    private float radius;
    CircleCollider2D my_col;
    private bool re = false;
	// Use this for initialization
	void Start () {
        foreach (Sprite s in Resources.LoadAll<Sprite>("Character/Skills/sprite/explosion"))
        {
            explode.Add(s);
        }
        rd = GetComponent<Rigidbody2D>();
        ad = GetComponent<Attack_data>();
        StartCoroutine(Explode());
        StartCoroutine(SelfDistract());

        temp = rd.velocity;

        rdl = -transform.right * sp;
        rd.velocity = Vector2.zero;

        my_col = GetComponent<CircleCollider2D>();
        radius = my_col.radius;
        sr = GetComponent<SpriteRenderer>();
        originaly = sr.sprite;
        scale_original = Vector3.one * 0.75f;
        transform.localScale = scale_original;

        Physics2D.IgnoreLayerCollision( gameObject.layer, gameObject.layer, true);

        re = true;
    }

    public void ReLoadSpell()
    {
        if (!re)
            return;

        sr.sprite = originaly;
        transform.localScale = scale_original;

        rdl = -transform.right * sp;
        rd.velocity = Vector2.zero;
        my_col.radius = radius;

        StartCoroutine(Explode());
        StartCoroutine(SelfDistract());
    }

    void LateUpdate()
    {
        if (rd.velocity != Vector2.zero)
        {
            rdl = rd.velocity;
            rd.velocity = Vector2.zero;
        }
        transform.position += (Vector3)rdl * Time.deltaTime;
    }

    public void ZeroV(bool a = false)
    {
        if (!a)
        {
            rd.velocity = Vector2.zero;
            rdl = rd.velocity;
        }
        rd.velocity = temp;
        rdl = temp * Random.Range(0.2f, 1f);
    }



    IEnumerator SelfDistract()
    {
        yield return new WaitForSeconds(3);
        ad.DMG();
    }

    // Update is called once per frame
    IEnumerator Explode()
    {
        ad.triggered = false;
        while(!ad.triggered)
            yield return null;

        yield return new WaitForSeconds(Time.deltaTime * Random.Range(1f, 20f));

        GetComponent<Rigidbody2D>().isKinematic = true;
        er = er / 30;
        rdl = Vector2.zero;
        for (float i = 0; i < 5; i++)
        {
            yield return null;
            transform.localScale = new Vector2(transform.localScale.x * (1 + er), transform.localScale.y * (1 + er));
            sr.color = new Color(1, ((30 - i) / 30), ((30 - i) / 30), ((30 - i) / 90) + 0.66f);
        }
        transform.localScale = new Vector2(1,1);
        sr.color = new Color(1,1,1);
        
        sr.sprite = explode[0];
        rdl = Vector3.zero;
        for (int i = 1; i < explode.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sr.sprite = explode[i];
            my_col.radius = my_col.radius * 0.5f;
            
            //transform.localScale = new Vector2(transform.localScale.x * (1 - er * 2), transform.localScale.y * (1 - er * 2));
        }
        yield return Sleep_after(Time.deltaTime);
    }

    IEnumerator Sleep_after(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == name)
        {            
            return;
        }

        if (coll.transform.tag == "Attack")
        {
            rd.velocity = Vector2.zero;
            ad.DMG(true);
            return;
        }
        else if(coll.transform.tag == "character_box")
        {           
            return;
        }

        Vector3 dirc = (coll.gameObject.transform.position - gameObject.transform.position).normalized;
        int reverse_x = 1;
        int reverse_y = 1;

        if (Mathf.Abs(dirc.x) > Mathf.Abs(dirc.y))
            reverse_x = (dirc.x < 0) ? 1 : -1;
        else
            reverse_y = (dirc.y < 0) ? 1 : -1;


        Vector2 rdln = rdl;
        rdln.x = rdl.x * reverse_x;
        rdln.y = rdl.y * reverse_y;
        Vector2 nv = Vector2.Scale(dirc, rdln);

        Vector2 AddN = Vector2.zero;
        AddN.x = Mathf.Abs(rdln.x) - Mathf.Abs(nv.x);
        AddN.y = Mathf.Abs(rdln.y) - Mathf.Abs(nv.y);
        rd.velocity = nv + AddN;

    }
}
