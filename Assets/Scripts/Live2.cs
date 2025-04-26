using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2 : MonoBehaviour
{
    public int health = 100;

    public int hunger = 30;

    public int maximumHunger = 30;

    public int level = 1;

    private float hungerTimer = 0f;

    private bool findTheFood = false;

    private GameObject creatureForFood = null;

    private float moveSpeed;

    private Vector2 moveDirection;

    private float moveDistance;

    private Vector2 targetPosition;

    private bool isMoving = false;

    private float waitTime = 0f;


    void Update()
    {



        hungerTimer += Time.deltaTime; 

        if (hungerTimer >= 1f)
        {

            hunger = Mathf.Max(hunger - 1, 0);

            hungerTimer = 0f;


        }


        if (hunger <= 15)
        {

            if(!findTheFood)
            {

               FindCreature();


            }

            if(creatureForFood != null)
            {

              MoveToCreature();

                return;


            }
            else
            {

                findTheFood = false;

            }



        }

        else
        {

            findTheFood = false;

            creatureForFood = null;

        }


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


    void FindCreature()
    {

        GameObject[] creatures = GameObject.FindGameObjectsWithTag("Creature");

        float closeDistantion = Mathf.Infinity;

        GameObject close = null;

        foreach (GameObject c in creatures)
        {
            float distantion = Vector2.Distance(transform.position, c.transform.position);

            if (distantion < closeDistantion)
            {
                closeDistantion = distantion;

                close = c;



            }



        }


        if(close != null )
        {

            creatureForFood = close;

            findTheFood = true;

        }    


    }


    void MoveToCreature()
    {

        if (creatureForFood == null)
        {

            return;

        }


        Vector2 targetPosition = creatureForFood.transform.position;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4f * Time.deltaTime);

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);



        if(Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            Destroy(creatureForFood);

            hunger = maximumHunger;

            level += 1;

            findTheFood = false;

            creatureForFood = null;

            isMoving = false;

            waitTime = Random.Range(1f, 8f);

        }






    }














}
