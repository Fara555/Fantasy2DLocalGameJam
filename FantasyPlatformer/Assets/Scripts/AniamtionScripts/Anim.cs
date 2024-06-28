using System.Collections;
using UnityEngine;

public class Anim : MonoBehaviour
{
    [SerializeField] private float animationDuration;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(AnimationLoop());
    }

    private IEnumerator AnimationLoop()
    {
        while (true)
        {
            anim.SetBool("Start", false);
            yield return new WaitForSeconds(animationDuration);
            anim.SetBool("Start", true);
            yield return new WaitForSeconds(animationDuration);
        }
    }
}
