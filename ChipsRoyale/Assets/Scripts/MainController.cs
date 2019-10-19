using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public int numberOfPlayers = 0;
    public GameManager gameManager;
    public GameObject ChipsPlayerPrefab;

    private List<int> m_playerJoystickList = new List<int>();

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            numberOfPlayers = gameManager.playerList.Count;
            m_playerJoystickList = gameManager.playerList;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                GameObject playerCont = Instantiate(ChipsPlayerPrefab, this.transform.parent);
                playerCont.GetComponent<PlayerController>().joystick = gameManager.playerList[i];
            }
        }
    }

    void Update()
    {
        
    }

    public void PlayerDied(int joystickId)
    {
        numberOfPlayers--;
        m_playerJoystickList.Remove(joystickId);
        if (numberOfPlayers == 1)
        {
            int playerWinner = m_playerJoystickList[0];
            Debug.Log("PLAYER " + playerWinner + " WON");
            SceneManager.LoadScene(0);
        }
    }
}
