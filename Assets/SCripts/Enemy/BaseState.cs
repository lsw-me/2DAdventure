using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState     //抽象类，不继承MonoBehaviour，无法挂载到GameObject 上面
{
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);  //声明第一个函数方法

    public abstract void LogicUpdate();

    public abstract void PhysicsUpdate();
    public abstract void OnExit();

}
