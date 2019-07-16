using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool Operate;
    public Note currentNote;
    public Note outNote;

    public void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("조작가능");
        this.Operate = true;
        this.currentNote = other.GetComponent<Note>();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("조작불가");
        this.Operate = false;
        this.outNote = other.GetComponent<Note>();
    }
}
