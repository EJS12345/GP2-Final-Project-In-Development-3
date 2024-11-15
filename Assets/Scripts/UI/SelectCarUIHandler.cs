using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car prefabs")]
    public GameObject redCarPrefab;  // Add a reference for the Red Car Prefab
    public GameObject blueCarPrefab; // Add a reference for the Blue Car Prefab

    [Header("Spawn on")]
    public Transform spawnOnTransform;

    bool isChangingCar = false;

    CarData[] carDatas;

    int selectedCarIndex = 0;

    // Other components
    CarUIHandler carUIHandler = null;

    // Start is called before the first frame update
    void Start()
    {
        // Load the car data
        carDatas = Resources.LoadAll<CarData>("CarData/");

        StartCoroutine(SpawnCarCO(true));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            OnPreviousCar();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            OnNextCar();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSelectCar();
        }
    }

    public void OnPreviousCar()
    {
        if (isChangingCar)
            return;

        selectedCarIndex--;

        if (selectedCarIndex < 0)
            selectedCarIndex = carDatas.Length - 1;

        StartCoroutine(SpawnCarCO(true));
    }

    public void OnNextCar()
    {
        if (isChangingCar)
            return;

        selectedCarIndex++;

        if (selectedCarIndex >= carDatas.Length)
            selectedCarIndex = 0;

        StartCoroutine(SpawnCarCO(false));
    }

    public void OnSelectCar()
    {
        // Set the car ID for Player 1 and Player 2
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.Save();

        // Load the next scene
        SceneManager.LoadScene("Course1");
    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isChangingCar = true;

        if (carUIHandler != null)
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);

        // Instantiate the selected car prefab (Red or Blue)
        GameObject instantiatedCar = null;

        // Choose the appropriate car based on selectedCarIndex
        if (selectedCarIndex == 0) // Assuming index 0 is for Red Car
        {
            instantiatedCar = Instantiate(redCarPrefab, spawnOnTransform);
        }
        else // Assuming index 1 is for Blue Car
        {
            instantiatedCar = Instantiate(blueCarPrefab, spawnOnTransform);
        }

        // Setup the car UI handler
        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.SetupCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);

        yield return new WaitForSeconds(0.4f);

        isChangingCar = false;
    }
}


