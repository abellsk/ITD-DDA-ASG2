using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Database;

using System.Text.RegularExpressions;
using UnityEditor.PackageManager;
using UnityEngine.SceneManagement;
using System;
using Oculus.Platform.Models;

public class AuthManager : MonoBehaviour
{

    FirebaseAuth auth;
    DatabaseReference dbReference;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField usernameInput;
    public GameObject signUpBtn;
    public GameObject logInBtn;
    public GameObject forgetPasswordBtn;
    public GameObject signOutBtn;

    public TextMeshProUGUI errorMsgContent;

    //transition
    public Animator transition;
    public float transitionTime = 1f;

    // Start is called before the first frame update
    private void Awake()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public async void SignUp()
    {
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();

        if (ValidateEmail(email) && ValidatePassword(password))
        {
            //Debug.Log(password);
            //Debug.Log(email);
            //validate email and password.
            Debug.Log("validation success");
            FirebaseUser newPlayer = await SignUpNewUser(email, password);

            if (newPlayer != null)
            {
                errorMsgContent.gameObject.SetActive(false);

                string username = usernameInput.text;
                await CreateNewSimplePlayer(newPlayer.UserId, username, username, newPlayer.Email);
                await UpdatePlayerDisplayName(username);
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
        else
        {
            errorMsgContent.text = "Error in Signing Up. Invalid Email or password";
            errorMsgContent.gameObject.SetActive(true);
        }
    }

    private async Task<FirebaseUser> SignUpNewUser(string email, string password)
    {
        Debug.Log("Sign Up method...");
        FirebaseUser newPlayer = null;
        //automatically pass user info to the firebase project
        //attempt to create new user or check with there' already one
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            //perform task handling
            if (task.IsFaulted || task.IsCanceled)
            {
                if (task.Exception != null)
                {
                    string errorMsg = this.HandleSignUpError(task);
                    errorMsgContent.text = errorMsg;
                    Debug.Log("Error in signing up " + errorMsg);
                    errorMsgContent.gameObject.SetActive(true);
                }
                Debug.LogError("sorry, there was an error making your account. ERROR: " + task.Exception);
                //exit from the attempt
            }
            else if (task.IsCompleted)
            {
                errorMsgContent.gameObject.SetActive(false);
                newPlayer = task.Result;
                Debug.LogFormat("Welcome to the game {0} {1} ", newPlayer.UserId, newPlayer.Email);
                // do anything you want after player creation eg. create new player
            }

        });
        return newPlayer;
    }

    public async Task CreateNewSimplePlayer(string uuid, string displayName, string userName, string email)
    {
        User newPlayer = new User(displayName, userName, email);
        Debug.LogFormat("Player details: {0}", newPlayer.PrintPlayer());
        //root/players/uuid
        await dbReference.Child("players/" + uuid).SetRawJsonValueAsync(newPlayer.SimpleGamePlayerToJson());
    }

    public string GetCurrentUserDisplayName()
    {
        return auth.CurrentUser.DisplayName;
    }

    public void LogInUser()
    {
        Debug.Log("Logging In method...");
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();
        if (ValidateEmail(email) && ValidatePassword(password))
        {
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    string errorMsg = this.HandleLogInError(task);
                    errorMsgContent.text = errorMsg;
                    Debug.Log("Error in Logging in " + errorMsg);
                    errorMsgContent.gameObject.SetActive(true);
                    Debug.LogError("sorry, there was an error logging into your account, ERROR: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    errorMsgContent.gameObject.SetActive(false);
                    FirebaseUser currentPlayer = task.Result;
                    Debug.LogFormat("Welcome to NBA2K {0} :: {1}", currentPlayer.UserId, currentPlayer.Email);
                    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
                    GameManager.instance.signInPage.SetActive(false);
                    GameManager.instance.welcomePage.SetActive(true);
                }
            });
        }
        else
        {
            errorMsgContent.text = "Error in Logging In. Invalid email / password";
            errorMsgContent.gameObject.SetActive(true);
        }
    }

    public void SignOutUser()
    {
        Debug.Log("Sign out method...");
        if (auth.CurrentUser != null)
        {
            Debug.LogFormat("auth user {0} {1}", auth.CurrentUser.UserId, auth.CurrentUser.Email);

            //get current index of a scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            auth.SignOut();
            if (currentSceneIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public async Task UpdatePlayerDisplayName(string displayName)
    {
        if (auth.CurrentUser != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = displayName
            };
            await auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was cancelled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User profile updated successfully");
                Debug.LogFormat("Checking current user display name from auth {0}", GetCurrentUserDisplayName());
            });
        }
    }

    public void ForgetPassword()
    {
        string email = emailField.text.Trim();
        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Sorry, there was an error sending a password reset, ERROR: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Forget Password email sent successfully...");
            }
        });
        Debug.Log("Forget Password Method...");
    }

    public FirebaseUser GetCurrentUser()
    {
        return auth.CurrentUser;
    }

    //simple client side email validation
    public bool ValidateEmail(string email)
    {
        bool isValid = false;
        const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
        if (email != "" && Regex.IsMatch(email, pattern, options))
        {
            isValid = true;
        }

        return isValid;
    }

    public bool ValidatePassword(string password)
    {
        bool isValid = false;
        if (password != "" && password.Length >= 3)
        {
            isValid = true;
        }
        return isValid;
    }

    public string HandleSignUpError(Task<FirebaseUser> task)
    {
        string errorMsg = "";

        if (task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            errorMsg = "Sign up Fail\n";
            switch (errorCode)
            {
                case AuthError.EmailAlreadyInUse:
                    errorMsg += "Email already in use";
                    break;
                case AuthError.WeakPassword:
                    errorMsg += "Password is weak. Use at least 3 characters";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "password is missing";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid email used";
                    break;
                default:
                    errorMsg += "Issue in authentication: " + errorCode;
                    break;
            }
            Debug.Log("Error message " + errorMsg);
        }
        return errorMsg;

    }
    public string HandleLogInError(Task<FirebaseUser> task)
    {
        string errorMsg = "";

        if (task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            errorMsg = "Log In Fail\n";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    errorMsg += "Email is missing";
                    break;
                case AuthError.WrongPassword:
                    errorMsg += "Password is wrong";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "password is missing";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid email used";
                    break;
                case AuthError.UserNotFound:
                    errorMsg += "user is not found";
                    break;
                default:
                    errorMsg += "Issue in authentication: " + errorCode;
                    break;
            }
            Debug.Log("Error message " + errorMsg);
        }
        return errorMsg;

    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }


}
