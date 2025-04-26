using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;



public class FirebaseDatabase : MonoBehaviour
{


    public PlayerSaveData myCurrentData;
    public PlayerSaveData dataFromTheServer;



    private FirebaseAuth authentificationInstance;
    private FirebaseUser userProfileData;
    DatabaseReference databaseReference;



    public bool useDefaultCredentials; 
    private string defaultEmail = "myTest@testmail.com";
    private string defaultPassword = "password";



    public string userID;

    private Coroutine loginOrCreateUserRoutine;

    [SerializeField] public bool simulateNoInternet;

    private Coroutine interactingWithDataBaseRoutine;



    IEnumerator Start()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable || simulateNoInternet)
        {

            Debug.LogError("No Internet");
            yield break;

        }

        InitialiseFirebase();

        if (authentificationInstance == null)
        {

            Debug.LogError("No Authentification");
            yield break;

        }

        if (useDefaultCredentials)
        {

            if (loginOrCreateUserRoutine == null)
            {

                loginOrCreateUserRoutine = StartCoroutine(SignInUser(defaultEmail, defaultPassword));

            }

            while (loginOrCreateUserRoutine != null)
            {

                Debug.Log("Logging in");
                yield return null;
            }

        }

        else
        {

           
        }

        InitAndGetDatabaseReference();

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && interactingWithDataBaseRoutine == null)
        {

            interactingWithDataBaseRoutine = StartCoroutine(SavePlayerDataToServer());

        }


        if (Input.GetKeyDown(KeyCode.E) && interactingWithDataBaseRoutine == null)
        {

            interactingWithDataBaseRoutine = StartCoroutine(SavePlayerDataToServer());

        }



    }


    void InitialiseFirebase()
    {

        authentificationInstance = FirebaseAuth.DefaultInstance;

    }


    void InitAndGetDatabaseReference()
    {

        databaseReference = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;

    }



    IEnumerator SignInUser(string email, string password)
    {


        Task<AuthResult> signInUserTask = authentificationInstance.SignInWithEmailAndPasswordAsync(email, password);



        while (!signInUserTask.IsCompleted)
        {
            yield return null;

        }

        if (signInUserTask.IsCompletedSuccessfully)
        {

            Debug.Log("User Logged in");
            userProfileData = signInUserTask.Result.User;
            Debug.Log(userProfileData.DisplayName);

            userID = userProfileData.DisplayName;


        }
        else
        {

            Debug.Log("New User creation failed" + signInUserTask.Exception);

        }

       
        loginOrCreateUserRoutine = null;

        yield return null;




    }


    IEnumerator SavePlayerDataToServer()
    {

        string jsonData = JsonUtility.ToJson(myCurrentData);

        Task sendJSon = databaseReference.Child("users").Child(userProfileData.UserId).Child("PlayerSaveData").SetRawJsonValueAsync(jsonData);

        while(!sendJSon.IsCompleted && !(sendJSon.IsFaulted || sendJSon.IsCanceled))
        {
            yield return null;

        }

        if(sendJSon.IsFaulted || sendJSon.IsCanceled)
        {
            Debug.LogError("Error with saving the data");

            yield break;
        }

        Debug.Log("game saved");
        interactingWithDataBaseRoutine = null;

        yield return null;

         
    }

    //IEnumerator LoadPlayerDataFromServer()
    //{
    //    interactingWithDataBaseRoutine = null;

    //    yield return null;

    //}

}
