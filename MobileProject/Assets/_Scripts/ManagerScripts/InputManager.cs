﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
    public RopeSystem rs;
    public PlayerController pc;
    Vector2 begin = Vector2.zero;
    Vector2 end = Vector2.zero;
    float beginTime = 0;
    float endTime = 0;
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.isPaused)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    begin = touch.position;
                    beginTime = Time.time;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    end = touch.position;
                    endTime = Time.time;
                    Debug.Log("Touch Time: " + (end - begin).magnitude);
                    Vector2 dif = end - begin;
                    if (dif.magnitude < 10f)
                    {
                        rs.ShootHook(touch);
                        pc.dash = true;
                    }
                    else if (pc.dash)
                    {
                        if (dif.x > 0)
                            pc.Dash(0);
                        else
                            pc.Dash(1);
                        pc.dash = false;
                    }
                }
            }
        }
        else
            Input.ResetInputAxes();
	}
}
