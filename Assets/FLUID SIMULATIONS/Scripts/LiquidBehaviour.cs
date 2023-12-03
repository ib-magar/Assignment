using UnityEngine;
using System.Collections;

public class LiquidBehaviour : MonoBehaviour
{
    public GameObject objectToInstantiate; // Assign the prefab in the Inspector
    public Vector2 numberOfObjectsRange;
    public int numberOfObjects = 5;
    public float splashForce = 5f;
    public float splashDuration = 0.5f; // Duration of the splash movement

    private void Start()
    {
       // numberOfObjects = (int)Random.Range(numberOfObjectsRange.x, numberOfObjectsRange.y);
    }

    private void OnMouseDown()
    {
       // InstantiateObjects();
    }

    public void StartLiquidBehaviour()
    {
        InstantiateObjects();
    }

     void InstantiateObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
          GetComponentInChildren<SpriteRenderer>().enabled = false;
            GameObject newObj = Instantiate(objectToInstantiate, transform.position, Quaternion.identity,transform);
            //StartCoroutine(ApplySplashEffect(newObj.transform, i));
        }
    }

    private IEnumerator ApplySplashEffect(Transform objTransform, int index)
    {
        Vector2 originalPosition = objTransform.position;
        Vector2 splashDirection = CalculateCircularDirection(index) * splashForce;
        Vector2 targetPosition = originalPosition + splashDirection;
        float elapsedTime = 0;

        while (elapsedTime < splashDuration)
        {
            objTransform.position = Vector2.Lerp(originalPosition, targetPosition, (elapsedTime / splashDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objTransform.position = targetPosition;
    }

    private Vector2 CalculateCircularDirection(int index)
    {
        float angle = (index / (float)numberOfObjects) * 2 * Mathf.PI;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
