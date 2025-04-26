using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;



public class Authentication : MonoBehaviour
{
    

    //mainMenu
    [SerializeField] Transform mainMenuScreenPanel;
    [SerializeField] Button existingUserButton;
    [SerializeField] Button newUserButton;

    //login
    [SerializeField] Transform logInScreenPanel;  
    [SerializeField] Button logInButton;
    [SerializeField] Button signOutButton;
    [SerializeField] private TMP_InputField userEmail;
    [SerializeField] private TMP_InputField userPassword;
    [SerializeField] private TMP_InputField userDisplayName;


    [SerializeField] bool useDefaultCredentials;

    private string defaultEmail = "myTest@testmail.com"; 
    private string defaultPassword = "password";

    public bool isUserAuthentificated;
    [SerializeField] public bool simulateNoInternet;

    private FirebaseAuth authentificationInstance; 
    private FirebaseUser userProfileData; 


    private Coroutine loginOrCreateUserRoutine;







    IEnumerator Start()
    {
        



        SetUp();

        if(Application.internetReachability == NetworkReachability.NotReachable || simulateNoInternet)
        {

            Debug.LogError("No Internet");
            yield break;

        }


        InitialiseFirebase();

        if(authentificationInstance == null)
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

            while(loginOrCreateUserRoutine != null)
            {

                Debug.Log("Logging in");
                yield return null;
            }



            mainMenuScreenPanel.gameObject.SetActive(false);
            logInScreenPanel.gameObject.SetActive(false);   

        }



    }

    private void SetUp()
    { 
        existingUserButton.onClick.RemoveAllListeners();
        existingUserButton.onClick.AddListener(ExistingUser);

        newUserButton.onClick.RemoveAllListeners();
        newUserButton.onClick.AddListener(NewUser);
        signOutButton.onClick.RemoveAllListeners();
        signOutButton.onClick.AddListener(SignOut);


        mainMenuScreenPanel.gameObject.SetActive(true);
        logInScreenPanel.gameObject.SetActive(false);
       

    }


    private void NewUser()
    {

        mainMenuScreenPanel.gameObject.SetActive(false);
        logInScreenPanel.gameObject.SetActive(true); 
        userDisplayName.gameObject.SetActive(true);
        signOutButton.interactable = false;
        logInButton.GetComponentInChildren<TMP_Text>().text = "Sign Up";



        logInButton.onClick.RemoveAllListeners();


        logInButton.onClick.AddListener(CreateUser);
       // logInButton.onClick.AddListener(() => CreateUser(userEmail.text, userPassword.text, userDisplayName.text));




    }


    private void ExistingUser() 
    {

        mainMenuScreenPanel.gameObject.SetActive(false);
        logInScreenPanel.gameObject.SetActive(true);
        userDisplayName.gameObject.SetActive(false);
        signOutButton.interactable = false;
        logInButton.GetComponentInChildren<TMP_Text>().text = "Log In";


        logInButton.onClick.RemoveAllListeners();


        logInButton.onClick.AddListener(SignInUser);    

    }


    void InitialiseFirebase()  
    {

        authentificationInstance = FirebaseAuth.DefaultInstance;

    }

    void CreateUser()
    {
        
        if (loginOrCreateUserRoutine == null)
        {

            loginOrCreateUserRoutine = StartCoroutine(CreateNewUser(userEmail.text.Trim(), userPassword.text.Trim(), userDisplayName.text.Trim())); 

        }

    }

    void SignInUser()
    {

        if (loginOrCreateUserRoutine == null)
        {

            loginOrCreateUserRoutine = StartCoroutine(SignInUser(userEmail.text.Trim(), userPassword.text.Trim()));

        }


    }


    IEnumerator CreateNewUser(string email, string password, string displayName)
    {

        Task<AuthResult> creatingUserTask = authentificationInstance.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return creatingUserTask;

        if(creatingUserTask.IsCompletedSuccessfully)
        {

            Debug.Log("User Created");
            userProfileData = creatingUserTask.Result.User;

            UserProfile newProfile = new UserProfile { DisplayName = displayName };
            //newProfile.DisplayName = displayName;

            Task updateProfile = userProfileData.UpdateUserProfileAsync(newProfile);

           
            yield return updateProfile;

            if(updateProfile.IsCompletedSuccessfully)
            {

                Debug.Log("New User Profile Data Updated");

                //userProfileData = userResult;

            }
            else
            {
                Debug.Log(" User Profile Data not Updated" + updateProfile.Exception);

            }
           
        }
        else
        {

            Debug.Log("New User creation failed" + creatingUserTask.Exception);

        }

        isUserAuthentificated = true;


        loginOrCreateUserRoutine = null;

        signOutButton.interactable = true;

        logInButton.interactable = false;

        yield return null;

    }


    IEnumerator SignInUser(string email, string password)
    {

        Task<AuthResult> signInUserTask = authentificationInstance.SignInWithEmailAndPasswordAsync(email, password);

        
        while(!signInUserTask.IsCompleted)
        {
            yield return null;

        }

        if (signInUserTask.IsCompletedSuccessfully)
        {

            Debug.Log("User Logged in");
            userProfileData = signInUserTask.Result.User;
            // Debug.Log(userProfileData.DisplayName);

            Task reloadTask = userProfileData.ReloadAsync();

            yield return reloadTask;

            if (reloadTask.IsCompletedSuccessfully)
            {
                Debug.Log("Reloaded user data");

                Debug.Log("DisplayName: " + userProfileData.DisplayName);


            }
            else
            {

                Debug.LogWarning("Failed reloading user data" + reloadTask.Exception);



            }











        }
        else
        {

            Debug.Log("New User creation failed" + signInUserTask.Exception);

        }

        isUserAuthentificated = true;


        loginOrCreateUserRoutine = null;

        signOutButton.interactable = true;

        logInButton.interactable = false;


        yield return null;

    }

    void SignInExistingUser()
    {


    }


    void SignOut()
    {

        authentificationInstance.SignOut();


        isUserAuthentificated = false;

        signOutButton.interactable = false;

        logInButton.interactable = true;


        mainMenuScreenPanel.gameObject.SetActive(true);
        logInScreenPanel.gameObject.SetActive(false);
    }


}
