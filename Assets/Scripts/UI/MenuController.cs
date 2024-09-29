using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnPlayClicked()
    {
        var ao = SceneManager.LoadSceneAsync(Constant.GAME_SCENE);
        if (ao != null) ao.allowSceneActivation = true;
    }
}