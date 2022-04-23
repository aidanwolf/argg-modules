using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaiju : MonoBehaviour
{
    public TextMesh healthText;

    private float health = 100;

    private bool firstUpdate = false;
    private bool secondUpdate = false;

    // Update is called once per frame
    void Update()
    {
        if (healthText)
            healthText.text = ((Mathf.Round(health*10)/10).ToString() + "%");
    }

    public void ReceiveDamage () {
        health = health - 0.1f;

        if (!firstUpdate && health < 50) {
            FirebaseManager.instance.SendEventUpdateToEvent(1, "hp50");
            firstUpdate = true;
        } else if (!secondUpdate && health <= 0) {
            FirebaseManager.instance.SendEventUpdateToEvent(2, "kill");
            secondUpdate = true;

            Destroy(gameObject);
        }
    }
}
