using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ArrayExtensions
{
    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}

public class ChunkSelection : MonoBehaviour
{

    [Header("Prefab references")]
    public NestedPrefab[] chunks;

    // Componente do objeto escolhido aleatoriamente
    public NestedPrefab lastSelectedChunk;
    public ScrollingObject currentScrollObject;

    public void SelectChunk()
    {
        if (currentScrollObject == null)
            currentScrollObject = GetComponent<ScrollingObject>();

        lastSelectedChunk = chunks.GetRandom();
        // Pega o tamanho do chunk atual
        currentScrollObject.size =
            lastSelectedChunk.GetComponent<ScrollingObject>().size;

    }

    public Transform GenerateChunk()
    {
        return Instantiate(lastSelectedChunk.transform) as Transform;
    }
}
