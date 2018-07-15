using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SceneNames
{
    IntroScene,
    Selection,
    Training,
    Evaluation
}

public delegate void OnSceneChange(SceneNames newScene);

public class ApplicationSceneController: MonoBehaviour{

    public event OnSceneChange _OnSceneChange;
    private StatemashineModel _model;
    private static ApplicationSceneController _instance;

    public static ApplicationSceneController Instance()
    {
        if(_instance== null)
        {
            GameObject obj = new GameObject();
            obj.name = "SceneController";
            _instance = obj.AddComponent<ApplicationSceneController>();

        }

        return _instance;
    }

    /// <summary>
    /// Register Default storage event
    /// </summary>
    void Awake()
    {
        Debug.Log("INIT Controller");
        _model = new StatemashineModel();
        _OnSceneChange += AppllicationSceneController__OnSceneChange;
    }

    private void Start()
    {
        TriggerStateChange(SceneNames.IntroScene);
    }

    /// <summary>
    /// Stores the running State
    /// </summary>
    /// <param name="newScene"></param>
    private void AppllicationSceneController__OnSceneChange(SceneNames newScene)
    {
        _model.RunningScene = newScene;
    }

    /// <summary>
    /// Triggers a stateChange for the Application
    /// </summary>
    /// <param name="newScene"></param>
    public void TriggerStateChange(SceneNames newScene)
    {
        if (_OnSceneChange != null)
        {
            _OnSceneChange(newScene);
        }
    }

    public void UpdateUserName(string name)
    {
        _model.UserName = name;
    }

    /// <summary>
    /// State Mashine Model stores the information about the Overall App
    /// </summary>
    protected class StatemashineModel
    {
        private SceneNames _runningScene;
        private string _userName = "";

        public SceneNames RunningScene
        {
            get
            {
                return _runningScene;
            }

            set
            {
                _runningScene = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value;
            }
        }
    }

}
