using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public List<int> playerList = new List<int>();
    public List<Color32> colorList = new List<Color32>();

    public GameObject videoPlayer;

    private bool videoFade = false;

    void Start()
    {
        Debug.Log("testos " + FindObjectsOfType<GameManager>().Length);
        if (FindObjectsOfType<GameManager>().Length > 1)
            Destroy(this.gameObject);
        StartCoroutine(StopVideo(13.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (videoFade && videoPlayer.GetComponent<VideoPlayer>().targetCameraAlpha > 0)
            videoPlayer.GetComponent<VideoPlayer>().targetCameraAlpha--;
    }

    public IEnumerator Endgame(int playerId)
    {
        yield return new WaitForSeconds(10.0f);

        SceneManager.LoadScene(0);
    }

    public IEnumerator StopVideo(float videoLength)
    {
        yield return new WaitForSeconds(0.5f);
        
        DontDestroyOnLoad(this.gameObject);
        videoPlayer.SetActive(true);

        yield return new WaitForSeconds(videoLength - 0.5f);

        videoFade = true;
    }
}
