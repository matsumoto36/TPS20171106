using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    [HideInInspector]
    public GameObject Target;

    [HideInInspector]
    public float C_LerpSpeed;

    [HideInInspector]
    public float CameraSpeed;

    [HideInInspector]
    public float RayDistance;

    [HideInInspector]
    public float XMaxRadian, XMinRadian;

    [HideInInspector]
    public float minMaxSliderMinValue, minMaxSliderMaxValue;

    //回転ベクトル
    Vector3 C_Rotate;

    //カメラ原点
    Vector3 C_Origin;

    //カメラオフセットの空のオブジェクト
    GameObject CameraOffset;

    //Rayのターゲットとなるオブジェクト
    GameObject RayTarget;

    //カメラの原点となるオブジェクト
    GameObject CameraOrigin;

    // Use this for initialization
    void Start ()
    {
        // カメラのオフセットを生成
        CameraOffset = new GameObject("CameraOffset");

        // RayTargetを生成
        RayTarget = new GameObject("RayTarget");

        // CameraOriginを生成
        CameraOrigin = new GameObject("CameraOrigin");

        CameraOffset.transform.position = Target.transform.position;

        RayTarget.transform.position = new Vector3(CameraOffset.transform.position.x, CameraOffset.transform.position.y, RayDistance);

        CameraOrigin.transform.position = Camera.main.transform.position;

        transform.parent = CameraOffset.transform;

        RayTarget.transform.parent = CameraOffset.transform;

        CameraOrigin.transform.parent = CameraOffset.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        OffsetMove();

        CameraAngle();

        RaySystem();
	}

    void CameraAngle()
    {
        Vector3 CalcRotate = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

        C_Rotate += CalcRotate * CameraSpeed;

        C_Rotate.x = Mathf.Clamp(C_Rotate.x, XMinRadian, XMaxRadian);

        CameraOffset.transform.eulerAngles = C_Rotate;
    }

    void OffsetMove()
    {
        //カメラの原点を取得
        C_Origin = Camera.main.transform.position;

        //オフセットの移動
        CameraOffset.transform.position = Vector3.Lerp(CameraOffset.transform.position, Target.transform.position, C_LerpSpeed);
    }

    void RaySystem()
    {
        RaycastHit hit;

        //オフセットからRayTargetの方向を取得
        Vector3 direction = RayTarget.transform.position - CameraOffset.transform.position;

        //デバッグ用(Rayの描画)
        Debug.DrawRay(CameraOffset.transform.position, RayTarget.transform.position, Color.red, 1.0f);

        //OffsetからRayTargetまでの距離を計算
        float distance = Vector3.Distance(RayTarget.transform.position, CameraOffset.transform.position);

        //Rayを作成
        Ray ray = new Ray(CameraOffset.transform.position, direction);

        if (Physics.Raycast(ray, out hit, distance, 1 << LayerMask.NameToLayer("Wall")))
        {
            //Rayが衝突した場所を取得して、少し調整する
            Vector3 avoidPos = hit.point - direction.normalized * 0.1f;

            //カメラをに移動させる
            transform.position = Vector3.Lerp(transform.position, avoidPos, C_LerpSpeed);

            Debug.Log("Hit");
        }
        else
        {
            //カメラをカメラ原点に移動させる
            transform.position = Vector3.Lerp(transform.position, CameraOrigin.transform.position, C_LerpSpeed);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //インスタンス化
        CameraController Edit = target as CameraController;

        EditorGUILayout.LabelField("ターゲット");
        Edit.Target = (GameObject)EditorGUILayout.ObjectField("Target", Edit.Target, typeof(Object), true);

        EditorGUILayout.LabelField("カメラの移動速度");
        Edit.C_LerpSpeed = EditorGUILayout.Slider("Slider", Edit.C_LerpSpeed, 0.0f, 10.0f);

        EditorGUILayout.LabelField("カメラの回転速度");
        Edit.CameraSpeed = EditorGUILayout.Slider("Slider", Edit.CameraSpeed, 0.0f, 10.0f);

        EditorGUILayout.LabelField("Rayの距離");
        Edit.RayDistance = EditorGUILayout.Slider("Slider", Edit.RayDistance, -10.0f, 0.0f);

        EditorGUILayout.MinMaxSlider(new GUIContent("カメラの角度制限"), ref Edit.XMinRadian, ref Edit.XMaxRadian, -60.0f, 90.0f);

        EditorGUILayout.LabelField("MinDistance = ", Edit.XMinRadian.ToString());
        EditorGUILayout.LabelField("MaxDistance = ", Edit.XMaxRadian.ToString());
    }
}
#endif
