using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live1 : MonoBehaviour 
{
    public int health = 50;
    public int hunger = 10;

    private float moveSpeed;

    private Vector2 moveDirection;

    private float moveDistance;

    private Vector2 targetPosition;

    private bool isMoving = false;

    private float waitTime = 0f;


    void Update()
    {

        if (!isMoving)
        {

            waitTime -= Time.deltaTime;

            if (waitTime <= 0f)
            {

                moveSpeed = Random.Range(1f, 4f);

                moveDistance = Random.Range(1f, 5f);

                float angle = Random.Range(0f, 360f);

                moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

                targetPosition = (Vector2)transform.position + moveDirection * moveDistance;

                float zAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0, 0, zAngle);

                isMoving = true;





            }





        }





        if (isMoving)
        {

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {


                isMoving = false;

                waitTime = Random.Range(1f, 8f);



            }


        }


    }


}