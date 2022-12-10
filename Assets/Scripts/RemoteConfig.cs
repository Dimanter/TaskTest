using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using System;
using UnityEditor;

public class RemoteConfig : MonoBehaviour
{
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    // Start is called before the first frame update
    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        Debug.Log("RemoteConfig configured and ready!");
    }

    public void FetchFireBase()
    {
        FetchDataAsync();
    }

    public string GetUrl()
    {
        return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                 .GetValue("url").StringValue;
    }

    public void ShowData()
    {
        
        Debug.Log("Current Data:");
        Debug.Log("Url: " +
                 Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                 .GetValue("Url").StringValue);
        Debug.Log("Url: " +
                 Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                 .GetValue("url").StringValue);
        //Debug.Log("MaxCountToShowAdmob: " +
        //         Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        //         .GetValue("MaxCountToShowAdmob").LongValue);
        //Debug.Log("config_test_float: " +
        //         Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        //         .GetValue("config_test_float").DoubleValue);
        //Debug.Log("config_test_bool: " +
        //         Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
        //         .GetValue("config_test_bool").BooleanValue);
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask =
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    //[END fetch_async]

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task => {
                    Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));
                });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
