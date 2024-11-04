using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalTextLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI terminalText;
    [SerializeField] float typingSpeed = 0.05f;

    [SerializeField] string[] lines = {
        "Loading...",
        "Initializing system...",
        "Please wait.",
        "Loading botToHuman protocol...",
        "Loading emergency scripts...",
        "Loadin.. ░ ▒ ▓ ╠	╡	╢	╣	╤	╥	╦	╧	╨	╩	╪	╫	╬	▀	▄		┘	┐	┌	└	┤	┴	┬	├	─	│	┼	",
        "Loading complete."
    };

    private bool isCutsceneEnd = false;

    void Start()
    {
        terminalText.text = "~/ally/>";

        StartCoroutine(TypeText(" wake up -a7 --human --emergency", false));
    }

    private void Update()
    {
        if(Input.anyKeyDown || isCutsceneEnd)
            {
                LoadNextScene();
            }
    }

    IEnumerator TypeText(string line, bool clear)
    {
        if (clear)
            terminalText.text += "\n";

        foreach (char letter in line)
        {
            terminalText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(TypeAllStrings());
    }

        IEnumerator TypeAllStrings()
    {
        foreach (string line in lines)
        {
            terminalText.text += "\n";
            foreach (char letter in line)
            {
                terminalText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(1f);
        }
        isCutsceneEnd = true;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}