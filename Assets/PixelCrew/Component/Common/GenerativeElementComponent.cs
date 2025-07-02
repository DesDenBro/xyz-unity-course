using UnityEngine;

public class GenerativeElementComponent : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _maximum = 1;
    [SerializeField] private int _percentChance = 100;

    public GameObject Prefab => _prefab;
    public int Maximum => _maximum;
    public int PercentChance => _percentChance;
}
