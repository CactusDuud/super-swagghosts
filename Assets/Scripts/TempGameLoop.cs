using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class TempGameLoop : MonoBehaviour
{
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<HunterHealth> hunter_healths = new List<HunterHealth>();
    [SerializeField] private GhostHealth ghost_health;

    public static string winner;

    public int num_hunters;
    public bool is_set_up = false;

    // Start is called before the first frame update
    void Start()
    {
        //! Does this still work?
        StartCoroutine(Setup());
    }

    // Update is called once per frame
    void Update()
    {
        if (is_set_up == false) return;

        winner = CheckWinner();
        if (winner == "ghost")
        {
            Debug.Log($"{name}: The ghost has won!");

            // TODO: Replace with game over screen
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.LoadLevel("EndScreen");
            SceneManager.LoadScene("EndScreen");

        }
        else if (winner == "humans")
        {
            Debug.Log($"{name}: The hunters have won!");

            // TODO: Replace with game over screen
            PhotonNetwork.LeaveRoom();
            //PhotonNetwork.LoadLevel("EndScreen");
            SceneManager.LoadScene("EndScreen");
        }
    }


    // We probably could've optimised this better with an event-based system over Photon but
    //  none of us know how to get that to work lol
    private string CheckWinner()
    {
        // If ghost is dead then humans win
        if (ghost_health == null || ghost_health.is_down) return "humans";

        // Check if hunters are dead
        else
        {
            int down_count = 0;

            // Iterate through all human scripts to check if they are down
            for (int h = 0; h < num_hunters; h++)
            {
                if (hunter_healths[h] == null || hunter_healths[h].is_down == true) down_count++;
            }

            // if total number of dead pp == total num of humans, then ghost wins
            if (down_count >= num_hunters) return "ghost";
        }

        return "none";
    }


    IEnumerator Setup()
    {
        yield return new WaitForSeconds(5f);

        Debug.Log($"{name}: Counting players...");
        PhotonView[] photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
    
        // Go through each photonView/player in game and grab the GameObject
        foreach (PhotonView view in photonViews)
        {
            // Skip replicated players with no owner
            if (view.Owner != null)
            {
                GameObject playerPrefabObject = view.gameObject;
                players.Add(playerPrefabObject);
            }
        }

        // Goes through each GameObject found and grabs the health script
        foreach (GameObject player in players)
        {
            if(player.tag == "Player")
            {
                hunter_healths.Add(player.GetComponent<HunterHealth>());
            }
            // Assumes there is only one ghost
            else if (player.tag == "Ghost")
            {
                ghost_health = player.GetComponent<GhostHealth>();
            }
        }

        num_hunters = hunter_healths.Count;
        is_set_up = true;
        yield return new WaitForSeconds(0.1f);
    }
}
