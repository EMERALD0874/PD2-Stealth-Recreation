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

    float mouseSens;
    public bool gameOver;
    string reason;
    
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
        reason = "ERR Unknown Reason";
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
        Debug.Log(reason);
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
