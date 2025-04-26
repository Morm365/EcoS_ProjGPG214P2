using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;





public class UnityAnalytics : MonoBehaviour
{



    IEnumerator Start()
    {

        yield return UnityServices.InitializeAsync();


        AnalyticsService.Instance.StartDataCollection();


        //AnalyticsService.Instance.StopDataCollection();


        CustomEvent OnPlayerDeath = new CustomEvent("playerDeaths")
        {

             {"playerDeaths", 1 },
             {"playerDeathPosition", transform.position.ToString()}

        };








        AnalyticsService.Instance.RecordEvent(OnPlayerDeath);

        Debug.Log("On Player death called");

        yield return null;




        


    }





}



public static class GameEvents
{


    public static CustomEvent OnPlayerDeath = new CustomEvent("playerDeaths")
    {

        {"playerDeaths", 1 }

    };




    public static CustomEvent OnStartUpEvent = new CustomEvent("StartUpInformation")
    {

        {"devicetype", SystemInfo.deviceType.ToString()},

            {"platform", Application.platform.ToString()}





    };

 



}
