using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewState : MonoBehaviour {

    private Animator _animator;
    public SceneNames scene;

	void Awake () {

        ApplicationSceneController.Instance()._OnSceneChange += UIViewState__OnSceneChange;
		
	}

    private void UIViewState__OnSceneChange(SceneNames newScene)
    {
        bool isActive = (newScene == scene);
        Debug.Log("isSceneActive" + isActive);

        gameObject.SetActive(isActive);

    }
}
