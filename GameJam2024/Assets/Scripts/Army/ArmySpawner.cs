using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TestRegion))]
public class ArmySpawner : MonoBehaviour
{
    TestRegion region;

    public Unit unit;
    public float offset = .2f;
    //private int sortingOrder = 0;


    // Start is called before the first frame update
    void Start()
    {
        region = GetComponent<TestRegion>();
        InitTeam(region);
        SpawnUnit(unit);
    }

    private void InitTeam(TestRegion region){
        bool isAlly = region.isConquered;
        if(isAlly) unit.team = Unit.Team.Ally;
        else unit.team = Unit.Team.Enemy;
    }

    public void SpawnUnit(Unit unit){
        InitTeam(region);
        Vector2 spawnPos = new Vector2(region.city.transform.position.x + offset, region.city.transform.position.y + offset);
        Unit unitInst = Instantiate(unit, spawnPos, Quaternion.identity);
        unitInst.transform.parent = transform;
        //unitInst.GetComponent<Renderer>().sortingOrder = sortingOrder++;
        offset += .1f;
    }
}