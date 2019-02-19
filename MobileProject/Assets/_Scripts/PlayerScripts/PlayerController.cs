﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
    public bool isMoving;
    public bool dash;
    public GameUI gameUI;
    public ParticleSystem p1;
    public ParticleSystem p2;
    public ParticleSystem squishPE;
    private Rigidbody2D rb;
    private float swingForce;
    private float dashForce;
    [SerializeField]
    private float[] gravityTier;
    RopeSystem rs;
    private Animator m_anim;
	// Use this for initialization
	void Start () {
        if (!m_anim)
            m_anim = GetComponent<Animator>();
        if (swingForce <= 0)
            swingForce = 4.0f;
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        rs = GetComponent<RopeSystem>();
        if (dashForce <= 0)
            dashForce = 6.0f;
        if(gravityTier.Length == 0)
            gravityTier = new float[3] { 2.5f, 5f, 10f };
	}

    private void Update()
    {
        m_anim.SetFloat("vVel", rb.velocity.y);
        m_anim.SetFloat("hVel", rb.velocity.x);
    }

    #region Dynamic Gravity Update
    private void FixedUpdate()
    {
        if(rb.velocity.y > 10.0f)
            rb.velocity += Vector2.up * Physics2D.gravity.y * gravityTier[0] * Time.fixedDeltaTime;
        else if(rb.velocity.y > 5.0f && rb.velocity.y < 10.0f)
            rb.velocity += Vector2.up * Physics.gravity.y * gravityTier[0] * Time.fixedDeltaTime;
    }
    #endregion

    public void Dash(int direction)
    {
        Vector2 dir = (rs.ropeHingeAnchor.transform.position - transform.position).normalized;
        Vector2 perp = new Vector2(dir.y, -dir.x) * swingForce;
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        switch(direction){
            case 0:
                p1.Play();
                rb.AddRelativeForce(perp, ForceMode2D.Impulse);
                break;
            case 1:
                p1.Play();
                rb.AddRelativeForce(-perp, ForceMode2D.Impulse);
                break;
            case 2:
                p1.Play();
                rb.AddRelativeForce(transform.right * dashForce, ForceMode2D.Impulse);
                break;
            case 3:
                p2.Play();
                rb.AddRelativeForce(-transform.right * dashForce, ForceMode2D.Impulse);
                break;
        }
    }
    #region Dynamic Gravity Update
    public void Swing(Vector2 ropeDir)
    {
        Vector2 swingDir = new Vector2(ropeDir.y, -ropeDir.x).normalized;
        rb.velocity = swingDir * rb.velocity.magnitude;
    }
    #endregion


    public void Squish()
    {
        gameUI.RemoveSubscriber();
        GameManager.instance.SaveGame();
        squishPE.Play();
        //GameManager.instance.LoadScene("TitleScene");
    }

    public void Slice()
    {
        gameUI.RemoveSubscriber();
        GameManager.instance.SaveGame();
        GameManager.instance.LoadScene("TitleScene 1");
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "KillZone" || c.tag == "Ground")
        {
            gameUI.RemoveSubscriber();
            GameManager.instance.SaveGame();
            GameManager.instance.LoadScene("TitleScene 1");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_anim.SetTrigger("collision");
    }
}
