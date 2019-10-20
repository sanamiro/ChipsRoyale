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
        DontDestroyOnLoad(this.gameObject);
        videoPlayer.SetActive(true);
        StartCoroutine(StopVideo(13.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if (videoFade)
            videoPlayer.GetComponent<VideoPlayer>().targetCameraAlpha--;
    }

    public IEnumerator Endgame(int playerId)
    {
        yield return new WaitForSeconds(10.0f);

        SceneManager.LoadScene(0);
    }

    public IEnumerator StopVideo(float videoLength)
    {
        Debug.Log("testos");

        yield return new WaitForSeconds(videoLength);

        videoFade = true;
        videoPlayer.SetActive(false);
    }
}
