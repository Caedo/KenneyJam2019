using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager {

    static float fixedDelta = Time.fixedDeltaTime;

    public static void PauseTime() {
        Time.timeScale = 0;
        Time.fixedDeltaTime = fixedDelta * Time.timeScale;
    }

    public static void ResumeTime() {
        Time.timeScale = 1;
        Time.fixedDeltaTime = fixedDelta * Time.timeScale;
    }
}