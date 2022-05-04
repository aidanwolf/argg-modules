using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : Module
{

    private GameObject CollectUI;

    private GameObject collectUIPrefab;
    private GameObject mainCanvas;

    private ItemInfo itemInfo;

    public override void Init () {
        base.Init();

        //Show Collect UI

        Debug.Log("Init Init Init");

        if (CollectUI == null) {

            if (collectUIPrefab == null)
                collectUIPrefab = Resources.Load<GameObject>("CollectUI");

            if (mainCanvas == null)
                mainCanvas = GameObject.Find("Main");

            Debug.Log("show collect ui");

            itemInfo = GetComponent<ItemInfo>();

            CollectUI = Instantiate(collectUIPrefab,mainCanvas.transform);
            CollectUI.GetComponent<CollectPopUpController>().Init(
                itemInfo.itemName + " (" + itemInfo.itemAmount + ")",
                "Owned by " + itemInfo.inventoryId + "\n" + 
                itemInfo.itemDescription, AddToInventory);

        } else {

            Debug.Log("hide collect ui");

            Destroy(CollectUI);
            CollectUI = null;
        }

    }

    public override void Deinit () {
        base.Deinit();

        //Hide Collect UI
    }

    public void AddToInventory () {

        Debug.Log("COLLECTING, ADD TO INVENTORY!");
        //transfer
        ActionManager.SendItem(itemInfo.inventoryId,DiscordController.playerId,itemInfo.itemId);
        Destroy(gameObject);

    }
}
