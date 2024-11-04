using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutsceneTT : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI terminalText;
    [SerializeField] float typingSpeed = 0.05f;

    [SerializeField] string[] lines = {
        "Processing...",
        "Shut up system...",
        "Please wait.",
        "Close botToHuman protocol...",
        "Ally in transportation mode now",
        "Thanks for playing!",
        "Creators:",
        "DebranFaust: Unity Dev",
        "Erosrolf: Unity Dev, Narrative",
        "Turbo Laser: Art, Level Design, Unity Dev",
        "Zakary Tuktarov: Sound Design, Composer",
    };

    private bool isCutsceneEnd = false;

    void Start()
    {
        terminalText.text = "~/ally/>";

        StartCoroutine(TypeText(" sleep --transportation-mode", false));
    }

    private void Update()
    {
        if(isCutsceneEnd)
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