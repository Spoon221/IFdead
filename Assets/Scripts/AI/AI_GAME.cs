using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AI_GAME
{
    public static bool ExitOpened;

    public static Transform[] generators;
    public static Transform[] exits;

    static AI_GAME() => StartGame();

    public static void StartGame()
    {
        ExitOpened = false;
        generators = Array.ConvertAll(GameObject.FindObjectsOfType<Generator>(), item => item.transform);
        exits = Array.ConvertAll(GameObject.FindObjectsOfType<ExitForPlayer>(), item => item.transform);

    }
}
