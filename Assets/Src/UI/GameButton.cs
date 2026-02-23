using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField] GameObject optionButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OptionButtonOnClick()
    {
        UIManager.Instance.IsSettingOen();
        var optionButtonColor = (UIManager.Instance.isSettingOpen) ? true : false;
        if (optionButtonColor) Debug.Log("ŠJ‚¢‚½");
        if (!optionButtonColor) Debug.Log("•Â‚¶‚½");
        //optionButton.GetComponent<Image>().color = 
    }
}
