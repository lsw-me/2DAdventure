using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPartrolState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("walk", true);

    }
    public override void LogicUpdate()
    {
        //·¢ÏÖPLayer,ÇÐ»»×´Ì¬µ½×·»÷
        if(currentEnemy.FindPlayer())
        {
            //ÇÐ»»×´Ì¬
            currentEnemy.SwitchState(NPCState.Chase);
            
        }

        if (!currentEnemy.physicsCheck.isGround||(currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);
        }
    }
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false);
    }


}
