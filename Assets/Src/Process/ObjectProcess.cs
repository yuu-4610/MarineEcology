using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectProcess : MonoBehaviour
{
    //インスタンス化
    public static ObjectProcess Instance { get; private set; }
    //[HideInInspector] public Transform objectTransform; //生成予定オブジェクトの位置情報

    private string[] parentObjectsName;
    Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();
    //[HideInInspector] int objectLength; //オブジェクト配列の

    private void Awake()
    {
        //シングルトン
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void OnDestroy()
    {
        //イベント登録解除
        if (ObjectEventManager.Instance != null)
        {
            //ObjectEventManager.Instance.ObjectGenerate -= GeneratePieceObject;
        }
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }

    // Start is called before the first frame update
    //GameObject GenerateObject, Transform objectTransform, int childrenCount
    public GameObject GeneratePieceObject(GameObject generateObject, Transform objectTransform, string parentObjectName)
    {
        //親オブジェクトの生成
        var parentObject = GetPanetObject(parentObjectName);
        //オブジェクト生成
        var pieceObject = Instantiate(generateObject, objectTransform.position, Quaternion.identity, parentObject.transform);

        return pieceObject;
    }
    public GameObject SynthesisPieceEvent(GameObject nextPiece, GameObject piece, Piece otherPiece, string parentObjectName)
    {
        //親オブジェクトの生成
        var parentObject = GetPanetObject(parentObjectName);

        Vector3 center = (piece.transform.position + otherPiece.transform.position) / 2;
        Quaternion rotation = Quaternion.Lerp(piece.transform.rotation, otherPiece.transform.rotation, 0.5f);

        var synthesisPiece = Instantiate(nextPiece, center, rotation, parentObject.transform);

        return synthesisPiece;
    }
    private GameObject GetPanetObject(string name)
    {
        if (!parents.TryGetValue(name, out var parent) || parent == null)
        {
            parent = new GameObject(name);
            parents[name] = parent;
        }

        return parent;
    }
    
}
