using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;

public class DataLogger : MonoBehaviour
{


    private static DataLogger _instance;
    private static StringBuilder _logfileTrialSummary;
    private static string dataPathTrialSummary;

    /// <summary>
    /// Returns the Instance of the DataLogger
    /// </summary>
    /// <returns></returns>
    public static DataLogger Instance()
    {
        if (!_instance)
        {
            GameObject logger = new GameObject();
            _instance = logger.AddComponent<DataLogger>();
            _instance.Init();
        }

        return _instance;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Init()
    {
        string date = System.DateTime.Now.ToString();
        date = date.Replace("/", "_");
        date = date.Replace(" ", "_");
        date = date.Replace(":", "_");

        dataPathTrialSummary = Application.dataPath + "/Trials/" + "Trial" + date + ".csv";
        Debug.Log("DataPath: " + dataPathTrialSummary);

        if (!Directory.Exists(Application.dataPath + "/" + "Trials"))
        {
            Directory.CreateDirectory(Application.dataPath + "/" + "Trials");
        }

        _logfileTrialSummary = new StringBuilder();
        _logfileTrialSummary.AppendLine("Time;Distance;Trial;NameOfExercise");
    }

    /// <summary>
    /// 
    /// </summary>
    public void RecordTrial(float recordedTime, int trial, string NameOfExercise)
    {
        _logfileTrialSummary.AppendLine(Time.timeSinceLevelLoad + ";" + recordedTime + ";" + trial + ";" + NameOfExercise);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!File.Exists(dataPathTrialSummary))
            {
                File.WriteAllText(dataPathTrialSummary, _logfileTrialSummary.ToString());
            }
        }
    }
}
