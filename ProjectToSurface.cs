using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectToSurface : Module
{
    //ProjectToSurface[fallback:,cursor:,offset:]

    public string onSurface {get;set;}

    public string onMiss {get;set;}

    public Vector2 cursor {get;set;}

    public Vector3 offset {get;set;}

    public override void Init () {
        base.Init();

        SetDefaults();
    }

    private bool didHit = false;

    public override void Update() {
        base.Update();

        if (!init)
            return;

        Vector2 cursorScaled = Vector2.Scale(cursor, new Vector2(Screen.width,Screen.height));

        Ray ray = Camera.main.ScreenPointToRay (cursorScaled);
		RaycastHit[] hits = Physics.RaycastAll (ray);
		if (hits.Length > 0) {
			foreach (RaycastHit hit in hits) {
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Mesh")) {
					
                    var point = hit.point;
                    var normal = Vector3.Scale(hit.normal,offset);

                    transform.position = point;
                    transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    if (!didHit && !string.IsNullOrEmpty(onSurface)) {
                        ModuleParser.Parse(gameObject,onSurface);
                        didHit = true;
                    }

					break;
				}
			}
		} else {
            if (didHit && !string.IsNullOrEmpty(onMiss)) {
                ModuleParser.Parse(gameObject,onMiss);
                didHit = false;
            }
        }
 
    }

    public override void Deinit() {

        Debug.Log("ProjectToSurface Deinit!!!");

        base.Deinit();
       
    }

    public override void SetDefaults() {
        base.SetDefaults();

        if (cursor == null) {
            cursor = new Vector2(0.5f,0.5f);
        }
        
    }
}
