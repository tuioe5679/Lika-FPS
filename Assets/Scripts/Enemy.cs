using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float hp;
    public int str;

    public GameObject Text;
    public PlayerController player;
    public Transform targer;
    
    private bool dieanim;
    private Animator anim;
    private NavMeshAgent nav;
    private CapsuleCollider capcollider;
   

    private void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capcollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            StartCoroutine("die");
        }
        NavigationAI();
      
    }

    private void NavigationAI()
    {
        if (nav.destination != targer.transform.position && dieanim!=true)
        {
            nav.SetDestination(targer.position);
        }
        else
        {
            nav.SetDestination(transform.position);
        }
    }

    IEnumerator die()
    {
        anim.SetBool("die", true);
        dieanim = true;
        capcollider.isTrigger = true;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    IEnumerator Damageinstante()
    {
        GameObject DamageText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + Random.Range(2,2.2f),transform.position.z), transform.rotation);
        TextMeshPro tmp = DamageText.GetComponent<TextMeshPro>();
        Rigidbody Damagerigid = DamageText.GetComponent<Rigidbody>();
        Damagerigid.AddForce(Vector3.up * 2f,ForceMode.Impulse);
        tmp.text = player.AttackStr.ToString();
        yield return new WaitForSeconds(0.15f);
        Destroy(DamageText);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine("Damageinstante");
            hp -= player.AttackStr;
        }
    }
}
