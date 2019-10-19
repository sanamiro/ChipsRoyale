using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject charaSelectionMenu;
    public Button playButton;
    public Button quitButton;
    public List<GameObject> playerVisuals;

    private bool m_GoToCharaSelect = false;
    private bool m_GoToMainMenu = false;
    private bool m_OnCharaSelect = false;
    private bool m_isStartable = false;

    private int[] m_playerPositionList = new int[4];            //contient la position dans le tableau de sélection de chaque joystick
    private List<int> m_playerControllerList;                   //contient la liste des joueurs prêts à jouer

    void Start()
    {
        m_playerControllerList = new List<int>();

        for (int i = 0; i < 4; i++)                             //On initialise la pos de chaque joystick
            m_playerPositionList[i] = 0;                      //0 = joystick non activé sur l'écran de sélection
    }
    
    void Update()
    {
        if (m_OnCharaSelect)
        {
            for (int i = 1; i < 5; i++)
            {
                if (Input.GetButtonUp("A" + i))
                    AddPlayer(i);

                if (Input.GetButtonUp("B" + i))
                    RemovePlayer(i);

                if (Input.GetButtonUp("Start" + i) && m_isStartable)
                    StartGame();
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_GoToCharaSelect)
        {
            if (mainMenu.transform.localPosition.y < 450)
            {
                mainMenu.transform.position = new Vector3(mainMenu.transform.position.x, mainMenu.transform.position.y + 3, mainMenu.transform.position.z);
                charaSelectionMenu.transform.position = new Vector3(charaSelectionMenu.transform.position.x, charaSelectionMenu.transform.position.y + 3, charaSelectionMenu.transform.position.z);
            }
            else
            {
                m_GoToCharaSelect = false;
                m_OnCharaSelect = true;
                mainMenu.SetActive(false);
            }
        }

        if (m_GoToMainMenu)
        {
            if (mainMenu.transform.localPosition.y > 0)
            {
                mainMenu.transform.position = new Vector3(mainMenu.transform.position.x, mainMenu.transform.position.y - 3, mainMenu.transform.position.z);
                charaSelectionMenu.transform.position = new Vector3(charaSelectionMenu.transform.position.x, charaSelectionMenu.transform.position.y - 3, charaSelectionMenu.transform.position.z);
            }
            else
            {
                m_GoToMainMenu = false;
            }
        }

    }

    public void OnPlayButton()
    {
        if (mainMenu.activeSelf && !m_GoToCharaSelect && !m_GoToMainMenu)
            m_GoToCharaSelect = true;
    }

    public void OnQuitButton()
    {
        if (mainMenu.activeSelf && !m_GoToCharaSelect && !m_GoToMainMenu)
            Application.Quit();
    }

    public void OnBackButton()
    {
        if (charaSelectionMenu.activeSelf)
        {
            m_GoToMainMenu = true;
            m_OnCharaSelect = false;
            mainMenu.SetActive(true);
        }
    }

    private void AddPlayer(int playerId)
    {
        if (!m_playerControllerList.Contains(playerId))                         //Si le joystick n'est pas encore ajouté
        {
            m_playerControllerList.Add(playerId);                               //On l'ajoute

            for (int i = 0; i < 4; i++)                                         //On parcourt tous les visuels
            {
                if (!playerVisuals[i].activeSelf)                               //Si on trouve un visuel non actif
                {
                    m_playerPositionList[playerId - 1] = i;                     //La position du joystick est ajouté à la liste
                    playerVisuals[i].SetActive(true);
                    i = 3;                                                      //On sort de la boucle
                }
            }

            if (m_playerControllerList.Count >= 2)                              //Si on a 2 joueurs ou plus
                m_isStartable = true;                                           //On peut lancer la partie
        }
    }

    private void RemovePlayer(int playerId)
    {
        if (m_playerControllerList.Contains(playerId))
        {
            playerVisuals[m_playerPositionList[playerId - 1]].SetActive(false);
            m_playerPositionList[playerId - 1] = 0;
            m_playerControllerList.Remove(playerId);
            if (m_playerControllerList.Count < 2)
                m_isStartable = false;
        }
        else
            OnBackButton();
    }

    private void StartGame()
    {
        Debug.Log("START GAME");
    }
}
