using System.Collections.Generic;
using UnityEngine;

public class DamageCallBackController
{
    private List<DamageCallbackInfo> InfoList = new();
    private Transform startPlace;
    private float defSpeed;
    private float showTime = .25f;

    private float modifier;
    private Coroutine coroutine;
    private Coroutine movingCoroutine;

    public void Init(Transform startPlace, float damageSpeed, float showTime)
    {
        this.startPlace = startPlace;
        this.showTime = showTime;
        defSpeed = damageSpeed;
    }
    
    public void AddCallBack(GameObject callback)
    {
        var damageInfo = callback.GetComponent<DamageCallbackInfo>();
        var speed = GetSpeed(GetModifier());
        var add = new Vector3(Random.Range(-220, -80),Random.Range(-100, 100),0);

        damageInfo.speed = speed;
        damageInfo.showTime = showTime;
        damageInfo.startPlace = startPlace;
        damageInfo.Target = add;
        damageInfo.Ended += Remove;
        InfoList.Add(damageInfo);
        damageInfo.StartMove();
    }

    public void Clear()
    {
        foreach (var info in InfoList)
        {
            info.Destroy();
        }
        InfoList.Clear();
    }

    private void Remove(DamageCallbackInfo info)
    {
        InfoList.Remove(info);
        info.Destroy();
    }

    private float GetModifier()
    {
        return Mathf.Clamp(1f * InfoList.Count, 1f, 1f);
    }

    private float GetSpeed(float modifier)
    {
        return defSpeed * modifier * Time.fixedDeltaTime;
    }
}