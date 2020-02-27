using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasAtribute : MonoBehaviour
{
    [NullError(WarningLevel.low)] int iamDontSee;

    [NullError(WarningLevel.low)] public Transform iamlowLevel;

    [NullError(WarningLevel.hard)] [SerializeField] private Transform HARD;

    [NullError(WarningLevel.hard)] protected List<List<Dictionary<int, int>>> highLevelWarning;
}
