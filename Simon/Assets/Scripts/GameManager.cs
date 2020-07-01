using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject LightObject;
    public GameObject Cylinder, Pizza, Cube;
    public GameObject Ally;
    public GameObject Door;
    public int[] randArray;
    public Color RColor, YColor, BColor, GColor, PColor, SColor, OColor, WColor, BLColor;
    public int buttonON;
    public bool game_start = false;
    public int CIndex = 0, DoorIndex = 0, ClickIndex = 0;
    public Vector3 Cposition;

    Animator animator;

    void Start()
    {
        LightMemory();
        RandomNum();

        if (SimLevelManager.Level.levelselect == 4)
        {
            StartCoroutine(FirstAnimation());
        }
        else if (SimLevelManager.Level.levelselect == 6)
        {
            DoorOpened();
            StartCoroutine(SecondAnimation());
        }
        else
        {
            DoorIndex = 2;
            DoorOpened();
            StartCoroutine(ThirdAnimation());
        }
    }

    void Update()
    {
        if (game_start)
        {
            if (Input.GetMouseButtonDown(0)) //마우스 눌렀을 때
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    LeanTween.moveLocalY(hit.transform.gameObject, 0.05f, 0.05f).setEase(LeanTweenType.linear); //버튼 아래로
                }
                buttonON = int.Parse(hit.transform.name);
                ButtonLightOn();               
            }
            if (Input.GetMouseButtonUp(0)) //마우스 뗐을 때
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    ClickIndex = int.Parse(hit.transform.name);

                    if (ClickIndex != buttonON) //이거는 클릭했는데 발판 안에 다른 색에서 뗄 때
                    {
                        for (int i = 0; i < SimLevelManager.Level.levelselect; i++)
                        {
                            LeanTween.moveLocalY(LightObject.transform.GetChild(i).gameObject, 0.1f, 0.5f).setEase(LeanTweenType.easeOutBack);
                        }
                        goto Exit;
                    }

                    if (SimLevelManager.Level.levelselect == 9)
                    {
                        LeanTween.moveLocalY(hit.transform.gameObject, 0.2f, 0.5f).setEase(LeanTweenType.easeOutBack); //버튼 위로
                    }
                    else
                        LeanTween.moveLocalY(hit.transform.gameObject, 0.1f, 0.5f).setEase(LeanTweenType.easeOutBack); //버튼 위로

                    ////클릭 데이터 비교
                    if (randArray[CIndex] != ClickIndex)
                    {
                        SimSoundManager.Sound.StageFail();
                        game_start = false;
                        CIndex = 0;
                        FailHeart();
                        StartCoroutine(FailLight());
                    }
                    else if (CIndex == (SimLevelManager.Level.lightLevel - 1))
                    {
                        SimSoundManager.Sound.StageClear();
                        StartCoroutine(Success());
                        SimUIManager.UI.Clear_Count++;
                    }
                    else
                    {
                        SimSoundManager.Sound.ClickSimonButton();
                        CIndex++;
                    }
                Exit:
                    ButtonLightOff();
                }
                else
                {
                    for (int i = 0; i < SimLevelManager.Level.levelselect; i++)
                    {
                        LeanTween.moveLocalY(LightObject.transform.GetChild(i).gameObject, 0.1f, 0.5f).setEase(LeanTweenType.easeOutBack);
                    }
                    ButtonLightOff();
                }
            }
        }
    }

    public void RandomNum() //랜덤숫자
    {
        randArray = new int[SimLevelManager.Level.lightLevel];

        for (int i = 0; i < SimLevelManager.Level.lightLevel; i++)
        {
            randArray[i] = Random.Range(0, SimLevelManager.Level.levelselect);
        }
    }

    IEnumerator ChangeColor() //라이트 순서대로 빛나게
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < SimLevelManager.Level.lightLevel; i++)
        {
            if (SimLevelManager.Level.levelselect == 4)
                LightObject = Cylinder;
            else if (SimLevelManager.Level.levelselect == 6)
                LightObject = Pizza;
            else
                LightObject = Cube;

            LightOn(randArray[i]);
            SimSoundManager.Sound.ButtonOn();
            //노출 시간
            if (SimLevelManager.Level.timeLevel == 1)
            {
                Debug.Log("1");
                yield return new WaitForSeconds(1f);
            }
            else if (SimLevelManager.Level.timeLevel == 2)
            {
                Debug.Log("2");
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.Log("3");
                yield return new WaitForSeconds(3f);
            }

            LightOff(randArray[i]);
            yield return new WaitForSeconds(0.5f);
        }
        game_start = true;
    }

    public void LightMemory()
    {
        if (SimLevelManager.Level.levelselect == 4)
            LightObject = Cylinder;
        else if (SimLevelManager.Level.levelselect == 6)
            LightObject = Pizza;
        else
            LightObject = Cube;

        //오브젝트 기본 색상 정보 저장
        RColor = LightObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
        YColor = LightObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
        GColor = LightObject.transform.GetChild(2).GetComponent<MeshRenderer>().material.color;
        BColor = LightObject.transform.GetChild(3).GetComponent<MeshRenderer>().material.color;

        if (LightObject == Pizza || LightObject == Cube)
        {
            PColor = LightObject.transform.GetChild(4).GetComponent<MeshRenderer>().material.color;
            SColor = LightObject.transform.GetChild(5).GetComponent<MeshRenderer>().material.color;
        }

        if (LightObject == Cube)
        {
            OColor = LightObject.transform.GetChild(6).GetComponent<MeshRenderer>().material.color;
            WColor = LightObject.transform.GetChild(7).GetComponent<MeshRenderer>().material.color;
            BLColor = LightObject.transform.GetChild(8).GetComponent<MeshRenderer>().material.color;
        }
    }

    public void LightOn(int index)
    {
        //Light On
        switch (index)
        {
            case 0: //빨강
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f);
                break;
            case 1: //노랑
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 0f);
                break;
            case 2: //초록
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
                break;
            case 3: //파랑
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 1f);
                break;
            case 4: //핑크
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 1f);
                break;
            case 5: //하늘
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 1f);
                break;
            case 6: //주황
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 103 / 255f, 0f);
                break;
            case 7: //연핑
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 201 / 255f, 201 / 255f);
                break;
            case 8: //검정
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 115 / 255f);
                break;
        }
    }

    public void LightOff(int index)
    {
        //Light Off
        switch (index)
        {
            case 0: //빨강
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = RColor;
                break;
            case 1: //노랑
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = YColor;
                break;
            case 2: //초록
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = GColor;
                break;
            case 3: //파랑
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = BColor;
                break;
            case 4: //핑크
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = PColor;
                break;
            case 5: //하늘
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = SColor;
                break;
            case 6: //주황
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = OColor;
                break;
            case 7: //흰색
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = WColor;
                break;
            case 8: //검정
                LightObject.transform.GetChild(index).GetComponent<MeshRenderer>().material.color = BLColor;
                break;
        }
    }

    public void ButtonLightOn()
    {
        switch (buttonON)
        {
            case 0: //빨강
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f);
                break;
            case 1: //노랑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 0f);
                break;
            case 2: //초록
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
                break;
            case 3: //파랑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 1f);
                break;
            case 4: //핑크
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 1f);
                break;
            case 5: //하늘
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 1f);
                break;
            case 6: //주황
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 103 / 255f, 0f);
                break;
            case 7: //연핑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 201 / 255f, 201 / 255f);
                break;
            case 8: //검정
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 115 / 255f);
                break;
        }
    }

    public void ButtonLightOff()
    {
        //Light Off
        switch (buttonON)
        {
            case 0: //빨강
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = RColor;
                break;
            case 1: //노랑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = YColor;
                break;
            case 2: //초록
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = GColor;
                break;
            case 3: //파랑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = BColor;
                break;
            case 4: //핑크
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = PColor;
                break;
            case 5: //하늘
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = SColor;
                break;
            case 6: //주황
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = OColor;
                break;
            case 7: //연핑
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = WColor;
                break;
            case 8: //검정
                LightObject.transform.GetChild(buttonON).GetComponent<MeshRenderer>().material.color = BLColor;
                break;
        }
    }

    IEnumerator FailLight()
    {
        int count = 0;
        while (count < 3)
        {
            //틀렸을 때 버튼 효과 어둡게
            LightObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f);
            LightObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 0f);
            LightObject.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);
            LightObject.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 1f);

            if (LightObject == Pizza || LightObject == Cube)
            {
                LightObject.transform.GetChild(4).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 1f);
                LightObject.transform.GetChild(5).GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 1f);
            }
            if (LightObject == Cube)
            {
                LightObject.transform.GetChild(6).GetComponent<MeshRenderer>().material.color = new Color(1f, 103 / 255f, 0f);
                LightObject.transform.GetChild(7).GetComponent<MeshRenderer>().material.color = new Color(1f, 201 / 255f, 201 / 255f);
                LightObject.transform.GetChild(8).GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 115 / 255f);
            }
            yield return new WaitForSeconds(0.2f);

            LightObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = RColor;
            LightObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = YColor;
            LightObject.transform.GetChild(2).GetComponent<MeshRenderer>().material.color = GColor;
            LightObject.transform.GetChild(3).GetComponent<MeshRenderer>().material.color = BColor;

            if (LightObject == Pizza || LightObject == Cube)
            {
                LightObject.transform.GetChild(4).GetComponent<MeshRenderer>().material.color = PColor;
                LightObject.transform.GetChild(5).GetComponent<MeshRenderer>().material.color = SColor;
            }
            if (LightObject == Cube)
            {
                LightObject.transform.GetChild(6).GetComponent<MeshRenderer>().material.color = OColor;
                LightObject.transform.GetChild(7).GetComponent<MeshRenderer>().material.color = WColor;
                LightObject.transform.GetChild(8).GetComponent<MeshRenderer>().material.color = BLColor;
            }
            yield return new WaitForSeconds(0.2f);
            Debug.Log("Fail");
            count++;
        }
        RandomNum();
        StartCoroutine(ChangeColor());
    }


    IEnumerator FirstAnimation() //첫 번째 카메라,캐릭터 이동 (1-) 
    {
        Cposition = new Vector3(0f, 107f, -18f);

        //달려가기(1-1)
        if (SimLevelManager.Level.timeLevel == 3 && SimLevelManager.Level.lightLevel == 4)
        {
            yield return new WaitForSeconds(2f);
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", true);
            LeanTween.moveLocalX(Ally.transform.gameObject, -110f, 3f).setEase(LeanTweenType.linear);
            SimSoundManager.Sound.WalkStart = true;
            yield return new WaitForSeconds(3f);
            SimSoundManager.Sound.WalkStart = false;

            // 멈춤
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
        else //나머지는 발판 앞에서 시작
        {
            Ally.transform.localPosition = new Vector3(-110f, 0f, 0f);
            yield return new WaitForSeconds(2f);
        }

        Ally.transform.Find("Main Camera").transform.LeanMoveLocal(Cposition, 1f);
        Ally.transform.Find("Main Camera").transform.LeanRotateX(55f, 1f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeColor());
    }

    IEnumerator SecondAnimation() //두 번째 카메라,캐릭터 이동 (2-) 
    {
        Cposition = new Vector3(0f, 107f, -10f);

        if (SimLevelManager.Level.timeLevel == 3 && SimLevelManager.Level.lightLevel == 4)
        {
            Ally.transform.localPosition = new Vector3(-230f, 0f, 0f);
            yield return new WaitForSeconds(2f);
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", true);
            LeanTween.moveLocalX(Ally.transform.gameObject, -320f, 3f).setEase(LeanTweenType.linear);
            SimSoundManager.Sound.WalkStart = true;
            yield return new WaitForSeconds(3f);
            SimSoundManager.Sound.WalkStart = false;

            // 멈춤
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
        else
        {
            Ally.transform.localPosition = new Vector3(-320f, 0f, 0f);
            yield return new WaitForSeconds(2f);
        }
        Ally.transform.Find("Main Camera").transform.LeanMoveLocal(Cposition, 1f);
        Ally.transform.Find("Main Camera").transform.LeanRotateX(55f, 1f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeColor());
    }

    IEnumerator ThirdAnimation() //세 번째 카메라,캐릭터 이동 (3-) 
    {
        Cposition = new Vector3(0f, 107f, -10f);

        if (SimLevelManager.Level.timeLevel == 3 && SimLevelManager.Level.lightLevel == 4)
        {
            Ally.transform.localPosition = new Vector3(-445f, 0f, 0f);


            //달려가기
            yield return new WaitForSeconds(2f);
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", true);
            LeanTween.moveLocalX(Ally.transform.gameObject, -540f, 3f).setEase(LeanTweenType.linear);
            SimSoundManager.Sound.WalkStart = true;
            yield return new WaitForSeconds(3f);
            SimSoundManager.Sound.WalkStart = false;

            // 멈추고 카메라 이동
            Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
        }
        else
        {
            Ally.transform.localPosition = new Vector3(-540f, 0f, 0f);
            yield return new WaitForSeconds(2f);
        }

        Ally.transform.Find("Main Camera").transform.LeanMoveLocal(Cposition, 1f);
        Ally.transform.Find("Main Camera").transform.LeanRotateX(55f, 1f);
        yield return new WaitForSeconds(2f);
        StartCoroutine(ChangeColor());
    }

    IEnumerator Success() //맞췄을 시
    {
        Debug.Log("finish");
        game_start = false;
        Cposition = new Vector3(0f, 57.5f, -108f);

        //카메라 다시 캐릭터 보이게
        Ally.transform.Find("Main Camera").transform.LeanMoveLocal(Cposition, 0.5f);
        Ally.transform.Find("Main Camera").transform.LeanRotateX(17.0f, 0.5f);

        yield return new WaitForSeconds(1f);

        //오예 점프
        Ally.gameObject.GetComponent<Animator>().SetBool("isSuccess", true);

        //발판 쾅
        LeanTween.moveLocalY(LightObject.transform.gameObject, -0.15f, 0.2f).setEase(LeanTweenType.linear);

        //-9일 때만 실행
        if (SimLevelManager.Level.timeLevel == 1 && SimLevelManager.Level.lightLevel == 8)
        {
            //문 Open
            Debug.Log("open");

            if (SimLevelManager.Level.levelselect == 4) //열리는 문 index 설정
                DoorIndex = 0;
            else if (SimLevelManager.Level.levelselect == 6)
                DoorIndex = 2;
            else
                DoorIndex = 4;

            LeanTween.rotateY(Door.transform.GetChild(DoorIndex).gameObject, -90f, 1.5f).setEase(LeanTweenType.easeInBack);
            LeanTween.rotateY(Door.transform.GetChild(DoorIndex + 1).gameObject, 90f, 1.5f).setEase(LeanTweenType.easeInBack);
        }

        yield return new WaitForSeconds(3f);

        Ally.gameObject.GetComponent<Animator>().SetBool("isSuccess", false);
        Ally.gameObject.GetComponent<Animator>().SetBool("isRunning", true);

        if (SimLevelManager.Level.timeLevel == 1 && SimLevelManager.Level.lightLevel == 8)
        {
            if (SimLevelManager.Level.levelselect == 4) //first stage clear
                LeanTween.moveLocalX(Ally.transform.gameObject, -230f, 2f).setEase(LeanTweenType.linear);
            else if (SimLevelManager.Level.levelselect == 6) //second stage clear
                LeanTween.moveLocalX(Ally.transform.gameObject, -445f, 2f).setEase(LeanTweenType.linear);
            else //third stage clear
                LeanTween.moveLocalX(Ally.transform.gameObject, -670f, 2f).setEase(LeanTweenType.linear);

            SimSoundManager.Sound.WalkStart = true;
            yield return new WaitForSeconds(2f);
            SimSoundManager.Sound.WalkStart = false;
        }

        SimUIManager.UI.GameClear();
    }

    public void DoorOpened() //열려있는 문 설정
    {
        Door.transform.GetChild(DoorIndex).rotation = Quaternion.Euler(0, -90f, 0);
        Door.transform.GetChild(DoorIndex + 1).rotation = Quaternion.Euler(0, 90f, 0);
    }

    public void FailHeart()
    {
        SimUIManager.UI.LifeObject[SimUIManager.UI.life - 1].SetActive(false);
        SimUIManager.UI.life--;
        SimUIManager.UI.Fail_Count = 3 - SimUIManager.UI.life;

        if (SimUIManager.UI.life == 0)
            SimUIManager.UI.GameOver();
    }
}
