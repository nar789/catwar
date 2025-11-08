using UnityEngine;

public class RoboCatcher2 : MonoBehaviour
{

    public float radius = 0f;
    public LayerMask layer;

    public Collider[] colliders;

    EnemyScript enemy;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.isCharging() || GameController.Instance.getIsGameOver())
        {
            enemy.setCmd(EnemyScript.CMD.PATROL);
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


            /*
            if (Random.Range(0, 10) >= 3)
            {
                return;
            }*/

            enemy.setCmd(EnemyScript.CMD.ATTACK);
            
        } else
        {
            enemy.setCmd(EnemyScript.CMD.PATROL);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
