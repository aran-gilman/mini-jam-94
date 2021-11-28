using UnityEngine;
using UnityEngine.UI;

public class TierDisplay : MonoBehaviour
{
    public Sprite lockedTier;
    public Sprite unlockedTier;

    public void SetTier(int tier)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetComponent<Image>();
            image.sprite = i < tier ? unlockedTier : lockedTier;
        }
    }
}
