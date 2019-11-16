using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ReloadGame : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(Time.deltaTime * 1);
        print(AIplayer.AI.Count + " AI last count!");
        AIplayer.AI = new List<Health>();
        while (DontDestroy.first.Loading)
            yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
	
}
