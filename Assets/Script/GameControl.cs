using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameControl : MonoBehaviour
{
    private GameObject gameObject; // haritadaki son adamý tutmak ve iþlemler yapmak için
    public GameObject enemey;
    private List<GameObject> players = new List<GameObject>(); // Tüm playlerin listesi
    private GameObject[] playersEnemy; // haritadaki düþman player listesi
    private GameObject[] playersFriend; // haritadaki dost player listesi
    private int time; // oyun zamaný yönetmek için
    public int durum;
    private int turn;

    private void Start()
    {
        durum = 0;
        time = 1;
        turn = 0;
    }

    void Update()
    {
        if (durum == 0)
            playerControl();
        else if (durum == 1)
            selectPlayer();
        else if (durum == 3)
            canvaDestroy();

    }

    void canvaDestroy()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
        durum = 0;
    }

    void selectPlayer()
    {
        int i = 0;
        while ( i < playersEnemy.Length)
        {
            butonCreate(i.ToString(), i.ToString(), i);
            i++;
        }
        durum = 2;
    }

    int playerLifeControl() // player yaþýyorsa bir sonraki adama geçmesini saðlar
    {
        foreach (GameObject player in playersFriend)
        {
            if (gameObject == player)
                return 1;
        }
        foreach (GameObject player in playersEnemy)
        {
            if (gameObject == player)
                return 1;
        }
        return 0;
    }

    bool playerTimeControl(GameObject gameObject) // palyerin zaman kontrolünü yapar
    {
        if (time % gameObject.GetComponent<PlayerData>().Speed == 0)
            return true;
        return false;
    }

    GameObject playerTurnControl() // hangi playerde ise o playeri geri döndürür
    {
        turn += playerLifeControl();
        if (gameObject != null)
            Debug.Log("name : " + gameObject.name + " turn : " + time);

        while (turn < players.Count)
        {
            if (playerTimeControl(players[turn]))
                return players[turn];
            turn++;
        }
        turn = 0;
        time += 1;
        return null;
    }

    void playerControl() 
    {
        players.Clear();
        playersFriend = GameObject.FindGameObjectsWithTag("friend");
        playersEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        players.AddRange(playersFriend);
        players.AddRange(playersEnemy);
        if (gameObject != null)
            Debug.Log("name : " + gameObject.name + " time :" + time);

        gameObject = playerTurnControl();

        if (gameObject != null)
            durum = 1;

    }
    void fight(GameObject gameObject, GameObject enemyGameObject)
    {
        gameObject.GetComponent<PlayerControl>().playerFight(enemyGameObject);
    }

    void buttonEnemySelect(int sirasi)
    {
        fight(gameObject, playersEnemy[sirasi]);
        durum = 3;
    }

    void butonCreate(string butonIsmi, string butonYazisi, int butonSirasi)
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        // Yeni bir UI Buton oluþturalým ve Canvas altýnda konumlandýralým.
        GameObject buttonObj = new GameObject("MyButton");
        buttonObj.transform.SetParent(canvas.transform, false);

        // Buton nesnesine UI Buton bileþeni ekleyelim.
        UnityEngine.UI.Button buttonComponent = buttonObj.AddComponent<UnityEngine.UI.Button>();

        Transform rectTransform = buttonComponent.GetComponent<Transform>();

        // Butonun yeni pozisyonunu ayarlýyoruz
        rectTransform.position = new Vector2((100f*butonSirasi)+300, 350f);

        // Butonun metnini ayarlayalým.
        Text buttonText = buttonObj.AddComponent<Text>();
        buttonText.text = "Düsman" + butonYazisi;
        buttonText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        buttonText.alignment = TextAnchor.MiddleCenter;

        // Butonun týklama olayýna fonksiyon ekleyelim (isteðe baðlý).
        buttonComponent.onClick.AddListener(() => buttonEnemySelect(butonSirasi));
    }

}
