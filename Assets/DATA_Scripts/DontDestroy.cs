using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    public static DontDestroy first;
    public bool Loading = false;
	// Use this for initialization
	void Awake() {
        if (first == null)
        {
            first = this;
            DontDestroyOnLoad(this.gameObject);
            AIplayer.AI = new List<Health>();
        }
        else if (first != this)
            Destroy(this.gameObject);
    }

    public void LoadAll()
    {
        Loading = true;
        SceneManager.LoadScene(1);
    }

}

