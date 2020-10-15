using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("00000000");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
