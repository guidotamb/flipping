using UnityEngine;
using System.Collections;

public class CircleScript : MonoBehaviour
{

    public float rotationSpeed = 2f;

    private Color A;
    private Color B;
    private bool alreadySwapped = false;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    public void OnClicked()
    {
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        swap(A, B);
        Quaternion temp = transform.rotation;
        float yAngle = temp.y;
        while (true)
        {
            yAngle += 0.1f * rotationSpeed;
            yAngle = yAngle > 1 ? 1 : yAngle;
            transform.rotation = new Quaternion(temp.x, yAngle, temp.z, temp.w);
            yield return new WaitForSeconds(0.00000001f);
            if (yAngle == 1) break;
        }
        internalSwapColor();
        while (true)
        {
            yAngle -= 0.1f * rotationSpeed;
            yAngle = yAngle < 0 ? 0 : yAngle;
            transform.rotation = new Quaternion(temp.x, yAngle, temp.z, temp.w);
            yield return new WaitForSeconds(0.000000001f);
            if (yAngle == 0) break;
        }
        transform.rotation = initialRotation;
        
    }
    void internalSwapColor()
    {
        GetComponent<SpriteRenderer>().color = A;
    }

    void swap(Color otherB, Color otherA)
    {
        this.A = otherA;
        this.B = otherB;
        alreadySwapped = true;
    }

    public void setColors(Color otherA, Color otherB)
    {
        this.A = otherA;
        GetComponent<SpriteRenderer>().color = A;
        this.B = otherB;
    }

    public Color getColor()
    {
        return A;
    }

    public bool AlreadySwapped() {
        return alreadySwapped;
    }
}
