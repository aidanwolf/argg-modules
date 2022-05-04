using System.Collections;
using System.Collections.Generic;
using DigitsNFCToolkit;
using UnityEngine;

public class NFC : Module
{

    public string uri {get;set;}

    public string setVar {get;set;}
    public string onDetect {get;set;}

    public override void Init() {
        base.Init();

        SetDefaults();

        #if(!UNITY_EDITOR)
			NativeNFCManager.AddNFCTagDetectedListener(OnNFCTagDetected);
            NativeNFCManager.AddNDEFReadFinishedListener(OnNDEFReadFinished);
			Debug.Log("NFC Tag Info Read Supported: " + NativeNFCManager.IsNFCTagInfoReadSupported());
			Debug.Log("NDEF Read Supported: " + NativeNFCManager.IsNDEFReadSupported());
			Debug.Log("NDEF Write Supported: " + NativeNFCManager.IsNDEFWriteSupported());
			Debug.Log("NFC Enabled: " + NativeNFCManager.IsNFCEnabled());
			Debug.Log("NDEF Push Enabled: " + NativeNFCManager.IsNDEFPushEnabled());
        #endif

        #if(!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Enable();
        #endif

        #if(!UNITY_EDITOR)
			NativeNFCManager.ResetOnTimeout = true;
			NativeNFCManager.Enable();
        #endif
    }

    public override void Deinit() {
        base.Deinit();

        #if(!UNITY_EDITOR) && !UNITY_IOS
			NativeNFCManager.Disable();
        #endif
    }

    public void OnNFCTagDetected(NFCTag tag){
        
    }

    public override void SetDefaults() {
        base.SetDefaults();
        if (string.IsNullOrEmpty(setVar)) {
            setVar = "v";
        }
    }

    public void OnNDEFReadFinished(NDEFReadResult result) {
        if(result.Success) {
            NDEFMessage message = result.Message;
            List<NDEFRecord> records = message.Records;
            int length = records.Count;

            var text = "";

            for(int i = 0; i < length; i++)
            {
                NDEFRecord record = records[i];
                switch(record.Type)
                {
                    case NDEFRecordType.TEXT:
                        TextRecord textRecord = (TextRecord)record;
                        text = textRecord.text;
                        var language = textRecord.languageCode;
                        var encoding = textRecord.textEncoding;
                        break;
                    case NDEFRecordType.URI:
                        UriRecord uriRecord = (UriRecord)record;
                        text = uriRecord.fullUri;
                        var uri = uriRecord.uri;
                        var protocol = uriRecord.protocol;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(onDetect)) {
                //we need to check if the uri matches
                if (string.IsNullOrEmpty(uri) || text.Contains(uri)) {

                    //replace any instance of v with
                    var _onDetect = onDetect.Replace(":" + setVar + ",",":" + text + ",");
                    ModuleParser.Parse(gameObject,_onDetect);
                }
            }
        }
    }
}
