using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRange;
    public float attackRate;



    private void OnTriggerStay2D(Collider2D other) //通过other来访问  **被攻击的人**
    {
        other.GetComponent<Character>()?.TakeDamage(this);  //可以获得被攻击的人character的代码，然后执行.将当前类的实例（this）传入  ? 用来判断对方是否有这个函数 
    }
}
