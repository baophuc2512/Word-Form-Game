using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataQuestion", menuName = "Data/DataQuestion")]

public class DataQuestion : ScriptableObject
{
    public List<Question> eachQuestion;
}

[Serializable]

public class Question 
{
    public string textQuestion;
    public string wordToTransform;
    public string typeQuestion;
    public string answer;
    public float time;
}
