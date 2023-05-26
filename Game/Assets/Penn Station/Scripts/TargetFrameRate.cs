#if UNITY_EDITOR
using System.Collections;
using System.Threading;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{

		public int target = 60;
		float currentFrameTime;
		
    // Start is called before the first frame update
    void Start()
    {
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = 9999;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / target;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
  
}
#endif
