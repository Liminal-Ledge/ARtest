using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacePainting : MonoBehaviour
{
    private ARTrackedImageManager trackedImagesManager;
    public GameObject[] ArPrefabs;
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImagesManager.trackedImagesChanged += changed;
    }

    private void OnDisable()
    {
        trackedImagesManager.trackedImagesChanged -= changed;
    }

    private void changed(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var image in eventArgs.added)
        {
            var name = image.referenceImage.name;
            foreach (var prefab in ArPrefabs)
            {
                if (name.Equals(prefab.name) && !_instantiatedPrefabs.ContainsKey(name))
                {
                    var newPrefab = Instantiate(prefab, image.transform);
                    _instantiatedPrefabs[name] = newPrefab;
                }
            }
        }
        foreach (var image in eventArgs.updated)
        {
            _instantiatedPrefabs[image.referenceImage.name].SetActive(image.trackingState == TrackingState.Tracking);
        }
        foreach (var image in eventArgs.removed)
        {
            _instantiatedPrefabs[image.referenceImage.name].SetActive(false);
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
