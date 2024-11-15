using System.Collections;
using UnityEngine;
using TMPro;

public class CountdownUIHandler : MonoBehaviour
{
    public TextMeshProUGUI countDownText;

    private void Awake()
    {
        if (countDownText == null)
        {
            Debug.LogError("countDownText is not assigned in the Inspector.");
            return;
        }

        countDownText.text = ""; // Reset countdown text initially
    }

    // Start is called before the first frame update
    void Start()
    {
        // Confirm the GameObject is active
        gameObject.SetActive(true);

        Debug.Log("Starting countdown...");
        StartCoroutine(CountDownCO());
    }

    IEnumerator CountDownCO()
    {
        yield return new WaitForSeconds(0.3f);

        int counter = 3;

        // Countdown loop
        while (counter > 0)
        {
            countDownText.text = counter.ToString();
            Debug.Log("Countdown: " + counter); // Log the countdown value
            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        // Display "GO" at the end
        countDownText.text = "GO";
        Debug.Log("Countdown: GO");
        yield return new WaitForSeconds(0.5f);

        // Hide the countdown UI after it finishes
        gameObject.SetActive(false);
        Debug.Log("Countdown finished and UI hidden.");
    }
}

