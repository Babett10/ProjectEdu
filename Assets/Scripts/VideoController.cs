using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer[] videoPlayers;
    public RenderTexture renderTexture;
    public Slider[] progressSliders; // 🔥 ubah jadi array

    private VideoPlayer currentPlayer;
    private Slider currentSlider;

    void Start()
    {
        // Setup semua slider jadi progress only
        foreach (var s in progressSliders)
        {
            s.minValue = 0;
            s.maxValue = 1;
            s.interactable = false;
            s.value = 0;
        }
    }

    void Update()
    {
        // Update slider yang aktif saja
        if (currentPlayer != null &&
            currentPlayer.isPlaying &&
            currentPlayer.length > 0 &&
            currentSlider != null)
        {
            currentSlider.value =
                (float)(currentPlayer.time / currentPlayer.length);
        }
    }

    // 🎬 Dipanggil saat buka panel
    public void OpenVideo(VideoPlayer selected)
    {
        // Stop semua video & reset slider
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            videoPlayers[i].Stop();
            videoPlayers[i].targetTexture = null;

            if (i < progressSliders.Length)
                progressSliders[i].value = 0;
        }

        // Reset tampilan
        ClearRenderTexture();

        // Set video aktif
        currentPlayer = selected;
        currentPlayer.targetTexture = renderTexture;

        // 🔥 cari slider yang sesuai
        int index = System.Array.IndexOf(videoPlayers, selected);
        if (index >= 0 && index < progressSliders.Length)
            currentSlider = progressSliders[index];
        else
            currentSlider = null;

        // Prepare saja (tidak langsung play)
        currentPlayer.Prepare();
    }

    // ▶️ Play / Pause
    public void PlayPause()
    {
        if (currentPlayer == null) return;
        if (!currentPlayer.isPrepared) return;

        if (currentPlayer.isPlaying)
            currentPlayer.Pause();
        else
            currentPlayer.Play();
    }

    // ❌ Tutup semua video
    public void StopAll()
    {
        foreach (var vp in videoPlayers)
        {
            vp.Stop();
            vp.targetTexture = null;
        }

        foreach (var s in progressSliders)
        {
            s.value = 0;
        }

        currentPlayer = null;
        currentSlider = null;

        ClearRenderTexture();
    }

    // 🔥 Clear RenderTexture
    void ClearRenderTexture()
    {
        if (renderTexture != null)
        {
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }
    }
}