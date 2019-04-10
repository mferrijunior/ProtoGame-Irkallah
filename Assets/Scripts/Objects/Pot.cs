using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    private BoxCollider2D boxCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Smash()
    {
        audioSource.time = 0.4f;
        audioSource.Play();
        anim.SetBool("smash", true);
        StartCoroutine(BreakCo());
    }

    IEnumerator BreakCo()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(0.55f);
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
