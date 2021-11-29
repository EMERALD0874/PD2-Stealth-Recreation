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

    float mouseSens;
    bool gameOver;
    
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void GameOver()
    {
        gameOver = true;
        StopPlayerMovement();
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
