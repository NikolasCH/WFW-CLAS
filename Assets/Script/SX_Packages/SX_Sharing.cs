using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.CrossPlatform.Social;
using SA.Foundation.Templates;
using SA.Foundation.Utility;
using SA.Android.Firebase.Analytics;

public class SX_Sharing : MonoBehaviour
{

    // Start is called before the first frame update
    public void Sharing()
    {
        var client = UM_SocialService.SharingClient;
            client.SystemSharingDialog(MakeSharingBuilder(), PrintSharingResult);
    // Start is called before the first f
    }
    private UM_ShareDialogBuilder MakeSharingBuilder()
    {
        var builder = new UM_ShareDialogBuilder();
        builder.SetText("Помоги, собрать загаданные слова!");
        builder.SetUrl("https://play.google.com/store/apps/details?id=" + Application.identifier);

        //Juts generating simple red texture with 32x32 resolution
        var sampleRedTexture = SA_IconManager.GetIcon(Color.red, 32, 32);
        builder.AddImage(screenShot());

        return builder;
    }

	Texture2D screenShot() {
            GameObject SX = GameObject.Find ("SX");
            SX.GetComponent<SX_Ads> ().smartBanneHide ();
            Camera Camera = GameObject.Find ("Camera").GetComponent<Camera>();
			RenderTexture rt = new RenderTexture(Camera.pixelWidth, Camera.pixelHeight, 24);
			Camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(Camera.pixelWidth, Camera.pixelHeight, TextureFormat.RGB24, false);
			Camera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, Camera.pixelWidth, Camera.pixelHeight), 0, 0);
            Camera.targetTexture = null;
            SX.GetComponent<SX_Ads> ().smartBanneShow ();
			return screenShot;
	}
    
    public static void PrintSharingResult(SA_Result result)
    {
        if(result.IsSucceeded) {
            AN_FirebaseAnalytics.LogEvent("Sharing_Completed");
            Debug.Log("Sharing Completed.");
        } else {
            Debug.Log("Failed to share: " + result.Error.FullMessage);
        }
    }
}
