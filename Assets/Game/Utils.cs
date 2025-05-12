using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Dessine un cercle sur le plan XY, ignoré l'axe Z.
    /// </summary>
    /// <param name="center">Centre du cercle</param>
    /// <param name="radius">Rayon du cercle</param>
    /// <param name="color">Couleur des lignes Debug</param>
    /// <param name="duration">Durée d'affichage des lignes</param>
    public static void DrawCircle(Vector3 center, float radius, Color color, float duration = 0f)
    {
        int numSegments = 100;
        float angleStep = 360f / numSegments;

        Vector3[] points = new Vector3[numSegments];

        for (int i = 0; i < numSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle);
            points[i] = new Vector3(x, y, center.z); // Z est inchangé
        }

        for (int i = 0; i < numSegments - 1; i++)
        {
            Debug.DrawLine(points[i], points[i + 1], color, duration);
        }

        Debug.DrawLine(points[numSegments - 1], points[0], color, duration);
    }

    /// <summary>
    /// Clamp un point à l'intérieur d'un cercle défini sur le plan XY.
    /// </summary>
    /// <param name="point">Point à vérifier</param>
    /// <param name="center">Centre du cercle</param>
    /// <param name="radius">Rayon du cercle</param>
    /// <returns>Point inchangé ou "clampé" sur le périmètre</returns>
    public static Vector3 ClampPoint(Vector3 point, Vector3 center, float radius)
    {
        Vector2 delta = new Vector2(point.x - center.x, point.y - center.y);
        float distance = delta.magnitude;

        if (distance > radius)
        {
            Vector2 clamped = delta.normalized * radius;
            return new Vector3(center.x + clamped.x, center.y + clamped.y, point.z);
        }

        return point;
    }
}