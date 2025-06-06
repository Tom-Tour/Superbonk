using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private GridLayoutGroup gridGroup;

    private void Awake()
    {
        gridGroup = GetComponent<GridLayoutGroup>();
    }
    private void Start()
    {
        int index = 0;
        foreach (Transform child in gridGroup.transform)
        {
            int capturedIndex = index;
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnButtonClicked(capturedIndex, button));
            }
            index++;
        }
    }

    private void OnButtonClicked(int index, Button button)
    {
        Debug.Log($"Bouton cliqué à l’index {index}, nom : {button.name}");
    }
}
