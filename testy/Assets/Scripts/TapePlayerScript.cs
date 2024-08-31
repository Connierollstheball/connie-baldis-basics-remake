using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: Add logic to this

public class TapePlayerScript : MonoBehaviour
{
    public Sprite Closed;
    public Sprite Open;
    public AudioSource AudioSource;
    public bool playingtape = false;

    public void PlayTape()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Closed;
        playingtape = true;
        AudioSource.Play();
        Invoke("StopTape", 10.0f);
    }

    public void StopTape()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Open;
        playingtape = false;
        AudioSource.Stop();
    }
}
