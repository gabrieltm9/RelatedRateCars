using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    public float targetTime;
    public GameObject targetTimeInput;

    public GameObject car1;
    public GameObject car2;

    public float car1Speed;
    public float car2Speed;

    public GameObject speed1Txt;
    public GameObject speed2Txt;

    public LineRenderer hypotenuseLr;
    public LineRenderer baseLr;
    public LineRenderer heightLr;

    public GameObject hypotenuseTxt;
    public GameObject baseTxt;
    public GameObject heightTxt;
    public GameObject relatedRateTxt;

    public float relatedRate;

    public float a;
    public float b;
    public float c;

    public GameObject interceptPoint;

    public bool isRunning;

    // Update is called once per frame
    void Update()
    {
        if(!isRunning)
        {
            CalculateSpeed();
            if (Input.GetKeyDown(KeyCode.Space))
                MoveCars();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.Space)))
            {
                StopAllCoroutines();
                isRunning = false;
            }
        }

        DrawHypotenuse();
        DrawBase();
        DrawHeight();

        interceptPoint.transform.position = new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z);
    }

    void DrawHypotenuse()
    {
        hypotenuseLr.SetPosition(0, car1.transform.position);
        hypotenuseLr.SetPosition(1, car2.transform.position);

        float newX = (car1.transform.position.x + car2.transform.position.x) / 2;
        float newZ = (car1.transform.position.z + car2.transform.position.z) / 2;
        hypotenuseTxt.transform.position = new Vector3(newX + 3, car1.transform.position.y + 5, newZ - 3);
        c = Mathf.Sqrt((a * a) + (b * b));
        c = Mathf.Round(c * 10f) / 10f;
        hypotenuseTxt.GetComponent<Text>().text = "" + c;
    }

    void DrawBase()
    {
        baseLr.SetPosition(0, car1.transform.position);
        baseLr.SetPosition(1, new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z));

        float newZ = (car1.transform.position.z + car2.transform.position.z) / 2;
        baseTxt.transform.position = new Vector3(car1.transform.position.x, car1.transform.position.y, newZ);
        a = Mathf.Abs(car1.transform.position.z - car2.transform.position.z);
        a = Mathf.Round(a * 10f) / 10f;
        baseTxt.GetComponent<Text>().text = "" + a;
    }

    void DrawHeight()
    {
        heightLr.SetPosition(0, car2.transform.position);
        heightLr.SetPosition(1, new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z));

        float newX = (car1.transform.position.x + car2.transform.position.x) / 2;
        heightTxt.transform.position = new Vector3(newX, car2.transform.position.y, car2.transform.position.z);
        b = Mathf.Abs(car1.transform.position.x - car2.transform.position.x);
        b = Mathf.Round(b * 10f) / 10f;
        heightTxt.GetComponent<Text>().text = "" + b;
    }

    void MoveCars()
    {
        GetRelatedRate();
        isRunning = true;
        StartCoroutine(MoveToPosition(car1.transform, new Vector3(car1.transform.position.x, car1.transform.position.y, car2.transform.position.z), targetTime));
        StartCoroutine(MoveToPosition(car2.transform, new Vector3(car1.transform.position.x, car2.transform.position.y, car2.transform.position.z), targetTime));
    }

    void RunSim()
    {
        isRunning = true;
    }

    void CalculateSpeed()
    {
        float xDist = car1.transform.position.x - car2.transform.position.x;
        float zDist = car1.transform.position.z - car2.transform.position.z;

        car1Speed = zDist / targetTime;
        car2Speed = xDist / targetTime;

        car1Speed = Mathf.Round(car1Speed * 10f) / 10f;
        car2Speed = Mathf.Round(car2Speed * 10f) / 10f;

        speed1Txt.GetComponent<Text>().text = "" + car1Speed;
        speed2Txt.GetComponent<Text>().text = "" + car2Speed;
    }

    public void GetRelatedRate()
    {
        float tempA = car1Speed * a * 2;
        float tempB = car2Speed * b * 2;
        float tempAB = Mathf.Abs(tempA) + Mathf.Abs(tempB);
        float tempC = c * 2;
        relatedRate = tempAB / tempC;
        relatedRate = Mathf.Round(relatedRate * 10f) / 10f;

        relatedRateTxt.GetComponent<Text>().text = relatedRate + "u/s";
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        isRunning = false;
    }

    public void UpdateTargetTime()
    {
        targetTime = float.Parse(targetTimeInput.GetComponent<TMP_InputField>().text);
    }
}
