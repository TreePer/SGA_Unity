using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour {

    [SerializeField] private GameObject EnemyPrefap;


    IEnumerator Create() {
        yield return new WaitForSeconds(0.5f);

        GameObject obj = Instantiate(EnemyPrefap);
        obj.transform.position = transform.position;

        Renderer renderer = GetComponent<Renderer>();

        Color color = renderer.material.color;

        color.a = 0.0f;

        obj.transform.parent = GameObject.Find("EnemyList").transform;

        float rColor = 0.0f;

        while (true) {
            yield return null;

            rColor += Time.deltaTime;

            color.a = rColor;

            if (rColor >= 255.0f)
                break;
        }

        EnemyManager.Instance.AddEnemy(obj);

        
        
    }

    private void OnCollisionEnter(Collision collision) {
        StartCoroutine(Create());
    }

}
