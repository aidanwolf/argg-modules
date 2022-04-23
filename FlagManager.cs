using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager
{
    // Dictionary<string,int> tempFlags = {};
    // Dictionary<string,int> tempFlagCats = {};

    public static int FLAG (string flag) {
        // if (tempFlags[flag] != null) {
        //     return tempFlags[flag];
        // }
        return PlayerPrefs.GetInt(flag,-1);
    }

    public static int FLAG (string flag, out int i) {
        // if (tempFlags[flag] != null) {
        //     return tempFlags[flag];
        // }
        i = FlagManager.FLAG(flag);
        return i;
    }

    public static int SET_FLAG (string flag,int v) {
        Debug.Log("SETTING " + flag + " TO " + v);
        
        //if (temp == null && tempFlags[flag] == null) {
            PlayerPrefs.SetInt(flag,v);
            PlayerPrefs.Save();
        // } else {
        //     if (temp != null) {
        //         if (tempFlagCats[temp] == null) {
        //             tempFlagCats[temp] = [];
        //         }
        //         tempFlagCats[temp].push(flag);
        //     }
        //     if (script.debug)
        //         print(flag + " = " + v);
        //     tempFlags[flag] = v;
        // }
        return v;
    }

    public static int SET_FLAG_lt (string flag,int v) {//,temp) {
        if (v < FlagManager.FLAG(flag)) {
            FlagManager.SET_FLAG(flag,v);//,temp);
        }
        return FlagManager.FLAG(flag);
    }

    public static int SET_FLAG_gt (string flag,int v) {//,temp) {
        if (v > FlagManager.FLAG(flag)) {
            FlagManager.SET_FLAG(flag,v);//,temp);
        }
        return FlagManager.FLAG(flag);
    }

    // global.DESTROY_TFLAG = function (temp) {
        
    //     if (tempFlagCats[temp] == null)
    //         return;
        
    //     for (var i = 0;i < tempFlagCats[temp].length;i++) {
    //         delete tempFlags[tempFlagCats[temp][i]];
    //     }
        
    //     delete tempFlagCats[temp];
    // }
}
