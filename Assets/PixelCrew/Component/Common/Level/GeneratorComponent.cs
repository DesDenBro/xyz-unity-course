using UnityEngine;

public class GeneratorComponent : MonoBehaviour
{
    public void Generate()
    {
        var elements = GetComponentsInChildren<GenerativeElementComponent>();
        if (elements == null || elements.Length == 0) return;

        foreach (var element in elements)
        {
            if (element == null || element.Prefab == null) continue;

            for (int i = 0; i < element.Maximum; i++)
            {
                var val = Random.Range(0, 100);
                if (val <= element.PercentChance)
                {
                    var newPos = new Vector3(transform.position.x + (Random.Range(-100, 100) / 300f), transform.position.y, transform.position.z);
                    Instantiate(element.Prefab, newPos, Quaternion.identity);
                }
            }
        }
    }
}
