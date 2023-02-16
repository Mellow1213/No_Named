using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 유일한 인스턴스
    private static GameManager instance;

    // 싱글톤 인스턴스에 접근할 수 있는 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성
            if (instance == null)
            {
                GameObject singletonObject = new GameObject();
                instance = singletonObject.AddComponent<GameManager>();
                singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    // 초기화
    private void Awake()
    {
        // 다른 인스턴스가 이미 존재하면 삭제
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // 싱글톤 기능 구현
    public void DoSomething()
    {
        // ...
    }
}
