using System;
using UnityEngine;
public class Example : MonoBehaviour
{
    public static readonly string OnGameStartEvent = "OnGameStart";
    public static readonly string OnPlayerScoredEvent = "OnPlayerScored";
    public static readonly string OnUpdatePlayerDataEvent = "OnUpdatePlayerData";

    // Event registration methods
    private void OnEnable()
    {
        // Registering events
        EventManager.Register(OnGameStartEvent, new Action(OnGameStart));
        EventManager.Register(OnPlayerScoredEvent, new Action<int>(OnPlayerScored));
        EventManager.Register(OnUpdatePlayerDataEvent, new Action<Player,int>(OnUpdatePlayerData));
    }

    // Event unregistration methods
    private void OnDisable()
    {
        // Unregistering events
        EventManager.Unregister(OnGameStartEvent, new Action(OnGameStart));
        EventManager.Unregister(OnPlayerScoredEvent, new Action<int>(OnPlayerScored));
        EventManager.Unregister(OnUpdatePlayerDataEvent, new Action<Player,int>(OnUpdatePlayerData));
    }

    // Event handler for "OnGameStart"
    private void OnGameStart()
    {
        Debug.Log("The game has started!");
    }

    // Event handler for "OnPlayerScored"
    private void OnPlayerScored(int score)
    {
        Debug.Log($"Player scored {score} points!");
    }

    //Player Class
    public class Player
    {
        public int id;
        public string name;
        public int score;
    }

    // Event handler for "OnUpdatePlayerData"
    private void OnUpdatePlayerData(Player player,int score)
    {
        Debug.Log($"Player id {player.id} Player name is {player.name}");
        player.score += score;     
    }

    // Example of triggering events
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Triggering "OnGameStart" event
            EventManager.Execute(OnGameStartEvent);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // Triggering "OnPlayerScored" event with a score of 10
            EventManager.Execute(OnPlayerScoredEvent, 10);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            // Triggering "OnUpdatePlayerData" event with a score of 10
            Player player = new();
            player.id = 232323;
            player.name = "Sahil";

            EventManager.Execute(OnUpdatePlayerDataEvent, player, 10);
        }
    }
}
