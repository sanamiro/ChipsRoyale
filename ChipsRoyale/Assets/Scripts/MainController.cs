using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public int numberOfPlayers = 0;
    public GameManager gameManager;
    public GameObject ChipsPlayerPrefab;
    public GameObject HandPrefab;
    public GameObject Liquide;
    public GameObject TextWin;

    public List<Material> materialList;

    private List<int> m_playerJoystickList = new List<int>();
    private PlayAudio m_audioComp;

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
                playerCont.GetComponentInChildren<PlayerController>().joystick = gameManager.playerList[i];
                playerCont.GetComponentInChildren<MeshRenderer>().material = materialList[i];
            }
        }

        m_audioComp = Camera.main.GetComponent<PlayAudio>();
    }

    void Update()
    {
        float r = Random.Range(0.0f, 1.0f);
        if (r > 0.99f && numberOfPlayers > 1)
            SpawnHand();
        if (r < 0.01f && numberOfPlayers > 1)
            SpawnLiquide();
    }

    public void PlayerDied(int joystickId)
    {
        numberOfPlayers--;
        m_playerJoystickList.Remove(joystickId);
        if (numberOfPlayers == 1)
        {
            int playerWinner = m_playerJoystickList[0];
            Debug.Log("PLAYER " + playerWinner + " WON");
            StartCoroutine(gameManager.Endgame(playerWinner));
            m_audioComp.PlaySound(PlayAudio.Son.Victory);

            TextWin.SetActive(true);
            TextWin.GetComponent<Text>().text = "Player " + playerWinner + " Won !";
        }
    }

    private void SpawnHand()
    {
        // Modifier la position de spawn en fonction du coefHand du Player
        GameObject mostSaucedChips = null;
        GameObject lessSaucedChips = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Chips"))
        {
            float sauce = go.GetComponentInChildren<PlayerController>().coefHand;
            if (sauce > 1f) {
                if (mostSaucedChips == null)
                {
                    mostSaucedChips = go;
                }
                else if (sauce > mostSaucedChips.GetComponentInChildren<PlayerController>().coefHand)
                {
                    mostSaucedChips = go;
                }
            }

            if (sauce < 1f) {
                if (lessSaucedChips == null)
                {
                    lessSaucedChips = go;
                }
                else if (sauce < lessSaucedChips.GetComponentInChildren<PlayerController>().coefHand)
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

    private void SpawnLiquide()
    {

        float x = Random.Range(-7.5f, 7.5f);
        float y = Random.Range(-5.0f, 5.0f);
        float r = Random.Range(0f, 360f);

        Instantiate(Liquide, new Vector3(x, 0, y), Quaternion.Euler(0f, r, 0f));
    }
}
