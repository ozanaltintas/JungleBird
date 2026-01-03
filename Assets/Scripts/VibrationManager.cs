using UnityEngine;

public static class VibrationManager
{
    // Titreşimi kökten kapatmak istersen bunu false yapabilirsin
    public static bool hapticsEnabled = true;

    // COIN İÇİN: Çok kısa, keskin titreşim
    public static void VibrateLight()
    {
        if (!isMobile() || !hapticsEnabled) return;

#if UNITY_ANDROID
            // Android'de 40 milisaniyelik "TIK" hissi
            VibrateAndroid(40);
#elif UNITY_IOS
        // iOS'ta plugin olmadan yapabileceğimiz tek şey bu
        // Coin toplarken çok sık titreşim rahatsız ederse burayı yorum satırı yapabilirsin.
        Handheld.Vibrate();
#endif
    }

    // ÖLÜM İÇİN: Biraz daha uzun, "GÜM" hissi
    public static void VibrateHeavy()
    {
        if (!isMobile() || !hapticsEnabled) return;

#if UNITY_ANDROID
            // Android'de 200 milisaniyelik sarsıntı
            VibrateAndroid(200);
#elif UNITY_IOS
        Handheld.Vibrate();
#endif
    }

    // Android için özel sihirli kod (Reflection kullanarak native erişim)
    private static void VibrateAndroid(long milliseconds)
    {
        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            if (vibrator != null)
            {
                vibrator.Call("vibrate", milliseconds);
            }
        }
        catch
        {
            // Eski telefonlarda çalışmazsa standart titreşime düş
            Handheld.Vibrate();
        }
    }

    // Sadece mobilde çalışsın diye kontrol
    private static bool isMobile()
    {
        return Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer;
    }
}