using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image Bar;
    [HideInInspector] public float currentHP;
    [HideInInspector] public Collider2D objectCollider;
    public float maxHP = 100f;

    protected virtual void Start()
    {
        objectCollider = GetComponent<Collider2D>();    
    }

    void OnEnable()
    {
        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        Bar.fillAmount = currentHP / 100;
    }

    public virtual void DealDamage(float damageAmount)
    {
        currentHP -= damageAmount;
    }
}