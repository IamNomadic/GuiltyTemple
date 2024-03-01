using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerCombat.cs
    public Transform attackPoint;
    public float attackRange = 0.2f; //feel free to change the attack range too, not sure how much range were giving him at the base state
    public LayerMask enemyLayers;
    
    public void Attack(InputAction.CallbackContext context)
=======
    public Animator PlayerAnimator;
    public Transform AttackCenter;
[SerializeField]
    public float AttackRange = 0.4f; //changes in each form
    public LayerMask EnemyLayers;
    public PlayerInputs PlayerControls;
    private InputAction attack;
    private void Awake()
>>>>>>> Stashed changes:Assets/Scripts/PlayerCombat.cs
    {
        PlayerAnimator = GetComponent<Animator>();
        PlayerControls = new PlayerInputs();
    }
    private void OnEnable()
    {
        attack = PlayerControls.Player.Attack;
        attack.Enable();
    }
    private void OnDisable()
    {
        attack.Disable();
    }
    public void Attack()
    {
        if(attack.triggered)
        {
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerCombat.cs
            AttackMechanic();
            Debug.Log("Attacked");
=======
            AttackMechanic(0.35f);
            PlayerAnimator.SetBool("IsAttacking", false);
>>>>>>> Stashed changes:Assets/Scripts/PlayerCombat.cs
        }
        
    }
<<<<<<< Updated upstream:GuiltyTemple/Assets/Scripts/PlayerCombat.cs
    void AttackMechanic()
    {
        //we cna put an attack animation here later if we make one
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
=======
    IEnumerator AttackMechanic(float executeTime)
    {
        PlayerAnimator.SetBool("IsAttacking", true);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackCenter.position, AttackRange, EnemyLayers);
>>>>>>> Stashed changes:Assets/Scripts/PlayerCombat.cs

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " was hit.");
            //do damage here
        }
        yield return new WaitForSeconds(0.35f);
    }
    
    void OnDrawGizmosSelected()
    {
        if (AttackCenter == null)
            return;
        Gizmos.DrawWireSphere(AttackCenter.position, AttackRange);
    }
}
