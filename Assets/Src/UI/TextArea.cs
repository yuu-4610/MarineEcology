using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; //反映テキスト
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //得点をテキストに
        textMeshPro.text = UIManager.Instance.GetPoint().ToString();
    }
}
