using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAreaTracker
{
    
    private static Dictionary<int, Area> playerAreas = new Dictionary<int, Area>()
    {
        {1, Area.TopLeft},
        {2, Area.TopRight},
        {3, Area.BottomLeft},
        {4, Area.BottomRight}
    };
public static void SetPlayerArea(int playerNum, Area area)
{
    if (playerAreas.ContainsKey(playerNum))
    {
        playerAreas[playerNum] = area;
    }
    else
    {
        playerAreas.Add(playerNum, area);
    }

    Debug.Log($"[PlayerAreaTracker] Player{playerNum} -> {area}");
}
public static Area GetPlayerArea(int playerNum)
{
    if (playerAreas.TryGetValue(playerNum, out Area area))
    {
        return area;
    }
    else
    {
        Debug.LogWarning($"[PlayerAreaTracker] Player{playerNum} not found, returning None");
        return Area.None;
    }
}
}

