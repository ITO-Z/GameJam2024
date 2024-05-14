using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Unit : MonoBehaviour
{
    /*

    //Nu cred ca are sens sa lucrez cu scriptable objects aici.

    public UnitTypeSO unitType;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite) Debug.Log("It exists!");
        spriteRenderer.sprite = unitType.sprite;
    }
    */


    public enum Team { Ally, Enemy };
    public Team team;
    public int health;
    public int damage;
    public float speed;
    public float attackSpeed = 1;

    SpriteRenderer spriteRenderer;

    private GameObject selectedGameObject;
    private Unit target = null;
    private Vector2 movePosition;
    bool started = false;

    void Awake()
    {
        var col = GetComponent<PolygonCollider2D>();
        if (col != null)
        {
            Destroy(col);
            col = gameObject.AddComponent<PolygonCollider2D>();
        }
        else
            col = gameObject.AddComponent<PolygonCollider2D>();

        movePosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (team == Team.Ally) spriteRenderer.color = Color.blue;
        else spriteRenderer.color = Color.red;

        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }


    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    private void Update()
    {

        var step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, movePosition, step);

        /* if (Vector2.Distance(transform.position, movePosition) < 0.001f)
         {
             movePosition *= -1.0f;
         }*/
        if (target != null && !started && Vector2.Distance(transform.position, movePosition) <= .01f)
        {
            StartCoroutine("TakeThatCunt");
            started = true;
        }
    }


    public void MoveTo(Vector2 movePosition)
    {
        this.movePosition = movePosition;
        Debug.Log("Moving to " + movePosition.ToString());
    }

    public void Attack(Vector2 movePosition, Unit target)
    {
        this.movePosition = movePosition;
        this.target = target;
        Debug.Log("Target locked");
    }

    private IEnumerator TakeThatCunt()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            if (target == null)
                break;
            target.health -= this.damage;
            if (target.health <= 0)
            {
                Destroy(target.gameObject);
                started = false;
                StopAllCoroutines();
            }
            Debug.Log("Take that cunt!");
        }
    }

}
