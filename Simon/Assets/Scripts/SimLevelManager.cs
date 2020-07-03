using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * LevelSelect 씬 관련 스크립트
 */

public class SimLevelManager : MonoBehaviour
{
    #region 변수
    public static int Selected_Level = 0;

    public GameObject LevelPanel;
    public GameObject[] Position;

    public GameObject LeftArrow;
    public GameObject RightArrow;

    public Transform[] DotPosition;
    public GameObject DotIndicator;

    public int levelselect;
    public int timeLevel;
    public int lightLevel;

    public string[] LevelObjectName = new string[27];
    #endregion

    #region Singleton
    private static SimLevelManager level;
    public static SimLevelManager Level
    {
        get { return level; }
    }

    private void Awake()
    {
        level = GetComponent<SimLevelManager>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mmr_Sim_Data.mmr_Sim.Count; i++) //레벨 
        {
            LevelObjectName[i] = "Level " + Mmr_Sim_Data.mmr_Sim[i].Level;
        }
        for (int i = 0; i < Mmr_Sim_Data.mmr_Sim.Count; i++)
        {
            GameObject clearstar = GameObject.Find(LevelObjectName[i]).transform.Find("ClearStar").gameObject;
            GameObject obj_lock = GameObject. Find(LevelObjectName[i]).transform.Find("Lock").gameObject;

            //깬 별 개수에 맞게 Star 오브젝트 활성화
            switch (int.Parse(Mmr_Sim_Data.mmr_Sim[i].StarCnt))
            {
                case 1:
                    clearstar.transform.Find("Star0").gameObject.SetActive(true);
                    break;
                case 2:
                    clearstar.transform.Find("Star0").gameObject.SetActive(true);
                    clearstar.transform.Find("Star1").gameObject.SetActive(true);
                    break;
                case 3:
                    clearstar.transform.Find("Star0").gameObject.SetActive(true);
                    clearstar.transform.Find("Star1").gameObject.SetActive(true);
                    clearstar.transform.Find("Star2").gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            
            if (Mmr_Sim_Data.mmr_Sim[i].Lock.Equals("true")) //아직 안풀린 레벨 잠금
            {
                obj_lock.SetActive(true);
                GameObject.Find(LevelObjectName[i]).GetComponent<Button>().interactable = false;
            }
            else //잠금 해제
            {
                obj_lock.SetActive(false);
                GameObject.Find(LevelObjectName[i]).GetComponent<Button>().interactable = true;
            }
        }
    }

    public void NextPage() //다음 페이지
    {
        // 페이지 위치 옮기고 왼화살표 키고 포지션상태 변경
        LeftArrow.SetActive(true);
        if (DotIndicator.transform.localPosition.x == -40)
        {
            LeanTween.move(LevelPanel, Position[1].transform.position, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.move(DotIndicator, DotPosition[1].position, 0.5f).setEase(LeanTweenType.linear);
        }
        else
        {
            RightArrow.SetActive(false);
            LeanTween.move(LevelPanel, Position[2].transform.position, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.move(DotIndicator, DotPosition[2].position, 0.5f).setEase(LeanTweenType.linear);
        }
    }

    public void BeforePage() //이전 페이지
    {
        // 페이지 위치 옮기고 오른화살표 끄고 포지션상태 변경
        RightArrow.SetActive(true);
        if (DotIndicator.transform.localPosition.x == 0)
        {
            LeftArrow.SetActive(false);
            LeanTween.move(LevelPanel, Position[0].transform.position, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.move(DotIndicator, DotPosition[0].position, 0.5f).setEase(LeanTweenType.linear);
        }
        else
        {
            LeanTween.move(LevelPanel, Position[1].transform.position, 0.5f).setEase(LeanTweenType.linear);
            LeanTween.move(DotIndicator, DotPosition[1].position, 0.5f).setEase(LeanTweenType.linear);
        }
    }

    //각 스테이지에 time/light/level 값을 넣어놓았음 -> 그것에 따라 stage 난이도 달라짐
    public void TimeSelect(int timeIndex) 
    {
        switch (timeIndex)
        {
            case 3:
                timeLevel = 3;
                break;
            case 2:
                timeLevel = 2;
                break;
            case 1:
                timeLevel = 1;
                break;
        }
    }

    public void LightSelect(int lightIndex)
    {
        switch (lightIndex)
        {
            case 4:
                lightLevel = 4;
                break;
            case 6:
                lightLevel = 6;
                break;
            case 8:
                lightLevel = 8;
                break;
        }
    }

    public void LevelSelect(int levelIndex)
    {
        switch (levelIndex)
        {
            case 4:
                levelselect = 4;
                break;
            case 6:
                levelselect = 6;
                break;
            case 9:
                levelselect = 9;
                break;
        }
        SceneManager.LoadScene("SampleScene"); //게임 진행 화면으로
    }

    public void SelectLevel(int level) //선택한 stage 정보 
    {
        Selected_Level = level;
    }

    public void Back() //게임 시작 화면으로
    {
        SceneManager.LoadScene("GameStart");
    }
}
