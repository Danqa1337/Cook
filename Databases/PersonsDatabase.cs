using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonsDatabase", menuName = "Databases/PersonsDatabase")]
public class PersonsDatabase : Database<PersonsDatabase, PersonsTable, PersonsTable.Param, PersonsDatabase>
{
    [SerializeField] private SerializableDictionary<PersonType, PersonData> _personsByType = new SerializableDictionary<PersonType, PersonData>();
    [SerializeField] private List<Person> _prefabs;
    protected override string enumName => "PersonType";

    public static SerializableDictionary<PersonType, PersonData> PersonsByType { get => instance._personsByType; }

    public override void StartUp()
    {
    }

    public static Person GetPrefab(PersonType personType)
    {
        return instance._prefabs.FirstOrDefault(c => c.PersonType == personType);
    }

    protected override void StartReimport()
    {
        base.StartReimport();
        _personsByType = new SerializableDictionary<PersonType, PersonData>();
    }

    protected override void ProcessParam(PersonsTable.Param param)
    {
        var personData = new PersonData();
        personData.spawnChance = param.spawnChance;
        _personsByType.Add(param.enumName.DecodeCharSeparatedEnumsAndGetFirst<PersonType>(), personData);
    }

    public static PersonData GetPersonData(PersonType personType)
    {
        return instance._personsByType[personType];
    }

    public static PersonData[] GetAllPersons()
    {
        return instance._personsByType.Values.ToArray();
    }
}

[System.Serializable]
public class PersonData
{
    public float spawnChance;
}