using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float delayBeforeSwitch;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(delayBeforeSwitch, () =>
        {
            SceneSwitch();
        });
    }

    void SceneSwitch()
    {
        SceneManager.LoadScene(sceneName);
    }
}
