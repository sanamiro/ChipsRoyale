using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public int numberOfPlayers = 0;
    public GameManager gameManager;
    public GameObject ChipsPlayerPrefab;
    public GameObject HandPrefab;

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
        float r = Random.Range(0.0f, 1.0f);
        if (r > 0.99f)
            SpawnHand();
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

    private void SpawnHand()
    {
        // Modifier la position de spawn en fonction du coefHand du Player
        GameObject mostSaucedChips = null;
        GameObject lessSaucedChips = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Chips"))
        {
            float sauce = go.GetComponent<PlayerController>().coefHand;
            if (sauce > 1f) {
                if (mostSaucedChips == null)
                {
                    mostSaucedChips = go;
                }
                else if (sauce > mostSaucedChips.GetComponent<PlayerController>().coefHand)
                {
                    mostSaucedChips = go;
                }
            }

            if (sauce < 1f) {
                if (lessSaucedChips == null)
                {
                    lessSaucedChips = go;
                }
                else if (sauce < lessSaucedChips.GetComponent<PlayerController>().coefHand)
                {
                    lessSaucedChips = go;
                }
            }
        }

        float x = Random.Range(-7.5f, 7.5f);
        float y = Random.Range(-5.0f, 5.0f);

        if (mostSaucedChips == null)
        {
            if (lessSaucedChips != null)
            {
                while (Vector2.Distance(new Vector2(x, y), new Vector2(lessSaucedChips.transform.position.x, lessSaucedChips.transform.position.z)) < 2f)
                {
                    x = Random.Range(-7.5f, 7.5f);
                    y = Random.Range(-5.0f, 5.0f);
                }
            }
        }
        else
        {
            x = Random.Range(-1.0f, 1.0f) + mostSaucedChips.transform.position.x;
            y = Random.Range(-1.0f, 1.0f) + mostSaucedChips.transform.position.z;
        }

        GameObject hand = Instantiate(HandPrefab, this.transform.parent);
        hand.transform.position = new Vector3(x, 0, y);
    }
}
