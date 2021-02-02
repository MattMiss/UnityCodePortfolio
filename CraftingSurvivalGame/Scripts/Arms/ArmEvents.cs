using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmEvents : MonoBehaviour
{
    public ArmsController armsController;

    /// <summary>
    /// 
    /// </summary>
    public void Action1Finished(){
        armsController.Action1Finished();
    }

    /// <summary>
    /// 
    /// </summary>
    public void HandAtSide(){
        armsController.GrabToolFinished();
    }

    public void Action1Open(){
        armsController.SwingOpen();
    }

    public void Action1Closed(){
        armsController.SwingClosed();
    }

    public void SwingTool(){
        armsController.PlaySwingToolSound();
    }
    
}
