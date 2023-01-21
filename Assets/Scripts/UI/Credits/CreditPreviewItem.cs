using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditPreviewItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI artist;
    [SerializeField] TextMeshProUGUI art;
    [SerializeField] TextMeshProUGUI link;

    public void SetContent(string strArtist, string strArt, string strLink)
    {
        artist.text = strArtist;
        art.text = strArt;
        link.text = strLink;
    }
}
