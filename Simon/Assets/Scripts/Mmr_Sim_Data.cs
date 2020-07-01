using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mmr_Sim_Data
{
    public static List<Mmr_Sim> mmr_Sim = new List<Mmr_Sim>();

    public static string getPath()
    {
#if UNITY_EDITOR 
        return Application.dataPath + "/CSV/" + "Saved_data_Mmr_Sim.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"/Saved_data_Mmr_Sim.csv";
#elif UNITY_STANDALONE_WIN
        return Application.persistentDataPath+"/"+"Saved_data_Mmr_Sim.csv";
#endif
    }
}

public class Mmr_Sim
{
    public string Level { get; set; }
    public string PlayTime { get; set; }
    public string CorrectCnt { get; set; }
    public string UncorrectCnt { get; set; }
    public string StarCnt { get; set; }
    public string Lock { get; set; }
}