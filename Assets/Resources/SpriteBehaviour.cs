using UnityEngine;
using Unity.Netcode;

public class SpriteBehaviour : NetworkBehaviour
{
    public NetworkVariable<int> colorIndex = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private SpriteRenderer spriteRenderer;

    private int playerIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerIndex = InputHandler.Instance.PlayerIndexes.Count - 1;
        ChangeColor(playerIndex);
    }

    public override void OnNetworkSpawn()
    {
        colorIndex.OnValueChanged += OnColorIndexChanged;
        if (IsServer)
        {
            // colorIndex.Value = Random.Range(0, Palette.playerColors.Length);
            int colorId = PlayerHandler.Instance.GetPlayerCount() - 1;
            colorIndex.Value = colorId < 0 ? 0 : colorId;
        }
        ChangeColor(colorIndex.Value);
    }

    public override void OnNetworkDespawn()
    {
        colorIndex.OnValueChanged -= OnColorIndexChanged;
    }

    private void OnColorIndexChanged(int oldValue, int newValue)
    {
        ChangeColor(newValue);
    }

    private void ChangeColor(int colorId)
    {
        if (colorId >= 0 && colorId < Palette.playerColors.Length)
        {
            Color32 color = Palette.playerColors[colorId];
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogWarning($"Index de couleur invalide : {colorId}");
        }
    }
}
