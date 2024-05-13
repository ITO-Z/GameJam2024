using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Transform selectionAreaTransform;
    private Vector2 startPosition;
    private List<Unit> selectedUnitList = new List<Unit>();
    private float offset = .5f;

    private void Awake(){
        selectionAreaTransform = transform.GetChild(0);
        selectionAreaTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            selectionAreaTransform.gameObject.SetActive(true);

            startPosition = mouseWorldPosition();
        }

        if(Input.GetMouseButton(0)){
            Vector2 currentMousePosition = mouseWorldPosition();
            MakeSelection(currentMousePosition);
        }

        if(Input.GetMouseButtonUp(0)){
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, mouseWorldPosition());

            foreach(Unit unit in selectedUnitList){
                unit.SetSelectedVisible(false);
                SpriteRenderer unitRenderer = unit.gameObject.GetComponent<SpriteRenderer>();
                unitRenderer.sortingOrder = 5;
            }

            selectedUnitList.Clear();
            
            foreach(Collider2D collider2D in collider2DArray){
                Unit unit = collider2D.GetComponent<Unit>();
                if(unit.team == Unit.Team.Ally){
                    SpriteRenderer unitRenderer = unit.gameObject.GetComponent<SpriteRenderer>();
                    unitRenderer.sortingOrder = 5;
                    
                    unit.SetSelectedVisible(true);
                    selectedUnitList.Add(unit);
                }
            }
            Debug.Log(selectedUnitList.Count);
        }

        if(Input.GetMouseButtonDown(1)){
            Vector2 moveToPosition = mouseWorldPosition();
            Collider2D collider2D = Physics2D.OverlapPoint(mouseWorldPosition());
            if(collider2D != null){
                Unit unit = collider2D.GetComponent<Unit>();
                if(unit != null && unit.team == Unit.Team.Enemy){
                    foreach(Unit my_unit in selectedUnitList){
                        my_unit.Attack(new Vector2(unit.transform.position.x + offset, unit.transform.position.y), unit);
                    }
                }
                else{
                    foreach(Unit my_unit in selectedUnitList){
                        my_unit.MoveTo(moveToPosition);
                    }
                }
            }
            else{
                foreach(Unit unit in selectedUnitList){
                    unit.MoveTo(moveToPosition);
                }
            }


            //List<Vector2> targetPositionList = GetPositionListAround(moveToPosition, new float[] {1f, 2f, 3f}, new int[] {1, 2, 3});

            //int targetPositionListIndex = 0;

            //foreach(Unit unit in selectedUnitList){
            //    unit.MoveTo(targetPositionList[targetPositionListIndex], );
           //     targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
           //}
        }
    }

    private void MakeSelection(Vector2 currentMousePosition){
        Vector2 lowerLeft = new Vector2(
                Mathf.Min(startPosition.x, currentMousePosition.x),
                Mathf.Min(startPosition.y, currentMousePosition.y)
            );
            Vector2 upperRight = new Vector2(
                Mathf.Max(startPosition.x, currentMousePosition.x),
                Mathf.Max(startPosition.y, currentMousePosition.y)
            );
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
    }

    private Vector3 mouseWorldPosition(){
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }


    private List<Vector2> GetPositionListAround(Vector2 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray){
        List<Vector2> positionList = new List<Vector2>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray. Length; i++) {
            positionList.AddRange(GetPositionListAround(startPosition,ringDistanceArray[i],ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector2> GetPositionListAround(Vector2 startPosition, float distance, int positionCount) {
        List<Vector2> positionList = new List<Vector2>();
        for(int i = 0; i <  positionCount; i++){
            float angle = i*(360f / positionCount);
            Vector2 dir = ApplyRotationToVector(new Vector2(1, 0), angle);
            Vector2 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }

    private Vector2 ApplyRotationToVector(Vector2 vec, float angle){
        return Quaternion.Euler(0, 0, angle) * vec;
    }
}
