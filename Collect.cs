using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : Module
{

    private GameObject CollectUI;

    private GameObject collectUIPrefab;
    private GameObject mainCanvas;

    private ItemInfo itemInfo;
    private Item itemModule;

    public override void Init () {
        base.Init();

        if (itemModule == null)
            itemModule = Componentizer.DoComponent<Item>(gameObject,true);

        if (CollectUI == null) {

            if (collectUIPrefab == null)
                collectUIPrefab = Resources.Load<GameObject>("CollectUI");

            if (mainCanvas == null)
                mainCanvas = GameObject.Find("Main");

            itemInfo = GetComponent<ItemInfo>();

            CollectUI = Instantiate(collectUIPrefab,mainCanvas.transform);
            CollectUI.GetComponent<CollectPopUpController>().Init(
                itemInfo.itemName + " (" + itemInfo.itemAmount + ")",
                "Owned by " + ((itemInfo.inventoryId==DiscordController.playerId)?"You":itemInfo.inventoryId) + "\n" + 
                itemInfo.itemDescription, ()=>{
                    itemModule.OnCollectItem();
                    ActionManager.SendItem(itemInfo.inventoryId,DiscordController.playerId,itemInfo.itemId);
                    Destroy(gameObject);
                });

        } else {
            Destroy(CollectUI);
            CollectUI = null;
        }

    }
    public override void Deinit () {
        base.Deinit();
        Destroy(CollectUI);
        CollectUI = null;
    }
}
