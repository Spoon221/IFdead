using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AI_GAME
{
    public static bool ExitOpened;


    static AI_GAME() => StartGame();

    public static void StartGame()
    {
        ExitOpened = false;
    }
}
