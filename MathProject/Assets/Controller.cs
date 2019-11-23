using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public GameObject car1;
    public GameObject car2;

    public LineRenderer hypotenuseLr;
    public LineRenderer baseLr;
    public LineRenderer heightLr;

    // Update is called once per frame
    void Update()
    {
        DrawHypotenuse();
        DrawBase();
        DrawHeight();
    }

    void DrawHypotenuse()
    {
        hypotenuseLr.SetPosition(0, car1.transform.position);
        hypotenuseLr.SetPosition(1, car2.transform.position);
    }

    void DrawBase()
    {
        baseLr.SetPosition(0, car1.transform.position);
        baseLr.SetPosition(1, new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z));
    }

    void DrawHeight()
    {
        heightLr.SetPosition(0, car2.transform.position);
        heightLr.SetPosition(1, new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z));
    }
}
