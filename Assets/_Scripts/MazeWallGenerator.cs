using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallGenerator : MonoBehaviour
{
    public enum MazePiece
    {
        DeadEnd,
        Corner,
        Hallway,
        Wall,
        CrossSection
    }

    public MazePiece pieceType;

    public float length = 5.0f; // -5 to 5
    public float width = 0.85f;

    [Header("Sections")]
    public GameObject wallPositiveX;
    public GameObject wallNegativeX;
    public GameObject wallPositiveZ;

    [Header("Forest Objects")]
    public GameObject[] trees;
    public GameObject[] bushes;
    public GameObject[] stones;

    // Start is called before the first frame update
    void Start()
    {
        float lengthOffset = 0.0f;
        float widthOffset = 0.0f;

        if (pieceType != MazePiece.CrossSection)
        {
            for (int i = 0; i < 8; i++)
            {
                lengthOffset += Random.Range(1.0f, 1.25f);
                widthOffset = Random.Range(-0.1f, -0.85f);

                GameObject temp1;
                GameObject temp2;
                GameObject temp3;

                switch (pieceType)
                {
                    case MazePiece.DeadEnd:
                        temp1 = Instantiate(GetPiece(), wallPositiveX.transform);
                        temp1.transform.localPosition = new Vector3(widthOffset, 0, length - lengthOffset);
                        temp1.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        temp2 = Instantiate(GetPiece(), wallNegativeX.transform);
                        temp2.transform.localPosition = new Vector3(-widthOffset, 0, length - lengthOffset);
                        temp2.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        temp3 = Instantiate(GetPiece(), wallPositiveZ.transform);
                        temp3.transform.localPosition = new Vector3(length - lengthOffset, 0, widthOffset);
                        temp3.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
                        break;

                    case MazePiece.Hallway:
                        temp1 = Instantiate(GetPiece(), wallPositiveX.transform);
                        temp1.transform.localPosition = new Vector3(widthOffset, 0, length - lengthOffset);
                        temp1.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        temp2 = Instantiate(GetPiece(), wallNegativeX.transform);
                        temp2.transform.localPosition = new Vector3(-widthOffset, 0, length - lengthOffset);
                        temp2.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        wallPositiveZ.SetActive(false);
                        break;

                    case MazePiece.Corner:
                        temp1 = Instantiate(GetPiece(), wallPositiveX.transform);
                        temp1.transform.localPosition = new Vector3(widthOffset, 0, length - lengthOffset);
                        temp1.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        temp2 = Instantiate(GetPiece(), wallPositiveZ.transform);
                        temp2.transform.localPosition = new Vector3(length - lengthOffset, 0, widthOffset);
                        temp2.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        wallNegativeX.SetActive(false);
                        break;

                    case MazePiece.Wall:
                        temp1 = Instantiate(GetPiece(), wallPositiveZ.transform);
                        temp1.transform.localPosition = new Vector3(length - lengthOffset, 0, widthOffset);
                        temp1.transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);

                        wallPositiveX.SetActive(false);
                        wallNegativeX.SetActive(false);
                        break;
                }
            }
        }
        else
        {
            wallPositiveX.SetActive(false);
            wallNegativeX.SetActive(false);
            wallPositiveZ.SetActive(false);
        }
    }

    private GameObject GetPiece()
    {
        int r = Random.Range(1, 5);

        if (r == 1)
            return trees[Random.Range(0, trees.Length)];
        else if (r == 2 || r == 3)
            return bushes[Random.Range(0, bushes.Length)];
        else
            return stones[Random.Range(0, stones.Length)];
    }
}
