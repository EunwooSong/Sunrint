using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnetPlayerAnimation : MonoBehaviour {

    //Anim------------------------------------------------------
    public enum AnimState
    {
        idle = 0
        , runForward, forward //SW, W
        , runBackward, backward //SS. S
        , runRight, right//SD, D
        , runLeft, left//SA, A
        , jumxp// Space
    }
    //Anim 상태
    public AnimState animState = AnimState.idle;
    public AnimationClip[] animClips;

    private CharacterController ch;

    private Animation anim;
    //----------------------------------------------------------

    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animation>();
        ch = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 localVelocity = transform.InverseTransformDirection(ch.velocity);
        Vector3 forwardDir = new Vector3(0.0f, 0.0f, localVelocity.z);
        Vector3 rightDir = new Vector3(localVelocity.x, 0.0f, 0.0f);

        if (forwardDir.z >= 0.1f)
        {
            animState = AnimState.forward;
        }
        else if (forwardDir.z <= -0.1f)
        {
            animState = AnimState.backward;
        }
        else if (rightDir.x >= 0.1f)
        {
            animState = AnimState.right;
        }
        else if (rightDir.x <= -0.1f)
        {
            animState = AnimState.left;
        }
        else {
            animState = AnimState.idle;
        }
        //Play Animation
        anim.CrossFade(animClips[(int)animState].name, 0.2f);
    }
}
