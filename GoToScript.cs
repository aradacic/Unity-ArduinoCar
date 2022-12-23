using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GoToScript : MonoBehaviour
{

    public Rigidbody rb;

    public float speed = 1.0f;
    float step;

    Vector3 globalPos;

    public bool movementInProgress = false;
    public bool rotationInProgress = false;
    float distanceThreshold = 0.01f;

    public float rotateAngle;

    public float angleToRotatePhysichalCar;

    public float currentAngle;

    Transform carTransform;



    private void Start()
    {
        carTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        //step neovisan o frame-u i korutina se zove kad se robot ne krece i kad se klikne mis
        step = speed * Time.deltaTime;
        if (!movementInProgress && Input.GetMouseButtonDown(0))
            StartCoroutine(RectangularMove());


    }


    //funkcija s kojom skupimo podatke na koji tile tribamo otic
    public void MoveTo(int x, int z)
    {
        Vector3 pos = new Vector3(x, 0, z);
        globalPos = pos;

    }

    IEnumerator RectangularMove()
    {
        // ovo je istina dok se izvrsava korutina da se nebi zvala svaki frame
        movementInProgress = true;

        // 0. racunamo movement vektor da znamo za koliko triba maknit robota
        Vector3 moveVector = globalPos - transform.position;
        Vector3 target;

        // 1. pomicemo se po x osi odnosno po stupcima
        target = transform.position + new Vector3(moveVector.x, 0, 0);
        currentAngle = carTransform.eulerAngles.y;
        yield return StartCoroutine(MoveTo(target));

        // 2. kad je zavrseno kretanje po x idemo po z odnosno po retcima
        target = transform.position + new Vector3(0, 0, moveVector.z);
        currentAngle = carTransform.eulerAngles.y; 
       
        yield return StartCoroutine(MoveTo(target));

        movementInProgress = false;
        currentAngle = carTransform.eulerAngles.y;
       
    }

    //funkcija za micanje i okretanje robota
    IEnumerator MoveTo(Vector3 destination)
    {
        //okrice robota is ispod uzmemo kut pod kojim je robot okrenut
        transform.LookAt(destination);
        rotateAngle = carTransform.eulerAngles.y;

        //funkcija s milijum ifova da tocan kut posaljemo fizickom robotu
        ReturnAngleForCarRotating((int)currentAngle, (int)rotateAngle);
        

        //vozi robota sve dok robot ne dode na misto na koje triba
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            

            if (Vector3.Distance(transform.position, destination) < distanceThreshold)
                yield break;
            else
                yield return null;
        }

    }

    void ReturnAngleForCarRotating(int carsAngleBefore, int carsAngleAfter)
    {
        //kutovi isti ne triba se okricat
        if(currentAngle == rotateAngle)
        {
            return;
            //iako ce ostat kutovi od proslog klikanja samo necemo slat nista i nece se okricat, da saljemo opet bi se okrenia za kut od prosle runde
        }

        ////////////////////////////////////
        //okretanje za kad je pocetni kut 0 tj kad je okrenut sjeverno
        if(currentAngle == 0 && rotateAngle == 90)
        {
            angleToRotatePhysichalCar = 90;
        }

        if(currentAngle == 0 && rotateAngle == 180)
        {
            angleToRotatePhysichalCar = 180;
        }

        if(currentAngle == 0 && rotateAngle == 270)
        {
            angleToRotatePhysichalCar = -90;
        }
        ///////////////////////////////////////
        
        ///////////////////////////////////////
        //okretanje kad je pocetni kut 90 tj kad je okrenut desno
        if(currentAngle == 90 && rotateAngle == 270)
        {
            angleToRotatePhysichalCar = 180;
        }

        if(currentAngle == 90 && rotateAngle == 180)
        {
            angleToRotatePhysichalCar = 90;
        }

        if(currentAngle == 90 && rotateAngle == 0)
        {
            angleToRotatePhysichalCar = -90;
        }
        //////////////////////////////////////

        //////////////////////////////////////
        //okretanje kad je kut 180, tj kad je okrenut juzno
        if(currentAngle == 180 && rotateAngle == 0)
        {
            angleToRotatePhysichalCar = 180;
        }

        if(currentAngle == 180 && rotateAngle == 90)
        {
            angleToRotatePhysichalCar = -90;
        }

        if(currentAngle == 180 && rotateAngle == 270)
        {
            angleToRotatePhysichalCar = 90;
        }
        //////////////////////////////////////////
        
        /////////////////////////////////////////
        //okretanje kad je kut 270, tj kad je okrenut lijevo
        if(currentAngle == 270 && rotateAngle == 0)
        {
            angleToRotatePhysichalCar = 90;
        }

        if(currentAngle == 270 && rotateAngle == 90)
        {
            angleToRotatePhysichalCar = 180;
        }

        if(currentAngle == 270 && rotateAngle == 180)
        {
            angleToRotatePhysichalCar = -90;
        }
        //////////////////////////////////////
    }

}
