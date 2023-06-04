using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Particle System Reference")]
    [SerializeField]
    private new ParticleSystem particleSystem;

    public float fallTime = 0.8f;

    public Vector3 rotationPoint;

    public const int Height = 20;
    public const int Width = 10;

    private float _previousTime;

    private BlockSpawner _blockSpawner;

    private static readonly Transform[,] Grid = new Transform[Width, Height];

    private void Start()
    {
        _blockSpawner = GameManager.Instance.BlockSpawner;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);

                if (!ValidMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
                if (!ValidMove())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
            }

        float time;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            time = fallTime / 10;
        }
        else
        {
            time = fallTime;
        }

        if (Time.time - _previousTime > time)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                particleSystem.Play();
                AddToGrid();
                CheckForLines();
                AudioManager.Instance.PlayFX(1);
                EmptyChildCheck();
                this.enabled = false;

                if (GameManager.Instance.currentGameState != GameManager.GameStates.GameOver)
                {
                    _blockSpawner.SpawnNewBlock();
                }
            }

            _previousTime = Time.time;
        }
    }

    private void CheckForLines()
    {
        for(int i = Height - 1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    private bool HasLine(int i)
    {
        for(int j = 0; j < Width; j++)
        {
            if (Grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < Width; j++)
        {
            Destroy(Grid[j, i].gameObject);
            Grid[j, i] = null;
            
        }

        GameManager.Instance.ScoreUpdate(10);
        AudioManager.Instance.PlayFX(0);
    }

    private void RowDown(int i)
    {
        for(var y = i; y < Height; y++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (Grid[j, y] == null)
                {
                    continue;
                }

                Grid[j, y - 1] = Grid[j, y];

                if (Grid != null)
                {
                    Grid[j, y] = null;
                    Grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            //GameOver Check
            if (roundedY == 18)
            {
                StartCoroutine(GameManager.Instance.ChangeGameStates(GameManager.GameStates.GameOver));
            }

            Grid[roundedX, roundedY] = children;
        }
    }

    private bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= Width || roundedY < 0 || roundedY >= Height)
            {
                return false;
            }

            if (Grid[roundedX, roundedY])
                return false;
        }

        return true;
    }

    private void EmptyChildCheck()
    {
        foreach(Transform blocks in _blockSpawner.transform)
        {
            if (blocks.transform.childCount <= 0)
            {
                Destroy(blocks.gameObject);
            }
        }

    }
}
