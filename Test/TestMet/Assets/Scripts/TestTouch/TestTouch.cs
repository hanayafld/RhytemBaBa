using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(this.RayTouch());
    }

    private IEnumerator RayTouch()
    {
        while (true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("쐈어");
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "KeyB")
                    {
                        Debug.Log("B");
                    }
                    else if (hit.collider.tag == "KeyA")
                    {
                        Debug.Log("A");
                    }
                }
            }
            yield return null;
        }
    }
}
