using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 임시 스크립트(CSV 관리) -> 나중에 다른 콘텐츠랑 통합할 때 삭제
 */

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _uiManager;
    public static UIManager UI
    {
        get { return _uiManager; }
    }
    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CreateMmrSimFile();
    }

    public void SceneChange() //start -> level 선택 씬으로
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnApplicationQuit() //게임 종료 시, CSV 쓰기
    {
        UpdateMmrSimFile();
    }

    void CreateMmrSimFile() //CSV 파일 생성
    {
        if (!System.IO.File.Exists(Mmr_Sim_Data.getPath()) || new FileInfo(Mmr_Sim_Data.getPath()).Length == 0)
        {
            string filePath = Mmr_Sim_Data.getPath();
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            for (int i = 0; i < 27; i++)
            {
                Mmr_Sim temp = new Mmr_Sim
                {
                    Level = (i + 1) + "",
                    PlayTime = "00:00",
                    CorrectCnt = "0",
                    UncorrectCnt = "0",
                    StarCnt = "0"
                };
                if (i == 0)
                {
                    temp.Lock = "false";
                }
                else
                {
                    temp.Lock = "true";
                }
                Mmr_Sim_Data.mmr_Sim.Add(temp);
                string str = Mmr_Sim_Data.mmr_Sim[i].Level + "," + Mmr_Sim_Data.mmr_Sim[i].PlayTime + "," + Mmr_Sim_Data.mmr_Sim[i].CorrectCnt + ","
                    + Mmr_Sim_Data.mmr_Sim[i].UncorrectCnt + "," + Mmr_Sim_Data.mmr_Sim[i].StarCnt + "," + Mmr_Sim_Data.mmr_Sim[i].Lock;

                if (i == 0)
                {
                    outStream.WriteLine("Level,PlayTime,CorrectCnt,UncorrectCnt,StarCnt,Lock");
                }
                outStream.WriteLine(str);
            }
            outStream.Close();
        }
        else
        {
            List<Dictionary<string, object>> data = CSVReader.Read(@Mmr_Sim_Data.getPath());

            for (var i = 0; i < data.Count; i++)
            {
                Mmr_Sim mt = new Mmr_Sim();
                mt.Level = data[i]["Level"].ToString();
                mt.PlayTime = data[i]["PlayTime"].ToString();
                mt.CorrectCnt = data[i]["CorrectCnt"].ToString();
                mt.UncorrectCnt = data[i]["UncorrectCnt"].ToString();
                mt.StarCnt = data[i]["StarCnt"].ToString();
                mt.Lock = data[i]["Lock"].ToString();

                Mmr_Sim_Data.mmr_Sim.Add(mt);
            }
        }
    }

    void UpdateMmrSimFile() //CSV 데이터 덮어쓰기
    {
        string filePath = Mmr_Sim_Data.getPath();
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine("Level,PlayTime,CorrectCnt,UncorrectCnt,StarCnt,Lock");
        for (int i = 0; i < 27; i++)
        {
            string str = Mmr_Sim_Data.mmr_Sim[i].Level + "," + Mmr_Sim_Data.mmr_Sim[i].PlayTime + "," + Mmr_Sim_Data.mmr_Sim[i].CorrectCnt + ","
                + Mmr_Sim_Data.mmr_Sim[i].UncorrectCnt + "," + Mmr_Sim_Data.mmr_Sim[i].StarCnt + "," + Mmr_Sim_Data.mmr_Sim[i].Lock;
            outStream.WriteLine(str);
        }
        outStream.Close();
    }

}
