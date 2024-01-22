using UnityEngine;
using UnityEngine.EventSystems; // Required for detecting clicks on GameObjects

public class PickUpSlime : MonoBehaviour, IPointerClickHandler
{
    // Assuming you have a way to reference or identify the specific slime prefab to instantiate.
    public GameObject slimePrefab;
    public GameObject checkImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        TryPickUpSlime();
    }

    private void TryPickUpSlime()
    {
        if (checkImage.activeSelf) return;
        int emptySlotIndex = SlimeManager.instance.FindFirstEmptySlot();

        if (emptySlotIndex != -1) // Checks if there is an empty slot available
        {
            
            PickUp(emptySlotIndex);
        }
        else
        {
            // No empty slots available, handle accordingly (e.g., show a message)
            Debug.Log("All slime slots are occupied.");
        }
    }

    private void PickUp(int slotIndex)
    {
        // Assuming SlimeManager.instance.slimeIconPrefabs is an array of the prefabs that correspond to the slimes
        // and that slimePrefab is one of those prefabs, find its index or directly use the provided prefab.
        GameObject slimeIcon = Instantiate(slimePrefab, SlimeManager.instance.SlimeSlots[slotIndex].transform);
        slimeIcon.name = slimePrefab.name;
        // Set the local position of the slimeIcon to zero to ensure it's correctly positioned within the slot.
        slimeIcon.transform.localPosition = Vector3.zero;
        checkImage.SetActive(true);

        // Perform any additional setup or assignments here
    }

    


}