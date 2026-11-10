using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Door : MonoBehaviour
{
    public void Interact()
    {
        // is it bedtime?
        if(WorldTime.Instance.IsPaused) 
        { 
            SleepManager.Instance.GoToSleep();
        }
        else
        {
            Debug.Log("Not bedtime yet.");
        }
    }

    public string GetPrompt()
    {
        if (WorldTime.Instance.IsPaused)
        {
            return "Click to go to bed.";
        }else
        {
            return "Too early to sleep.";
        }
    }
}
