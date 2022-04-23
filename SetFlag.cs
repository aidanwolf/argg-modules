using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlag : Module
{
    public string flag {get;set;}
    public override void Init () {
        base.Init();
        
        int lastIndexOf_ = flag.LastIndexOf("_");
        if (lastIndexOf_ == -1)
            lastIndexOf_ = 0;

        string flag_indexString = flag.Substring(lastIndexOf_+1,flag.Length-1-lastIndexOf_);
        //we need to check if flag has an index defined
        if (int.TryParse(flag_indexString, out int flag_index)) {
            var _flag = flag.Substring(1,flag.LastIndexOf("_")-1); 

            Debug.Log("set " + _flag + " to " + flag_index);

            FlagManager.SET_FLAG(_flag,flag_index);
            GameScriptParser.UpdateFlags();
        }
    }

    
}
