using System.Collections;
using UnityEngine;

public class TestEnnemi : MonoBehaviour
{
    private Vector3 direction = Vector3.zero;
    private float speed = 4.0f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        GameObject visual = new GameObject();
        visual.name = "Visual";
        visual.transform.parent = transform;
        visual.transform.localPosition = Vector3.zero;
        visual.transform.localRotation = Quaternion.Euler(90, 0, 0);
        visual.transform.localScale = new Vector3(.5f, .5f, .5f);
        spriteRenderer = visual.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 5;
        spriteRenderer.sprite = Resources.Load<Sprite>("ennemis");
    }
    void Start()
    {
        StartCoroutine(Routine());
    }
    void Update()
    {
        transform.Translate(direction * (Time.deltaTime * speed));
    }
    
    IEnumerator Routine()
    {
        while (true)
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                {
                    direction = Vector3.zero;
                    break;
                }
                case 1:
                {
                    direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                    break;
                }
                default:
                {
                    break;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
