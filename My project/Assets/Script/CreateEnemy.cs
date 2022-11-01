using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour {

    private GameObject EnemyPrefap;
    private List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();

    private void Awake() {
        EnemyPrefap = Resources.Load("Prefabs/Objects/Jammo") as GameObject;
    }

    private void Start() {
    }
    
    private void OnCollisionEnter(Collision collision) {
        StartCoroutine(Create());
    }

    void FindRenderer(GameObject _obj) {
        for (int i = 0; i < _obj.transform.childCount; ++i) {
            GameObject obj = _obj.transform.GetChild(i).gameObject;

            if(obj.transform.childCount < 0) {
                FindRenderer(obj);
            }

            SkinnedMeshRenderer renderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (renderer != null)
                renderers.Add(renderer);
        }
    }

    IEnumerator Create() {
        yield return new WaitForSeconds(0.5f);

        while (true) {
            yield return new WaitForSeconds(2.0f);

            if (transform.childCount > 0)
                continue;

            GameObject obj = Instantiate(EnemyPrefap);

            obj.transform.name = "Jammo";

            obj.transform.position = transform.position;

            obj.transform.parent = this.transform;

            EnemyManager.Instance.AddEnemy(obj);

            renderers.Clear();
            FindRenderer(obj);

            foreach (SkinnedMeshRenderer renderer in renderers) {
                renderer.material.shader = Shader.Find("Transparent/VertexLit");

                if (renderer.material.HasProperty("_Color")) {
                    Color color = renderer.material.GetColor("_Color");
                    renderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, 0.0f));

                    StartCoroutine(SetColor(renderer, color));
                }
            }
        }

        
    }

    IEnumerator SetColor(SkinnedMeshRenderer renderer, Color color) {
        float rColor = 0.0f;

        while(true) {
            yield return null;

            rColor += Time.deltaTime;
            color.a = rColor;
            renderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, rColor));

            if (rColor >= 255)
                break;
        }
    }
}
