using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;

    [SerializeField] private GameObject m_hamsterObject;

    [SerializeField] private AudioClip m_inGameSr;
    [SerializeField] private AudioClip m_OverSr;

    private bool m_isGame = false;
    private int m_score = 0;
    public bool IsGame
    {
        get => m_isGame;
        set => m_isGame = value;
    }
    public int Score
    {
        get => m_score;
        set => m_score = value;
    }

    private List<House> m_houses = new List<House>();
    public List<House> Houses
    {
        get => m_houses;
        set => m_houses = value;
    }

    private UIManager m_uIManager = null;
    private Hamster m_hamster;

    public static GameManager Ins => m_instance;
    public UIManager UI => m_uIManager;
    public Hamster Player => m_hamster;

    private void Awake()
    {
        if (null == m_instance)
        {
            m_instance = this;
            m_uIManager = gameObject.AddComponent<UIManager>();
            m_hamster = m_hamsterObject.GetComponent<Hamster>();
        }
        else
            Destroy(this.gameObject);
    }

    public void Start_Game()
    {
        m_isGame = true;
        UI.Start_FadeIn(1f, Color.black);

        m_hamsterObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().clip = m_inGameSr;
        Camera.main.GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        
    }

    public GameObject Create_GameObject(string path, Transform transform = null)
    {
        return Instantiate(Resources.Load<GameObject>(path), transform);
    }

    public void Destroy_GameObject(ref GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void Change_Scene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Save_JsonData<T>(string filePath, List<T> saveData)
    {
        var Result = JsonConvert.SerializeObject(saveData);
        File.WriteAllText(filePath, Result);
    }

    public List<T> Load_JsonData<T>(string filePath)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);

        if (jsonAsset != null)
            return JsonConvert.DeserializeObject<List<T>>(jsonAsset.text);
        else
            Debug.LogError($"Failed to load Jsondata : {filePath}");

        return null;
    }

    public void Over_Game()
    {
        if (m_isGame == false)
            return;

        m_isGame = false;
        Player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Create_GameObject("Prefabs/UI/FinishPanel", GameObject.Find("Canvas").transform);

        Camera.main.GetComponent<AudioSource>().loop = false;
        Camera.main.GetComponent<AudioSource>().clip = m_OverSr;
        Camera.main.GetComponent<AudioSource>().Play();
    }

    public House Create_Target(int orderIndex)
    {
        if (m_houses.Count < 3) // 주문서 최대 개수 3개
            return null;

        while(true)
        {
            int index = Random.Range(0, m_houses.Count);

            for (int i = 0; i < m_houses.Count; ++i)
            {
                if (m_houses[i].OrderIndex == orderIndex)
                {
                    m_houses[i].Reset_Home(false);
                    break;
                }
            }

            if(m_houses[index].Registered == false)
            {
                m_houses[index].Registered_Target(orderIndex);
                return m_houses[index];
            }
        }
    }
}
