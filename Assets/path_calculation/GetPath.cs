using UnityEngine;
using System.Collections;

public class GetPath : MonoBehaviour {

    public MovePos target
    {
        get
        {         
            return mp.GetPos;
        }
    }

    public Vector2 rp = -Vector2.up * 2;
    private CharacterControl cc;
    private GetMovPoint mp;
    float speed = 20;
    Vector2[] path;
    int targetIndex;

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public bool walk = false;
    public Transform realMe;

    IEnumerator Start()
    {
        realMe = transform.parent;
        cc = realMe.GetComponent<CharacterControl>();
        cc.path = this;
        mp = GetComponent<GetMovPoint>();  
        
        while (Grid_creator.instance == null || !Grid_creator.instance.Loaded)
            yield return new WaitForSeconds(0.2f);
        StartCoroutine(UpdatePath());
        StartCoroutine(CheckIfStuck());
    }

    public void StopPath()
    {
        FinalDis = true;
        StopCoroutine("FollowPath");
    }

    float dist = 0;
    IEnumerator UpdatePath()
    {   
        while(!target.Move)
            yield return new WaitForSeconds(.3f);
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.pos, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector2 targetPosOld = target.pos;

        float temp = Vector2.Distance(transform.position, target.pos);
        while (target.Move)
        {                    
            
            if ((target.pos - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.pos, OnPathFound));
                targetPosOld = target.pos;
            }
            yield return new WaitForSeconds(minPathUpdateTime);
        }

        yield return UpdatePath();
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public void OnPathFound(MovePos ps)
    {
        if (ps.Move)
        {
            //path = new Vector2[1] { ps.pos };
            targetIndex = 0;
            realMe.position = ps.pos;
            StopCoroutine("FollowPath");
            //StartCoroutine("FollowPath");
        }
    }

    IEnumerator CheckIfStuck()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if (!FinalDis)
                continue;
            OnPathFound(Grid_creator.instance.WalkableNearMeNodePos(transform.position));
            
        }
    }
    Vector2 t = Vector2.zero;
    CharacterDirection dir;
    Vector2 currentWaypoint;
    Vector2 last_p;
    bool FinalDis = true;
    public bool NotMoving { get { return FinalDis; } }

    float dists = 10;
    float min_dists = 200;
    float s;
    IEnumerator FollowPath()
    {
        currentWaypoint = path[0];
        dir = CharacterDirection.back;
        t = -(((Vector2)this.realMe.position + rp) - currentWaypoint).normalized;
        last_p = t;
        double inc = 1;
        FinalDis = false;
        if (Mathf.Abs(t.x) > Mathf.Abs(t.y))
        {
            if (t.x > 0)
                dir = CharacterDirection.right;
            else
                dir = CharacterDirection.left;
        }
        else
        {
            if (t.y > 0)
                dir = CharacterDirection.back;
            else
                dir = CharacterDirection.front;
        }
        currentWaypoint = path[targetIndex];
        StopCoroutine("RefreshDir");
        StartCoroutine("RefreshDir");
        float dist;
        while (true)
        {
            if(!walk || cc.action != CharacterAnimation.idle && cc.action != CharacterAnimation.Walk)
            {
                yield return new WaitForSeconds(Time.deltaTime * 10f);
                continue;
            }
            dist = Vector2.Distance(((Vector2)realMe.position + rp), currentWaypoint);
            if (dist < 1.75f)
            {
                if (targetIndex >= path.Length - 1)
                {
                    if(dist < 0.75f)
                    {
                        FinalDis = true;
                        StartCoroutine("RefreshDir");
                        mp.Reset();
                        yield break;
                    }
                }
                else
                {
                    targetIndex++;
                    CalculateDir();
                    inc = 0;
                }
            }

            if (inc < 1)
            {
                inc = 0.25f * cc.walk_speed.speed_t;
                if(inc > 1)
                    inc = 1;
                
            }

            s = (dist / dists) * 3;
            if(s > 1)
                s = 1;

            t = t * (1 - (float)inc) + last_p * (float)inc * s;

            cc.Movement = t;
            cc.direction = dir;
           
            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    IEnumerator RefreshDir()
    {
        yield return new WaitForSeconds(0.1f);
        while(!FinalDis)
        {
            CalculateDir();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void CalculateDir()
    {
        currentWaypoint = path[targetIndex];
        last_p = -(((Vector2)this.realMe.position + rp) - currentWaypoint).normalized;
        if (Mathf.Abs(last_p.x) > Mathf.Abs(last_p.y * 0.67f))
        {
            if (last_p.x > 0)
                dir = CharacterDirection.right;
            else
                dir = CharacterDirection.left;
        }
        else
        {
            if (last_p.y > 0)
                dir = CharacterDirection.back;
            else
                dir = CharacterDirection.front;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            if (realMe == null)
                return;

            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine((Vector2)realMe.position + rp, path[i]);
                }
                else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }              
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine((Vector2)realMe.position + rp, (Vector2)realMe.position + rp + (Vector2)t * 30);

        }
    }
}
