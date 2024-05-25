using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponCrate : MonoBehaviour
{
    [SerializeField]
    private VisualEffect _visualEffect;

    private Animator _animator;


    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _animator.SetBool("Open", true);
        }
    }




    private void OnLidLifted()
    {
        _visualEffect.SendEvent("OnPlay");
    }
}
