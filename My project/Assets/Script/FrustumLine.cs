using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumLine : MonoBehaviour {

    [SerializeField] private Camera mainCamera;

    [SerializeField] [Range(0.01f, 1.0f)] private float X, Y, W, H;

    [SerializeField] private List<GameObject> CullingList = new List<GameObject>();
    [SerializeField] private List<MeshRenderer> RendererList = new List<MeshRenderer>();
    [SerializeField] private LayerMask Mask;

    Vector3[] Line = new Vector3[4];

    float Distance = 0.0f;
    float rColor = 1.0f;


    private void Awake() {
        mainCamera = transform.GetComponent<Camera>();
    }

    private void Start() {
        X = 0.47f;
        Y = 0.49f;
        W = 0.06f;
        H = 0.06f;

        Distance = 14.5f;
    }

    private void FixedUpdate() {
        mainCamera.CalculateFrustumCorners(new Rect(X, Y, W, H), mainCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, Line);

        CullingList.Clear();

        foreach(Vector3 element in Line) {
            var worldSpacePos = mainCamera.transform.TransformVector(element).normalized;

            Debug.DrawRay(transform.position, worldSpacePos * 14.5f, Color.black);

            Ray ray = new Ray(transform.position, worldSpacePos);

            RaycastHit[] hits = Physics.RaycastAll(ray, Distance, Mask);

            foreach(RaycastHit h in hits) {
                if (!CullingList.Contains(h.transform.gameObject)) {
                    CullingList.Add(h.transform.gameObject);
                }
            }
        }

        if(CullingList.Count == 0)
            foreach (MeshRenderer renderer in RendererList) {
                renderer.material.shader = Shader.Find("Transparent/VertexLit");

                if (renderer.material.HasProperty("_Color")) {
                    Color color = renderer.material.GetColor("_Color");
                    StartCoroutine(SetColor2(renderer, color));
                }
            }

        RendererList.Clear();

        foreach (GameObject obj in CullingList)
            FindRenderer(obj);


        foreach (MeshRenderer renderer in RendererList) {
            renderer.material.shader = Shader.Find("Transparent/VertexLit");

            if (renderer.material.HasProperty("_Color")) {
                Color color = renderer.material.GetColor("_Color");
                StartCoroutine(SetColor(renderer, color));
            }
        }

    }
    void FindRenderer(GameObject obj) {
        int i = 0;

        do {
            if (obj.transform.childCount > 0)
                FindRenderer(obj.transform.GetChild(i).gameObject);

            MeshRenderer renderer = obj.transform.GetComponent<MeshRenderer>();

            if(renderer != null) {
                RendererList.Add(renderer);
            }
            else
                ++i;

        } while (obj.transform.childCount > i);

    }

    IEnumerator SetColor(MeshRenderer renderer, Color color) {

        while (rColor > 0.3f) {
            yield return null;  

            rColor -= Time.deltaTime;
            renderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, rColor));
            Debug.Log(rColor);
        }
    }

    IEnumerator SetColor2(MeshRenderer renderer, Color color) {

        while (rColor < 255.0f) {
            yield return null;

            rColor += Time.deltaTime;
            renderer.material.SetColor("_Color", new Color(color.r, color.g, color.b, rColor));
        }
    }
}
