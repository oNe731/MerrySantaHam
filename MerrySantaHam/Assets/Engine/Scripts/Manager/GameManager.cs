using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_hamsterObject;

    private static GameManager m_instance = null;
    private UIManager m_uIManager = null;

    private bool m_isGame = false;
    private int m_score = 0;
    private Hamster m_hamster;
    private List<House> m_houses = new List<House>();

    public static GameManager Ins => m_instance;
    public UIManager UI => m_uIManager;
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
    public Hamster Player => m_hamster;
    public List<House> Houses
    {
        get => m_houses;
        set => m_houses = value;
    }


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

    private void Start()
    {
        m_uIManager.Start_FadeIn(1f, Color.black);
    }

    public void Start_Game()
    {
        m_isGame = true;

        m_hamsterObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/BGM/AFD0427_InGame");
        Camera.main.GetComponent<AudioSource>().Play();

        UI.Start_FadeIn(1f, Color.black);
    }

    private void Update()
    {
        /*
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
            m_hamster.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Cloud, 1));
        if (Input.GetKeyDown(KeyCode.W))
            m_hamster.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Fish, 1));
        if (Input.GetKeyDown(KeyCode.E))
            m_hamster.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Person, 1));
        if (Input.GetKeyDown(KeyCode.R))
            m_hamster.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Strawberry, 1));
        if (Input.GetKeyDown(KeyCode.T))
            m_hamster.Inventory.Add_Item(new Item(Item.ELEMENT.EM_Tree, 1));
        #endif
        */
    }


    public House Create_Target(int orderID)
    {
        if (m_houses.Count < 3) // 주문서 최대 개수 3개
            return null;

        while (true)
        {
            int index = Random.Range(0, m_houses.Count);
            if (m_houses[index].Registered == false)
            {
                m_houses[index].Reserve_Target(orderID);
                return m_houses[index];
            }
        }
    }

    public void Over_Game()
    {
        if (m_isGame == false)
            return;

        m_isGame = false;

        Player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Create_GameObject("Prefabs/UI/FinishPanel", GameObject.Find("Canvas").transform);

        Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/BGM/AFE0139_GameOver");
        Camera.main.GetComponent<AudioSource>().loop = false;
        Camera.main.GetComponent<AudioSource>().Play();
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
}
