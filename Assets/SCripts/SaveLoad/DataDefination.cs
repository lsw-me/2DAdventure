using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public PersistentType PersistentType;
    public string ID;

    private void OnValidate() //MonoBehaviour���Դ��ķ��������鿴�ֲ�
    {
        if(PersistentType ==PersistentType.ReadWrite)
        {
            if(ID ==string.Empty)
                ID = System.Guid.NewGuid().ToString(); //�Զ�����id
        }
        else
        {
            ID =string.Empty;
        }
    }
}
