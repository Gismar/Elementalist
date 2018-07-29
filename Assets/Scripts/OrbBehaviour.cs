using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    protected bool _IsAttacking = false;
    protected bool _BeganAim = false;
    protected WorldInformation _WorldInfo;
    protected IOrb _Orb;
    protected Vector2 _Offset;
    protected Transform _Player;

    private bool _IsIdle = true;
    private bool _CanUseSecondary = false;
    private bool _Return;
    private float _IdleTimerLerp;
    private float _SecondaryAttackTimer;

    private void Start() => _SecondaryAttackTimer = Time.time + _Orb.SecondaryAttackDelay;  

    void Update () {
        if (_IsIdle)
        {
            transform.position = new Vector2(_Player.position.x + Mathf.Cos(Time.time + _Offset.x) * transform.localScale.x,
                                            _Player.position.y + Mathf.Sin(Time.time + _Offset.y) * transform.localScale.y);
        }
        if (_Return)
        {
            transform.position = Vector3.Lerp(transform.position, _Player.position, _IdleTimerLerp);
            _IdleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _Player.position);
            if (_IdleTimerLerp >= 1)
            {
                _IsIdle = true;
                _Return = false;
            }
        }

        if (!_CanUseSecondary)
        {
            _CanUseSecondary = _SecondaryAttackTimer <= Time.time;
            transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_SecondaryAttackTimer - Time.time) / _Orb.SecondaryAttackDelay);
        }

        if (_IsAttacking) return;

        if (Input.GetKeyDown(_WorldInfo.Recall) && !_IsIdle)
        {
            _Return = true;
            _Orb.SetIdle();
            _IdleTimerLerp = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _Orb.ActivateAimLine();
            _IsIdle = false;
            _Return = false;
        }

        if (Input.GetMouseButton(0))
        {
            transform.rotation = Quaternion.Euler(0, 0, Aim());
            _Orb.UpdateAimLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _IsAttacking = true;
            _BeganAim = false;
            _Orb.MainAttack();
        }

        if (Input.GetMouseButtonDown(1) && _CanUseSecondary)
        {
            SetUpSecondaryAttackTimer();
            _IsAttacking = true;
            _Orb.SecondaryAttack();
        }
    }

    private void SetUpSecondaryAttackTimer()
    {
        _SecondaryAttackTimer = Time.time + _Orb.SecondaryAttackDelay;
        _CanUseSecondary = false;
    }

    private float Aim()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        return rotation;
    }
}
