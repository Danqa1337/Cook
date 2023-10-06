using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonsHandler : Singleton<PersonsHandler>
{
    [SerializeField] private CustomersSpawner _spawner;

    public static event Action<Person> OnPersonSpawned;

    public static event Action OnSpawned;

    public static event Action<Person> OnPersonQuit;

    public static event Action<Person> OnPersonInteracted;

    public static event Action<Dialog> OnPersonStartTalking;

    public static event Action<Trader> OnTraderShowWares;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private Person SpawnPerson(PersonType personType)
    {
        var person = _spawner.SpawnPerson(personType);
        OnPersonSpawned?.Invoke(person);
        OnSpawned?.Invoke();
        Debug.Log("Spawned " + personType);
        return person;
    }

    public Customer SpawnCustomer()
    {
        var customer = SpawnPerson(PersonType.Customer) as Customer;
        return customer;
    }

    public Trader SpawnTrader()
    {
        var trader = SpawnPerson(PersonType.Trader) as Trader;
        trader.OnShowWares += delegate { OnTraderShowWares.Invoke(trader); };
        return trader;
    }

    private void OnStartTalking(Dialog dialog)
    {
        OnPersonStartTalking?.Invoke(dialog);
    }
}