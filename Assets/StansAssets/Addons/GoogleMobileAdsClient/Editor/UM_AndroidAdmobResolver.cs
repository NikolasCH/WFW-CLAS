using SA.Android;
using SA.Android.Manifest;
using SA.CrossPlatform.Advertisement;
using UnityEditor;
using UnityEngine;

public class UM_AndroidAdmobResolver : AN_APIResolver
{
    [InitializeOnLoadMethod]
    private static void ResolverRegistration()
    {
        AN_Preprocessor.RegisterResolver(new UM_AndroidAdmobResolver());
    }
    
    public override bool IsSettingsEnabled
    {
        get { return true; }
        set { }
    }
    public override void AppendBuildRequirements(AN_AndroidBuildRequirements buildRequirements)
    {
        var androidAppId = UM_GoogleAdsSettings.Instance.AndroidIds.AppId;
        if (string.IsNullOrEmpty(androidAppId)) return;
        
        var applicationId = new AMM_PropertyTemplate("meta-data");
        applicationId.SetValue("android:name", "com.google.android.gms.ads.APPLICATION_ID");
        applicationId.SetValue("android:value", androidAppId);
        buildRequirements.AddApplicationProperty(applicationId);
    }
}
