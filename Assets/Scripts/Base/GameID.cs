using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameID
{
    public static HashSet<int> UniqueidList = new();
    
    public static int AllocateID()
    {
        int id = -1;
        for(int i = 0;i < UniqueidList.Count;i++)
        {
            if(!UniqueidList.Contains(i))
            {
                id = i;
                return id;
            }
        }
        if(id == -1)
        {
            id =  UniqueidList.Count;
        }
        return id;
    }
    public static void registerID(int id)
    {
        UniqueidList.Add(id);
    }
    public static void unregisterID(int id)
    {
        if(UniqueidList.Contains(id))
        {
            UniqueidList.Remove(id);
        }
    }
}
