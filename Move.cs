using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Module
{

    //Move[target: || position: || direction:, impulse:, speed: || time:, onMove:]
    //target
    //position (can be vector3 or anchorName)
    //direction
    //speed
    //time
    //string onMove:

    private AnchorManager.Anchor _targetAnchor;
    private AnchorManager.Anchor targetAnchor {
        get {
            if (!string.IsNullOrEmpty(target) && _targetAnchor == null) {
                _targetAnchor = AnchorManager.GetAnchor(target);
            }
            return _targetAnchor;
        }
        set {
            _targetAnchor = value;
        }
    }
    public string target {get;set;}
    public object position {get;set;}
    public string direction {get;set;}

    private Vector3? positionVec {
        get {  
            if (targetAnchor != null) {
                return targetAnchor.position;
            } else if (position != null) {
                Debug.Log("get pos");
                if (position.GetType() == typeof(Vector3)) {
                    Debug.Log("get pos vec3");
                    return transform.position+(Vector3)position;
                } else if (position.ToString().Contains("+")) {
                    var values = position.ToString().Split("+");
                    Vector3 pos = Vector3.zero;
                    for (var i = 0; i < values.Length;i++) {
                        if (values[i].StartsWith ("(") && values[i].EndsWith (")")) {
                            pos += ModuleParser.StringToVector3(values[i]);
                        } else {
                            pos += AnchorManager.GetAnchorPos(values[i]);
                        }
                    }
                    return pos;
                } else {
                    var anchorName = position.ToString();
                    return AnchorManager.GetAnchorPos(anchorName);
                }
            }
            return null;
        }
    }

    private Vector3? directionVec {
        get {
            switch(direction){
            case "forward":
                return transform.forward;
            case "back": 
                return -transform.forward;
            case "right": 
                return transform.right;
            case "left": 
                return -transform.right;
            case "up": 
                return transform.up;
            case "down": 
                return -transform.up;
            default:
                return null;
            }
        }
    }

    public float speed {get;set;}
    public float time {get;set;}

    public bool impulse {get;set;}

    public string onMove {get;set;}

    private Rigidbody rigidbody;

    private float startTime;
    private Vector3 startPos;

    private bool updating = false;

    public override void Init () {
        base.Init();
        rigidbody = GetComponent<Rigidbody>();

        startTime = Time.time;
        startPos = transform.position;
        updating = true;

        SetDefaults();
    }

    public override void Update() {
        base.Update();
 
        if (!updating)
            return;

        // if (!string.IsNullOrEmpty(target) && targetAnchor == null) {
        //     return;
        // }

        if (positionVec != null) {
            if (rigidbody) {
                if (time > 0) {
                    float speed = Vector3.Distance(startPos,(Vector3)positionVec)/time/2;
                    Vector3 moveBy = ((Vector3)positionVec-startPos).normalized*speed/10;
                    rigidbody.MovePosition(transform.position+moveBy);
                } else {
                    rigidbody.position = (Vector3)positionVec;
                }
            } else {
                if (time > 0) {
                    transform.position = Vector3.Lerp(startPos,(Vector3)positionVec,(Time.time-startTime)/time);
                } else {
                    transform.position = (Vector3)positionVec;
                }
            }

        } else if (directionVec != null) {
            if (rigidbody) {
                rigidbody.AddForce(((Vector3)directionVec * speed) - rigidbody.velocity, ForceMode.VelocityChange);
            } else {
                transform.position += (Vector3)directionVec * speed;
            }
        }

        if ((positionVec != null || impulse) && (time == 0 || Time.time-startTime >= time)) {
            updating = false;
            if (!string.IsNullOrEmpty(onMove))
                ModuleParser.Parse(gameObject,onMove);
        }
    }

    public override void Deinit() {
        base.Deinit();

        target = "";
        position = null;
        direction = "";

        Debug.Log("DEINIT MOVE");
    }

    public override void SetDefaults() {
        base.SetDefaults();
        if (speed == 0)
            speed = 1f;
    }
}
