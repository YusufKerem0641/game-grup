using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameControl : MonoBehaviour
{
    private GameObject gameObject; // haritadaki son adam� tutmak ve i�lemler yapmak i�in
    public GameObject enemey;
    private List<GameObject> players = new List<GameObject>(); // T�m playlerin listesi
    private GameObject[] playersEnemy; // haritadaki d��man player listesi
    private GameObject[] playersFriend; // haritadaki dost player listesi
    private int time; // oyun zaman� y�netmek i�in
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

    int playerLifeControl() // player ya��yorsa bir sonraki adama ge�mesini sa�lar
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

    bool playerTimeControl(GameObject gameObject) // palyerin zaman kontrol�n� yapar
    {
        if (time % gameObject.GetComponent<PlayerData>().Speed == 0)
            return true;
        return false;
    }

    GameObject playerTurnControl() // hangi playerde ise o playeri geri d�nd�r�r
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

        // Yeni bir UI Buton olu�tural�m ve Canvas alt�nda konumland�ral�m.
        GameObject buttonObj = new GameObject("MyButton");
        buttonObj.transform.SetParent(canvas.transform, false);

        // Buton nesnesine UI Buton bile�eni ekleyelim.
        UnityEngine.UI.Button buttonComponent = buttonObj.AddComponent<UnityEngine.UI.Button>();

        Transform rectTransform = buttonComponent.GetComponent<Transform>();

        // Butonun yeni pozisyonunu ayarl�yoruz
        rectTransform.position = new Vector2((100f*butonSirasi)+300, 350f);

        // Butonun metnini ayarlayal�m.
        Text buttonText = buttonObj.AddComponent<Text>();
        buttonText.text = "D�sman" + butonYazisi;
        buttonText.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        buttonText.alignment = TextAnchor.MiddleCenter;

        // Butonun t�klama olay�na fonksiyon ekleyelim (iste�e ba�l�).
        buttonComponent.onClick.AddListener(() => buttonEnemySelect(butonSirasi));
    }

}
