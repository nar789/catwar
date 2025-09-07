using UnityEngine;

public class RoboCatcher : MonoBehaviour
{

    public float radius = 0f;
    public LayerMask layer;

    public Collider[] colliders;
    Cat cat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cat = GetComponent<Cat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cat.isHitCmd() || GameController.Instance.getTime() < 10f)
        {
            return;
        }


        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            float short_distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            Collider short_enemy = null;

            foreach (Collider col in colliders)
            {

                if(col.GetComponent<CharController>().getIsHit())
                {
                    continue;
                }

                float short_distance2 = Vector3.Distance(transform.position, col.transform.position);
                if (short_enemy == null || short_distance > short_distance2)
                {
                    short_distance = short_distance2;
                    short_enemy = col;
                }
            }

            if (short_enemy == null)
            {
                return;
            }

            CharController robo = short_enemy.GetComponent<CharController>();
            cat.attackRobo(short_enemy.transform.position, robo);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
