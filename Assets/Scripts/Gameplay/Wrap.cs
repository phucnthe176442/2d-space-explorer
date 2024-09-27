using UnityEngine;

public class Wrap : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        var moveAjustment = Vector3.zero;
        if (viewportPosition.x < 0) moveAjustment.x += 1;
        else if (viewportPosition.x > 1) moveAjustment.x -= 1;
        if (viewportPosition.y < 0) moveAjustment.y += 1;
        else if (viewportPosition.y > 1) moveAjustment.y -= 1;
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAjustment);
    }
}