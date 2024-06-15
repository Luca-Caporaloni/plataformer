using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para la gestión de escenas

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool winCondition = false;
    public static int actualPlayer = 0;
    public List<Controller_Target> targets;
    public List<Controller_Player> players;

    void Start()
    {
        Physics.gravity = new Vector3(0, -30, 0);
        gameOver = false;
        winCondition = false;

        // Verifica que todos los jugadores estén correctamente asignados
        foreach (var player in players)
        {
            if (player == null)
            {
                Debug.LogError("A player in the players list is not assigned.");
                return;
            }
        }

        SetConstraits();
    }

    void Update()
    {
        GetInput();
        CheckWin();
    }

    private void CheckWin()
    {
        int i = 0;
        foreach (Controller_Target t in targets)
        {
            if (t.playerOnTarget)
            {
                i++;
            }
        }
        if (i >= 8)
        {
            winCondition = true;
            LoadNextLevel(); // Cargar el siguiente nivel
        }
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (actualPlayer <= 0)
            {
                actualPlayer = 8;
            }
            else
            {
                actualPlayer--;
            }
            SetConstraits();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (actualPlayer >= 8)
            {
                actualPlayer = 0;
            }
            else
            {
                actualPlayer++;
            }
            SetConstraits();
        }
    }

    private void SetConstraits()
    {
        foreach (Controller_Player p in players)
        {
            if (p == null)
            {
                Debug.LogError("Player in players list is null.");
                continue;
            }

            if (p.rb == null)
            {
                Debug.LogError("Rigidbody component is missing from player: " + p.gameObject.name);
                continue;
            }

            if (p == players[actualPlayer])
            {
                p.rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                p.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    public void LoadNextLevel()
    {
        // Asegúrate de que las escenas estén añadidas en Build Settings en la misma orden
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Comprueba si el índice de la siguiente escena es válido
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más niveles :(");
            // Aquí podrías añadir lógica para volver al primer nivel o mostrar una pantalla de fin del juego.
        }
    }
}