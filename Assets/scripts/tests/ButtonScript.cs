using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	public void GoTo(string name) {
        Pause(false);
        SceneController.GoToScene(name);
    }

    public void Show(Animator animator) {
        AudioController.PlayTapSnd(1f);
        animator.SetTrigger("Show");
    }

    public void Hide(Animator animator) {
        AudioController.PlayTapSnd(1f);
        animator.SetTrigger("Hide");
    }

    public void Exit() {
        AudioController.PlayTapSnd(1f);
        Application.Quit();
    }

    public void Pause(bool b) {
        if (b) {
            Time.timeScale = 0.0f;
            AudioController.StopBgm();
        } else {
            Time.timeScale = 1.0f;
            AudioController.ContinueBgm();
        }
        AudioController.PlayTapSnd(1f);
    }

    public void EnableObject(GameObject o) {
        AudioController.PlayTapSnd(1f);
        o.SetActive(true);
    }

    public void DisableObject(GameObject o)
    {
        AudioController.PlayTapSnd(1f);
        o.SetActive(false);
    }
}
