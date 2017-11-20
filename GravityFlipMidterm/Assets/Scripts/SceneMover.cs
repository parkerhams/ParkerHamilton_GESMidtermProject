using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public void StartButtonClicked()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void CreditsButtonClicked()
    {
        SceneManager.LoadScene("CreditsScreen");
    }

    public void TitleButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen");
    }


}
