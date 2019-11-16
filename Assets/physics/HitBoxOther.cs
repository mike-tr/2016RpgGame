using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitBoxOther : MonoBehaviour {

    public Vector3 ApplyForce = Vector3.zero;
    private Transform p;

    Dictionary<string, bool> collO = new Dictionary<string, bool>();
    

    // Use this for initialization
    void Start()
    {
        collO = new Dictionary<string, bool>();
        transform.name = "skill01";
        p = transform.parent;
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        transform.parent.position += ApplyForce;
        //ApplyForce *= 0.95f;
        if (Vector3.Distance(ApplyForce, Vector3.zero) < 0.1f)
            ApplyForce = Vector3.zero;

    }

    public void NewColl(string collName, dir CollDir, Vector3 DirF, Vector3 AddForce = default(Vector3))
    {
        try
        {
            collO.Add(collName, true);
            print(GetDir(DirF, ApplyForce) + " " + DirF);
            ApplyForce = GetDir(DirF, ApplyForce);
            ApplyForce += AddForce;
        }
        catch
        {

        }
    }

    Vector3 GetDir(Vector3 dir, Vector3 f)
    {

        Vector3 ret = Vector3.zero;
        float forceX = f.x;
        float forceY = f.y;
        int dx = (dir.x >= 0) ? 1 : -1;
        int dy = (dir.y >= 0) ? 1 : -1;

        ret.x = forceX * dir.x + forceY * Mathf.Abs(dir.y) * dx;
        ret.y = forceY * dir.y + forceX * Mathf.Abs(dir.x) * dy;
        //float convY = forceY * dir.y;

        return ret;
    }

    public void CollExit(string collName, dir CollDir)
    {
        try
        {
            collO.Remove(collName);
        }
        catch
        {

        }
    }
}
