using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState     //�����࣬���̳�MonoBehaviour���޷����ص�GameObject ����
{
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);  //������һ����������

    public abstract void LogicUpdate();

    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
