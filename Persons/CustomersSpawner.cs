using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomersSpawner : MonoBehaviour
{
    [SerializeField] private int _customersCount = 1;
    [SerializeField] private Person[] _personPrefabs;

    public Person SpawnPerson(PersonType personType)
    {
        var matchingPrefabs = _personPrefabs.Where(p => p.PersonType == personType).ToArray();
        var person = Instantiate(matchingPrefabs.RandomItem().gameObject).GetComponent<Person>();
        person.OnSpawned();
        return person;
    }
}