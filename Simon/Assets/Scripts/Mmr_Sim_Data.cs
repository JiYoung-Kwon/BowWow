using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * SIMON3D 데이터 (CSV 항목)
 */

public class Mmr_Sim_Data
{
    public static List<Mmr_Sim> mmr_Sim = new List<Mmr_Sim>();

    public static string getPath() //저장 경로
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

public class Mmr_Sim //csv 항목들
{
    public string Level { get; set; } //레벨
    public string PlayTime { get; set; } //플레이시간
    public string CorrectCnt { get; set; } //맞은 개수
    public string UncorrectCnt { get; set; } //틀린 개수
    public string StarCnt { get; set; } //별 얻은 개수
    public string Lock { get; set; } //잠김 여부
}