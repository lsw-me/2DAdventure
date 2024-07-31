using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public PersistentType PersistentType;
    public string ID;

    private void OnValidate() //MonoBehaviour中自带的方法，详情看手册
    {
        if(PersistentType ==PersistentType.ReadWrite)
        {
            if(ID ==string.Empty)
                ID = System.Guid.NewGuid().ToString(); //自动生成id
        }
        else
        {
            ID =string.Empty;
        }
    }
}
