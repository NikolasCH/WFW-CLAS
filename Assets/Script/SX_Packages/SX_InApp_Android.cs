using System.Collections.Generic;
using SA.Android.Utilities;
using SA.Android.Vending.BillingClient;
using SA.Foundation.Network.Web;
using SA.Foundation.Templates;
using UnityEngine;
using UnityEngine.UI;
using SA.Android;
using SA.Android.Samples;

    public class SX_InApp_Android : MonoBehaviour, AN_iSkuDetailsResponseListener, AN_iConsumeResponseListener
    {
        private static SX_BillingClient_Android m_BillingClientSample = null;   
        [SerializeField] private AN_ProductView m_ProductView = null;      
        private List<AN_Purchase> m_Purchases = new List<AN_Purchase>();    
        public SX_InApp_Android InApp;

        private void Start()
        {
            if (m_BillingClientSample == null)
            {
                m_BillingClientSample = new SX_BillingClient_Android();
            }                 
    
            m_BillingClientSample.Connect();

            m_BillingClientSample.InApp = this;
           
            //In this example we will rebuild whole UI when any product purchase state is changed.
            //But you can implement more advanced login and skip QuerySkuDetailsAsync step if you already done it earlier.
            m_BillingClientSample.OnStoreStateUpdated += BuildProducts;
        }

        public void Purchase(string ProductId)
        {
            BuildProducts();
            if(m_BillingClientSample.IsConnected){
                var skuDetails = GetSkuDetails(ProductId);
                var paramsBuilder = AN_BillingFlowParams.NewBuilder();
                paramsBuilder.SetSkuDetails(skuDetails);           
                m_BillingClientSample.Client.LaunchBillingFlow(paramsBuilder.Build());
            }           
        }

        public const string ITEM1 = "wfw2_p1";	
        public const string ITEM2 = "wfw2_p2";
        public const string ITEM3 = "wfw2_p3";	
        public const string ITEM4 = "wfw2_p4";	
        public const string ITEM5 = "wfw2_p5";

        public const string PAGE_1 = "page_1";

        public const string PAGE_2 = "page_2";

        public const string PAGE_3 = "page_3";

        public const string PAGE_4 = "page_4";

        public const string PAGE_5 = "page_5";

        public const string PAGE_6 = "page_6";

        public const string PAGE_7 = "page_7";

        public void Reward(string ProductId)
        {
            //Reward your used based on ProductId
            switch (ProductId) 
            {
                case ITEM1: 
                    Main.CoinAdd(1000);
                    Consume(ProductId); 
                    Main.buy();
                    break;
                case ITEM2:
                    Main.CoinAdd(2200);
                    Consume(ProductId); 
                    Main.buy();
                    break;
                case ITEM3:
                    Main.CoinAdd(3600);
                    Consume(ProductId); 
                    Main.buy();
                    break;
                case ITEM4:
                    Main.CoinAdd(5200);
                    Consume(ProductId); 
                    Main.buy();
                    break;
                case ITEM5:
                    Main.CoinAdd(7000);
                    Consume(ProductId); 
                    Main.buy();
                    break; 
                case PAGE_1:
                    PlayerPrefs.SetInt(PAGE_1, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_2:
                    PlayerPrefs.SetInt(PAGE_2, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_3:
                    PlayerPrefs.SetInt(PAGE_3, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_4:
                    PlayerPrefs.SetInt(PAGE_4, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_5:
                    PlayerPrefs.SetInt(PAGE_5, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_6:
                    PlayerPrefs.SetInt(PAGE_6, 1);
                    Application.LoadLevelAsync(1);
                    break;
                case PAGE_7:
                    PlayerPrefs.SetInt(PAGE_7, 1);
                    Application.LoadLevelAsync(1);
                    break;         
                default:
                    Debug.LogError("Unknown product Id: " + ProductId);
                    break;
            }     
        }  

        public void Consume(string ProductId)
        {
            BuildProducts();
            var skuDetails = GetSkuDetails(ProductId);
            var productPurchasedInfo = IsProductPurchased(skuDetails);
            var paramsBuilder = AN_ConsumeParams.NewBuilder();
            paramsBuilder.SetPurchaseToken(productPurchasedInfo.PurchaseToken);           
            m_BillingClientSample.Client.ConsumeAsync(paramsBuilder.Build(), this);      
        }                 
        private void BuildProducts()
        {           
            //Clean up current UI
            m_Purchases.Clear();
   
            //Let's get all the purchases  first
            var purchasesResult = m_BillingClientSample.Client.QueryPurchases(AN_BillingClient.SkuType.inapp);
            if (purchasesResult.IsSucceeded)
            {
                m_Purchases.AddRange(purchasesResult.Purchases);
            }
                
            //In case you also have subs products you can also Query for it as well.
            //In this example we only have inapp products types.
            var paramsBuilder = AN_SkuDetailsParams.NewBuilder();
            paramsBuilder.SetType(AN_BillingClient.SkuType.inapp);
            
            var skusList = new List<string>();

            foreach (var product in AN_Settings.Instance.InAppProducts) 
            {
                skusList.Add(product.Sku);
            }

            paramsBuilder.SetSkusList(skusList);
                
            m_BillingClientSample.Client.QuerySkuDetailsAsync(paramsBuilder.Build(), this);
        }
        
        //--------------------------------------
        // AN_iSkuDetailsResponseListener
        //--------------------------------------
        
        public void OnSkuDetailsResponse(SA_Result billingResult, List<AN_SkuDetails> skuDetailsList)
        {          
            AN_Logger.Log("OnSkuDetailsResponse IsSucceeded: " + billingResult.IsSucceeded);
            if (billingResult.IsSucceeded)
            {
                AN_Logger.Log("Loaded " + skuDetailsList.Count + " products");
                foreach (var skuDetails in skuDetailsList)
                {
                    AN_Logger.Log("--------------------->");
                    PrintSku(skuDetails);
                    
                    var productView = Instantiate(m_ProductView.gameObject, m_ProductView.transform.parent).GetComponent<AN_ProductView>();
                    productView.transform.localScale = m_ProductView.transform.localScale;
                    productView.gameObject.SetActive(true);
                    productView.ProductTitle.text = skuDetails.Title;
                    if (!string.IsNullOrEmpty(skuDetails.IconUrl))
                    {
                        SA_CachedRequestsFactory.GetTexture2D(skuDetails.IconUrl, texture =>
                        {
                            productView.ProductImage.texture = texture;
                        });
                    }
                    
                    var productPurchasedInfo = IsProductPurchased(skuDetails);

                    if (productPurchasedInfo != null)
                    {
                        productView.BuyButton.GetComponentInChildren<Text>().text = "Consume";
                        productView.BuyButton.onClick.AddListener(() =>
                        {
                            var paramsBuilder = AN_ConsumeParams.NewBuilder();
                            paramsBuilder.SetPurchaseToken(productPurchasedInfo.PurchaseToken);
                        
                            m_BillingClientSample.Client.ConsumeAsync(paramsBuilder.Build(), this);
                        });  
                    }
                    else
                    {
                        productView.BuyButton.GetComponentInChildren<Text>().text = "Buy";
                        productView.BuyButton.onClick.AddListener(() =>
                        {
                            var paramsBuilder = AN_BillingFlowParams.NewBuilder();
                            paramsBuilder.SetSkuDetails(skuDetails);
                        
                            m_BillingClientSample.Client.LaunchBillingFlow(paramsBuilder.Build());
                        });  
                    } 
                }
            }
        }
       
            
        //--------------------------------------
        //  AN_iConsumeResponseListener
        //--------------------------------------
        
        public void OnConsumeResponse(SA_iResult billingResult, string purchaseToken)
        {
            if (billingResult.IsSucceeded)
            {
                //Let's updated our UI again
                BuildProducts();
            }
            else
            {
                AN_BillingClientSample.ShowErrorMessage(billingResult.Error);
            }
        }
        
        private AN_Purchase IsProductPurchased(AN_SkuDetails skuDetails)
        {
            foreach (var purchase in m_Purchases)
            {
                if (purchase.Sku.Equals(skuDetails.Sku))
                {
                    return purchase;
                }
            }
            return null;
        }

        public static AN_SkuDetails GetSkuDetails(string sku){
            foreach (var product in AN_Settings.Instance.InAppProducts)
            {
                if (product.Sku.Equals(sku))
                {
                    return product;
                }
            }
            return null;
        }

        private void PrintSku(AN_SkuDetails skuDetails) 
        {
            AN_Logger.Log("skuDetails.Sku: " + skuDetails.Sku);
            AN_Logger.Log("skuDetails.Price: " + skuDetails.Price);
            AN_Logger.Log("skuDetails.Title: " + skuDetails.Title);
            AN_Logger.Log("skuDetails.Description: " + skuDetails.Description);
            AN_Logger.Log("skuDetails.FreeTrialPeriod: " + skuDetails.FreeTrialPeriod);
            AN_Logger.Log("skuDetails.IconUrl: " + skuDetails.IconUrl);
            AN_Logger.Log("skuDetails.IntroductoryPrice: " + skuDetails.IntroductoryPrice);
            AN_Logger.Log("skuDetails.IntroductoryPriceAmountMicros: " + skuDetails.IntroductoryPriceAmountMicros);
            AN_Logger.Log("skuDetails.IntroductoryPriceCycles: " + skuDetails.IntroductoryPriceCycles);
            AN_Logger.Log("skuDetails.IntroductoryPricePeriod: " + skuDetails.IntroductoryPricePeriod);
            AN_Logger.Log("skuDetails.OriginalPrice: " + skuDetails.OriginalPrice);
            AN_Logger.Log("skuDetails.OriginalPriceAmountMicros: " + skuDetails.OriginalPriceAmountMicros);
            AN_Logger.Log("skuDetails.PriceAmountMicros: " + skuDetails.PriceAmountMicros);
            AN_Logger.Log("skuDetails.PriceCurrencyCode: " + skuDetails.PriceCurrencyCode);
            AN_Logger.Log("skuDetails.SubscriptionPeriod: " + skuDetails.SubscriptionPeriod);
            AN_Logger.Log("skuDetails.IsRewarded: " + skuDetails.IsRewarded);
            AN_Logger.Log("skuDetails.OriginalJson: " + skuDetails.OriginalJson);
        }
    }