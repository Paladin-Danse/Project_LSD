using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldCacher
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _WaitForSecondsDic = new Dictionary<float, WaitForSeconds>();
    private static readonly Dictionary<float, WaitForSecondsRealtime> _WaitForsecondsRealtimeDic = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSeconds WaitForSeconds(float seconds) 
    {
        if(!_WaitForSecondsDic.TryGetValue(seconds, out WaitForSeconds wfs)) 
        {
            _WaitForSecondsDic.Add(seconds, wfs = new WaitForSeconds(seconds));
        }
        Debug.Log(_WaitForSecondsDic.Count);
        return wfs;
    }

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds) 
    {
        if (!_WaitForsecondsRealtimeDic.TryGetValue(seconds, out WaitForSecondsRealtime wfsrt))
        {
            _WaitForsecondsRealtimeDic.Add(seconds, wfsrt = new WaitForSecondsRealtime(seconds));
        }
        return wfsrt;
    }

    public static void ClearCache() 
    {
        _WaitForSecondsDic.Clear();
        _WaitForsecondsRealtimeDic.Clear();
    }
}
