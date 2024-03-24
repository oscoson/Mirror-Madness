using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiraCount : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI reflectorText;
    [SerializeField] TextMeshProUGUI rotatorText;

    private MiraManager miraManager;
    // Start is called before the first frame update
    void Start()
    {
        miraManager = FindAnyObjectByType<MiraManager>();
    }

    private void Update()
    {
        reflectorText.text = $"| Remaining: {(miraManager.MaxReflectorCount - miraManager.ReflectorCount)}" ;
        rotatorText.text = $"| Remaining: {(miraManager.MaxRotatorCount - miraManager.RotatorCount)}";
    }

}
