using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance { get { return _instance; } }

    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Gun gun;
    [SerializeField] AudioSource alarm;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMPro.TextMeshProUGUI gameOverText;

    float mouseSens;
    public bool gameOver;
    string reason;
    public List<Vector3> suspiciousObjects;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        mouseSens = mouseLook._mouseSens;
        gameOver = false;
        gameOverPanel.SetActive(false);
        reason = "ERR Unknown Reason";
        suspiciousObjects = new List<Vector3>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void GameOver(string r)
    {
        reason = r;
        gameOver = true;
        StopPlayerMovement();

        alarm.Play();
        gameOverPanel.SetActive(true);
        gameOverText.text = reason;
    }

    public void GameWon(string r)
    {
        reason = r;
        gameOver = true;
        StopPlayerMovement();

        //alarm.Play();
        gameOverPanel.SetActive(true);
        gameOverText.text = reason;
    }

    public void StartPlayerMovement()
    {
        if (gameOver)
            return;

        playerMovement.SetMovement(true);
        mouseLook._mouseSens = mouseSens;
        gun.canFire = true;
    }

    public void StopPlayerMovement()
    {
        playerMovement.SetMovement(false);
        mouseLook._mouseSens = 0f;
        gun.canFire = false;
    }
}
