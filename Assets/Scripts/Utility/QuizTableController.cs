using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizTableController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ans_text;
    [SerializeField] private MeshRenderer lightMesh;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject CheckMark;

    public void Activate()
    {
        UI.SetActive(true);
    }

    public void InputNum(int num)
    {
        ans_text.text += num.ToString();
    }

    public void RemoveNum()
    {
        int last = ans_text.text.Length - 1;
        if (last >= 0)
        {
            ans_text.text = ans_text.text.Remove(last, 1);
        }
    }

    public void Sumbit()
    {
        Game_Manager.singleton.CheckAns(int.Parse(ans_text.text));
    }

    public void DisplayCorrect()
    {
        lightMesh.material.SetColor("_EmissionColor", Color.green * 2.2f);
        CheckMark.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void DisplayWrong()
    {
        lightMesh.material.SetColor("_EmissionColor", Color.red * 2.2f);
    }
}
