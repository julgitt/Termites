using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Time.timeScale = 1f;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Time.timeScale = 2f;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Time.timeScale = 3f;
    }
}
