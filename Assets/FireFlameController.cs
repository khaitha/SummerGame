using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireFlameController : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject flameThrower;
    public Animator animator;  // Add a reference to the Animator component
    private bool isFlameThrowerActive = false;
    public float toggleDuration = 0.4f; // Duration in seconds before it auto-toggles off
    private bool doneFlame = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && !isFlameThrowerActive)
        {
            StartCoroutine(ToggleFlameThrowerCoroutine());
        }
    }

    private IEnumerator ToggleFlameThrowerCoroutine()
    {
        StartFlameThrower();

        // Wait for the specified duration
        yield return new WaitForSeconds(toggleDuration);

        StopFlameThrower();
    }

    void StartFlameThrower()
    {
        doneFlame = false;
        animator.SetBool("DoneFlame", doneFlame);
        // Set the animation boolean to true
        animator.SetTrigger("FireFlame");
        isFlameThrowerActive = true;
        var flame = Instantiate(flameThrower, shootingPoint.position, Quaternion.identity);
        flame.transform.forward = transform.forward;

        // Flip the flame image on its X axis if the player is facing left
        if (transform.localScale.x < 0)
        {
            Vector3 scale = flame.transform.localScale;
            scale.x *= -1;
            flame.transform.localScale = scale;

            // Shift the flame a bit to the left
            flame.transform.position = new Vector3(flame.transform.position.x - 1.2f, flame.transform.position.y, flame.transform.position.z);
        }
        else
        {
            // Shift the flame a bit to the right
            flame.transform.position = new Vector3(flame.transform.position.x + 1.2f, flame.transform.position.y, flame.transform.position.z);
        }

    }

    public void StopFlameThrower()
    {
        isFlameThrowerActive = false;
        doneFlame = true;
        // Set the animation boolean to false
        animator.SetBool("DoneFlame", doneFlame);
    }
}
