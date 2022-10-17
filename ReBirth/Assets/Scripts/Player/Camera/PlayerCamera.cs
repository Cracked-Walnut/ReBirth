using UnityEngine;
using Cinemachine;

/*
Purpose:
    To handle camera switches in real-time.

Last Edited:
    10-17-22.

How to Use:
    1. Create a PlayerCamera object in the script you wish to manipulate CinemachineVirtualCamera properties.
    2. Pick the proprty/properties you wish to manipulate. E.g., Vritual Camera priorities.
    3. Invoke the Priority setter in the script you've declared PlayerCamera.
*/

public class PlayerCamera : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cmvc;
    private int _priority; // Camera with the highest priority takes precedence

    // Camera manipulation (follow instructions at the top)
    public void SetPriority(int _priority) => cmvc.Priority = _priority;

}
