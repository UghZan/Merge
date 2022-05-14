using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] TextMesh powerText;
    public int Power;
    Vector3 _target, _oldPos;
    bool _shouldMove;
    float _moveTime, _timer;

    private void Start()
    {
        _shouldMove = false;
        powerText = GetComponentInChildren<TextMesh>();
    }

    public void SetMoveTarget(Vector3 position, float moveTime)
    {
        _shouldMove = true;
        _oldPos = transform.position;
        _target = position;
        _moveTime = moveTime;
    }

    private void Update()
    {
        if (_shouldMove)
        {
            transform.position = Vector3.Lerp(_oldPos, _target, _timer / _moveTime);
            _timer += Time.deltaTime;
            if(_timer >= _moveTime)
            {
                _shouldMove = false;
                transform.position = _target;
            }
        }
    }
    
    public void UpdatePower(int newPower)
    {
        Power = newPower;
        powerText.text = Power.ToString();
    }
}
