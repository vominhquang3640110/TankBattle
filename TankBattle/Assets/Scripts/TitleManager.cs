using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickPlayButton()
    {
        SoundManager.instance.PlayClickButton();
        UserManager.instance.isTutorialMode = false;
        SceneManager.LoadScene("Matching");
    }
    public void OnClickTutorialButton()
    {
        SoundManager.instance.PlayClickButton();
        UserManager.instance.isTutorialMode = true;
        SceneManager.LoadScene("Tutorial");
    }
}
