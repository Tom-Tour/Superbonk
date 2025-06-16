using UnityEngine;

[CreateAssetMenu(fileName = "AttacksData", menuName = "Scriptable Objects/AttacksData")]
public class AttacksData : ScriptableObject
{
    public string name;
    public float durationStartup; // délai avant que le coup touche (≃ preparationTime)
    public float durationActive;  // combien de temps la hitbox est active
    public float recoveryTime; // temps de récup avant autre action

    public float damage;
    public Vector2 offset; // position relative au perso (évite position absolue)
    public float radius;   // rayon de la hitbox (CircleCollider2D)
    public Vector2 knockback; // force de recul (x, y)
    public bool canBeCanceledIntoNext; // permet enchaînement ou non
    public AudioClip sfx; // son du coup (facultatif)
    public AnimationClip animation; // animation à jouer pour cette phase (optionnel)
}
