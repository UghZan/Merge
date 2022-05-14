using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MergeController : MonoBehaviour
{
    [SerializeField] float mergeTime; //how long the merge itself takes
    [SerializeField] float mergeRadus; //how large is mergeable characters search radius
    [SerializeField] LayerMask mask; //contains character layer
    [SerializeField] ParticleSystem mergeEffect;

    Camera _cam;
    RaycastHit _hit;
    Character _currentCharacter;
    bool _mergeInProgress;
    int _targetPower = 0;
    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _hit = new RaycastHit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !_mergeInProgress)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition); //get ray from touch position
            if(Physics.Raycast(ray, out _hit, 100, mask.value)) //check raycast against only character layer
            {
                _currentCharacter = _hit.collider.GetComponent<Character>(); //get a character that we are merging into
                MergeWithAroundCharacters();
            }
        }
    }

    void MergeWithAroundCharacters()
    {
        _mergeInProgress = true;
        Collider[] collidersAround = Physics.OverlapSphere(_currentCharacter.transform.position, mergeRadus, mask.value); //get all characters around
        List<Character> characters = new List<Character>();
        foreach(Collider col in collidersAround) //get all the same power characters excluding our merge target
        {
            Character character = col.GetComponent<Character>();
            if (character.Power == _currentCharacter.Power && character != _currentCharacter) characters.Add(character);
        }
        _targetPower = _currentCharacter.Power + characters.Count; //calculate resulting power beforehand
        StartCoroutine(MergeAnimation(characters));

    }

    IEnumerator MergeAnimation(List<Character> merged)
    {
        foreach (Character ch in merged)
        {
            ch.SetMoveTarget(_currentCharacter.transform.position, mergeTime);
        }
        yield return new WaitForSeconds(mergeTime);

        mergeEffect.transform.position = _currentCharacter.transform.position;
        mergeEffect.Play(); //play merge effect on character's position
        _currentCharacter.Power = _targetPower;
        foreach (Character ch in merged)
        {
            ch.gameObject.SetActive(false);//disable merged characters for later use
        }
        _mergeInProgress = false; //notify that we may merge again
        yield return null;
    }
}
