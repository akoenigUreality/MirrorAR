using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInScreen : UIViewState {

    private string _name;
    private Button _AcceptButton;

    private void Start()
    {
        _AcceptButton = GetComponentInChildren<Button>();
    }

    public void UpdateName(string name)
    {
        _name = name;
        ApplicationSceneController.Instance().UpdateUserName(name);
        Debug.Log(_name);
        //PlayAnimation
        _AcceptButton.enabled = (_name != null);

    }

    public void TriggerTrainingSelection()
    {
        ApplicationSceneController.Instance().TriggerStateChange(SceneNames.Selection);
    }
}
