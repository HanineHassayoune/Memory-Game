using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // the game grid size
    public const int columns = 4;
    public const int rows = 2;

    //the spacing between images in the grid
    public const float Xspace = 4f;
    public const float Yspace = -4f;

    [SerializeField] private MainImageScript startObject;

    //Array of sprite images to be used in the game
    [SerializeField] private Sprite[] images;

    //Function to randomize the order of elements in an array
    private int[] Randomiser(int[] locations)
    {
        int[] array = locations.Clone() as int[];
        for (int i = 0; i < array.Length; i++)
        {
            int newArray = array[i];
            int j = Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = newArray;
        }
        return array;
    }

    private void Start()
    {
        int[] locations = { 0, 0, 1, 1, 2, 2, 3, 3 };
        locations = Randomiser(locations);

        Vector3 startPosition = startObject.transform.position;

        // Create the grid of images 
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                MainImageScript gameImage;
                if (i == 0 && j == 0)
                {
                    // Use the startObject as the first image in the grid
                    gameImage = startObject;
                }
                else
                {
                    // Instantiate a new MainImageScript as a copy of the startObject
                    gameImage = Instantiate(startObject) as MainImageScript;
                }

                int index = j * columns + i;
                int id = locations[index];
                gameImage.ChangeSprite(id, images[id]);

                float positionX = (Xspace * i) + startPosition.x;
                float positionY = (Yspace * j) + startPosition.y;

                gameImage.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }
    }

    private MainImageScript firstOpen;
    private MainImageScript secondOpen;

    private int score = 0;
    private int attempts = 0;

    [SerializeField] private TextMesh scoreText;
    [SerializeField] private TextMesh attemptsText;

    public bool canOpen
    {
        get { return secondOpen == null; }
    }

    public void imageOpened(MainImageScript startObject)
    {
        if (firstOpen == null)
        {
            firstOpen = startObject;
        }
        else
        {
            secondOpen = startObject;
            StartCoroutine(CheckGuessed());
        }
    }

    private IEnumerator CheckGuessed()
    {
        if (firstOpen.spriteId == secondOpen.spriteId) // Compares the two objects
        {
            score++; // Add score
            scoreText.text = "Score: " + score;
        }
        else
        {
            yield return new WaitForSeconds(0.5f); // Start timer

            firstOpen.Close();
            secondOpen.Close();
        }

        attempts++;
        attemptsText.text = "Attempts: " + attempts;

        firstOpen = null;
        secondOpen = null;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }  


}
