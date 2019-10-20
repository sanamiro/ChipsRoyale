using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<int> playerList = new List<int>();
    public List<Color32> colorList = new List<Color32>();

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Endgame(int playerId)
    {
        yield return new WaitForSeconds(10.0f);

        SceneManager.LoadScene(0);
    }
}
