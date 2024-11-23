using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Point_Viz : MonoBehaviour
{
    UnityEngine.EventSystems.EventSystem mEventSystem = UnityEngine.EventSystems.EventSystem.current;

    Vector3 mOffset = new Vector3();

    void OnMouseDown()
    {
        if (mEventSystem.IsPointerOverGameObject())
        {
            return;
        }

        mOffset = transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));

    }

    void OnMouseDrag()
    {
        if (mEventSystem.IsPointerOverGameObject())
        {
            return;
        }
        Vector3 curScreenPoint = new Vector3(
              Input.mousePosition.x,
              Input.mousePosition.y, 0.0f);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + mOffset;
        Vector3 Clamped = new Vector3(Mathf.Clamp(curPosition.x, -(950), 950), Mathf.Clamp(curPosition.y, -950, 950), curPosition.z);
        transform.position = Clamped;
    }
    void OnMouseUp()
    {
        if (mEventSystem.IsPointerOverGameObject())
        {
            return;
        }

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
