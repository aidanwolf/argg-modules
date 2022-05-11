using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class ModuleParser : MonoBehaviour
{
    //var test = 'Item[equipped:!AnchorToHand,LookAt[speed:12],Gun]

    //Item.equipped = []
    //calls Item().Init();
    //Init parses equipped

    public static ModuleParser instance;

    private void Awake() {
        instance = this;
    }

    public class GameObjectWithScript {
        public GameObject gObj;
        public string script;

        public GameObjectWithScript (GameObject gObj, string script) {
            this.gObj = gObj;
            this.script = script;
        }
    }

    public class ModuleFunc {

        public string func;
        public Dictionary<string,string> param;

        public ModuleFunc (string func, string paramString) {
            this.func = func;

            if (!String.IsNullOrEmpty(paramString)) {
                param = new Dictionary<string, string>();

                List<int> colonIndex = new List<int>();
            
                string pattern = @"(\w+(?: \w+)*):";
                foreach (Match match in Regex.Matches(paramString, pattern)) {
                    //Debug.Log("REGEX " + match.Value + " / " + match.Index);
                    colonIndex.Add(match.Index);
                    colonIndex.Add(match.Index + match.Value.Length);
                }

                string paramName = "";
                string paramData = "";
                int waitToClose = 0;
                int paramStep = 0;

                var i = 0;

                Debug.Log(paramString);

                //1. split functions and parameters into two strings
                while (paramString.Length > 0) {

                    if (waitToClose == 0) {
                        if (colonIndex.Count > 0 && i >= colonIndex[0]) {
                            paramStep++;
                            colonIndex.RemoveAt(0);
                            
                        } else if (paramString[0] == '[') {
                            waitToClose++;
                        }
                    } else {
                        if (colonIndex.Count > 0 && i >= colonIndex[0]) {
                            colonIndex.RemoveAt(0);
                        }
                        if (paramString[0] == ']') {
                            waitToClose--;
                        } else if (paramString[0] == '[') {
                            waitToClose++;
                        }
                    }

                    if (paramStep == 1) {
                        paramName += paramString[0].ToString();
                    } else if (paramStep == 2) {
                        paramData += paramString[0].ToString();
                    } else if (paramStep == 3) {
                        //to-do: this was sloppy way to fix up strings
                        var paramNameF = paramName.Substring(0,paramName.Length-1);
                        var paramDataF = paramData.Substring(0,paramData.Length-1);
                        param.Add(paramNameF,paramDataF);
                        paramName = paramString[0].ToString();
                        paramData = "";
                        paramStep = 1;
                    }

                    paramString = paramString.Remove(0,1);

                    if (paramString.Length == 0) {
                        //to-do: this was sloppy way to fix up strings
                        var paramNameF = paramName.Substring(0,paramName.Length-1);
                        param.Add(paramNameF,paramData);
                    }
                    i++;
                }

            }
            

            // Debug.Log("func: " + func);
            // Debug.Log("param: " + param);
        }

    }
    //return list of modules to call

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        sVector = sVector.Substring(1, sVector.Length-2);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static Vector2 StringToVector2(string sVector)
    {
        // Remove the parentheses
        sVector = sVector.Substring(1, sVector.Length-2);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector2 result = new Vector2(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]));

        return result;
    }

    public static Vector4 StringToVector4(string sVector)
    {
        // Remove the parentheses
        sVector = sVector.Substring(1, sVector.Length-2);

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector4 result = new Vector4(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));

        return result;
    }


    public static void Parse (GameObject gameObject, string itemScript, bool INIT = true) {
        ModuleParser.instance.StartCoroutine(instance.ParseRoutine(gameObject,itemScript,INIT));
    }

    public IEnumerator ParseRoutine (GameObject gameObject, string itemScript, bool INIT = true) {

        var _itemScript = itemScript;
        List<ModuleFunc> functions = new List<ModuleFunc>();
        string functionToAdd = "";
        string paramsToAdd = "";
        int waitToClose = 0;

        //1. split functions and parameters into two strings
        while (_itemScript.Length > 0) {

            if (waitToClose == 0) {
                if (_itemScript[0] == ',') {
                    functions.Add(new ModuleFunc(functionToAdd,paramsToAdd));
                    functionToAdd = "";
                    paramsToAdd = "";
                } else if (_itemScript[0] == '[') {
                    waitToClose++;
                } else {
                    functionToAdd = functionToAdd + _itemScript[0].ToString();
                }
            } else {
                if (_itemScript[0] == ']') {
                    waitToClose--;;
                } else if (_itemScript[0] == '[') {
                    waitToClose++;
                }
                if (waitToClose > 0)
                    paramsToAdd = paramsToAdd + _itemScript[0].ToString();
            }

            _itemScript = _itemScript.Remove(0,1);

            if (_itemScript.Length == 0) {
                functions.Add(new ModuleFunc(functionToAdd,paramsToAdd));
            }
        }

        Debug.Log("function # " + functions.Count);

        List<Component> modules = new List<Component>();

        int assetsToLoad = 0;

        for (var x = 0;x < functions.Count;x++) {
            if (functions[x].param != null) {
                foreach (var keypair in functions[x].param) {
                    if (!keypair.Value.Contains("[") && !keypair.Value.Contains("<") && keypair.Value.Contains("/")) {

                        if (AssetManager.HasAsset(keypair.Value))
                            continue;

                        assetsToLoad++;
                        AssetManager.AddAsset(keypair.Value,null);

                        Debug.Log("keypair.Value = " + keypair.Value);

                        //to-do:check if object has any of item left in inventory
                        var inventoryId = gameObject.GetComponent<ItemInfo>()?.itemId;

                        FirebaseManager.Get(keypair.Value,(snapshot)=>{
                            if (snapshot.Exists) {
                                ActionManager.instance.SpawnItem(inventoryId, snapshot, (gObj, script) =>{
                                    AssetManager.AddAsset(keypair.Value,new GameObjectWithScript(gObj,script));
                                    assetsToLoad--;
                                });
                            } else {
    #if UNITY_EDITOR
                                var localAsset = GameObject.Find(keypair.Value.Split("/")[1]);
                                var localAssetScript = localAsset.GetComponent<TestModuleScript>().ModuleScript;
                                AssetManager.AddAsset(keypair.Value,new GameObjectWithScript(localAsset,localAssetScript));
                                assetsToLoad--;
    #else
                                Debug.Log("Module -> load asset failed");
    #endif
                            }
                        });
                    }
                }
            }
        }

        yield return new WaitUntil(()=> assetsToLoad==0);

        //1. add functions and split parameters into strings
        for (var i = 0; i < functions.Count;i++) {

            //1. add functions to gameobject
                //to-do: check if script inherits from Module to avoid destructive code

            if (gameObject == null)
                break;

            var func = functions[i].func;
            var param = functions[i].param;

            Type type = Type.GetType(func);
            var module = gameObject.GetComponent(type);
            if (module == null) {
                module = gameObject.AddComponent(type);
            }  

            //2. set parameters in module script 
            if (module) {
                
                modules.Add(module);

                if (param != null) {
                    foreach (KeyValuePair<string,string> keypair in param) {

                        Debug.Log("Attempting to set property " + keypair.Key + " to " + keypair.Value);

                        var prop = type.GetProperty(keypair.Key);

                        if (prop == null) {
                            Debug.LogWarning("Setting property " + keypair.Key + " failed! Likely does not exist..");
                            continue;
                        }


                        if (keypair.Value.Contains("[")) {

                            prop.SetValue(module, keypair.Value, null);

                        } else if (!keypair.Value.Contains("<") && keypair.Value.Contains("/")) {
                            //slash implies asset path

                            //load texture
                            //load music

                            prop.SetValue(module, AssetManager.GetAsset(keypair.Value), null);

                        } else if (keypair.Value.StartsWith ("(") && keypair.Value.EndsWith (")")) {
                            int freq = keypair.Value.Split(',').Length - 1;
                            if (freq == 1) {
                                Debug.Log("Vec2()");
                                prop.SetValue(module, StringToVector2(keypair.Value), null);
                            } else if (freq == 2) {
                                Debug.Log("Vec3()");
                                prop.SetValue(module, StringToVector3(keypair.Value), null);
                            } else if (freq == 3) {
                                Debug.Log("Vec4()");
                                prop.SetValue(module, StringToVector4(keypair.Value), null);
                            }
                        } else if (int.TryParse(keypair.Value, out int intValue)) {
                           prop.SetValue(module, intValue, null);
                        } else if (float.TryParse(keypair.Value, out float floatValue)) {
                           prop.SetValue(module, floatValue, null);
                        } else if (bool.TryParse(keypair.Value, out bool boolValue)) {
                           prop.SetValue(module, boolValue, null);
                        } else {
                          prop.SetValue(module, keypair.Value, null);
                        }
                    }
                }
            }
            if (INIT) {
                (module as Module).Init();
            } else {
                (module as Module).Deinit();
            }
        }

        // for (var x = 0; x < modules.Count;x++) {
        //     if (INIT) {
        //         (modules[x] as Module).Init();
        //     } else {
        //         (modules[x] as Module).Deinit();
        //     }
        // }

        yield return null;
    }
}
