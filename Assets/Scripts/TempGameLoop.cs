using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TempGameLoop : MonoBehaviour
{
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<PlayerHealth> p_health_scripts = new List<PlayerHealth>();
    [SerializeField] private GhostHealth g_health_script;
    public int total_num_humans;
    public bool has_setuped;

    // Start is called before the first frame update
    void Start()
    {
        has_setuped = false;
        //PhotonNetwork.countOfPlayers

        // Debug.Log("total human scripts" + p_health_scripts.Count);

        // go through each player and grab health script
        // foreach (var player in PhotonNetwork.PlayerList)
        // {
        //     Debug.Log(player.GetType());
        //     players.Add(player.GameObject);
        //     //Debug.Log(players.Length);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if(has_setuped == false)
        {
            has_setuped = true;
            StartCoroutine(SpawnEnemy());
            // if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CountOfPlayers) 
            // {
            //     SetUp();
            //     has_setuped = true;
            //     Debug.Log("checking #players");
            // }
        }
        else
        {
            string winner = "";
            winner = CheckWinner();
            if(winner == "None")
            {Debug.Log("No Winner rn");}
            else if(winner == "ghost")
            {
                Debug.Log("Winner is ghost");
                // PhotonNetwork.LeaveRoom();
                // PhotonNetwork.LoadLevel("MainMenu");
                Application.Quit();
            }
            else if(winner == "humans")
            {
                Debug.Log("Winners is human");
                Application.Quit();
                // PhotonNetwork.LeaveRoom();
                // PhotonNetwork.LoadLevel("MainMenu");
            }
            
        }
    }


    private string CheckWinner()
    {
        string winner = "None";
        int count_down = 0;

        // if ghost is dead then humans win
        if(g_health_script.is_down == true) {winner = "humans";}
        else
        {   
            // goes through all human scripts to check is_dead var
            for(int h = 0; h < total_num_humans; h++)
            {
                if(p_health_scripts[h].is_down == true) {count_down++;}
            }
            // if total number of dead pp == total num of humans, then ghost wins
            if(count_down == total_num_humans) {winner = "ghost";}
        }

        return winner;
    }

    void SetUp()
    {
        var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
    
        // go through each photonView/players in game and grabs the gameobject
        foreach (var view in photonViews)
        {
            var player = view.Owner;
            //Objects in the scene don't have an owner, its means view.owner will be null
            if(player!=null){
                var playerPrefabObject = view.gameObject;
                players.Add(playerPrefabObject);
            }
        }

        // Debug.Log("total players: " + players.Count);

        //goes through each player prefab and grabs the health script
        foreach (var player in players)
        {
            if(player.tag == "Player")
            {
                p_health_scripts.Add(player.GetComponent<PlayerHealth>());
            }
            else if (player.tag == "Ghost")
            {g_health_script = player.GetComponent<GhostHealth>();} //assumming there is only 1 ghost
        }

        total_num_humans = p_health_scripts.Count;
    }

    // foreach (var player in PhotonNetwork.PlayerList) // 2 Players Room
    // {

    // }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5f);
        SetUp();
        // has_setuped = true;
        Debug.Log("checking #players");
        yield return new WaitForSeconds(0.1f);
    }
}
