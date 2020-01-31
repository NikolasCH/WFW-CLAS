////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using SA.iOS.StoreKit;
using SA.Foundation.Templates;
using SA.iOS.Examples;

    /// <summary>
    /// The example class that shows how to implement InApp support in your app
    /// </summary>
    public class SX_BillingClient_iOS : ISN_iSKPaymentTransactionObserver
    {
    //This could also be defined with the Plugin editor settings
    //Stan's Assets -> IOSNative -> EditSettings
    public const string ITEM1 = "wfw4_p1";
    public const string ITEM2 = "wfw4_p2";
    public const string ITEM3 = "wfw4_p3";
    public const string ITEM4 = "wfw4_p4";
    public const string ITEM5 = "wfw4_p5";

    public const string PAGE_1 = "wfw4_page_1";

    public const string PAGE_2 = "wfw4_page_2";

    public const string PAGE_3 = "wfw4_page_3";

    public const string PAGE_4 = "wfw4_page_4";

    public const string PAGE_5 = "wfw4_page_5";

    public const string PAGE_6 = "wfw4_page_6";

    public const string PAGE_7 = "wfw4_page_7";


    private static bool IsInitialized = false;

        public void init()
        {
            // just make sure we init only once
            if (!IsInitialized)
            {
                ISN_SKPaymentQueue.RegisterProductId(ITEM1);
                ISN_SKPaymentQueue.RegisterProductId(ITEM2);
                ISN_SKPaymentQueue.RegisterProductId(ITEM3);
                ISN_SKPaymentQueue.RegisterProductId(ITEM4);
                ISN_SKPaymentQueue.RegisterProductId(ITEM5);
                ISN_SKPaymentQueue.RegisterProductId(PAGE_1);
                ISN_SKPaymentQueue.RegisterProductId(PAGE_2);
                ISN_SKPaymentQueue.RegisterProductId(PAGE_3);
                ISN_SKPaymentQueue.RegisterProductId(PAGE_4);


            IsInitialized = true;


                ISN_SKPaymentQueue.Init(result => {
                    Debug.Log("result.Products.Count " + result.Products.Count);
                    Debug.Log("result.InvalidProductIdentifiers.Count " + result.InvalidProductIdentifiers.Count);

                });

                //Since current class is implement's ISN_iSKPaymentTransactionObserver
                //we can add it as the transaction observer
                ISN_SKPaymentQueue.AddTransactionObserver(this);
            }
        }


        //--------------------------------------
        //  Private Methods
        //--------------------------------------

        private static void UnlockProducts(ISN_iSKPaymentTransaction transaction)
        {

        //At this point user already paid for content, so we need to provide it
        //Unless, we want to make sure that payment was legit, and nobody trying to hack us
        //In order to do it, we have to use server side verification, you can read more about it here:
        //https://developer.apple.com/library/content/releasenotes/General/ValidateAppStoreReceipt/Chapters/ValidateRemotely.html#//apple_ref/doc/uid/TP40010573-CH104-SW1
        //
        //this step isn't required. Use it only if you want to make sure that payment is 100% legit
        //So far let's just print a Base64 receipt data

        //  Debug.Log("Receipt: " + ISN_SKPaymentQueue.AppStoreReceipt.AsBase64StringString);
        string ProductId = transaction.ProductIdentifier;
            switch (ProductId)
            {
            case ITEM1:
                Main.CoinAdd(1000);
                //Consume(ProductId);
                Main.buy();
                break;
            case ITEM2:
                Main.CoinAdd(2200);
               // Consume(ProductId);
                Main.buy();
                break;
            case ITEM3:
                Main.CoinAdd(3600);
               // Consume(ProductId);
                Main.buy();
                break;
            case ITEM4:
                Main.CoinAdd(5200);
               // Consume(ProductId);
                Main.buy();
                break;
            case ITEM5:
                Main.CoinAdd(7000);
                //Consume(ProductId);
                Main.buy();
                break;
            case PAGE_1:
                PlayerPrefs.SetInt("page_1", 1);
                Application.LoadLevelAsync(1);
                break;
            case PAGE_2:
                PlayerPrefs.SetInt("page_2", 1);
                Application.LoadLevelAsync(1);
                break;
            case PAGE_3:
                PlayerPrefs.SetInt("page_3", 1);
                Application.LoadLevelAsync(1);
                break;
            case PAGE_4:
                PlayerPrefs.SetInt("page_4", 1);
                Application.LoadLevelAsync(1);
                break;  
            case PAGE_5:
                PlayerPrefs.SetInt("page_5", 1);
                Application.LoadLevelAsync(1);
                break;
            case PAGE_6:
                PlayerPrefs.SetInt("page_6", 1);
                Application.LoadLevelAsync(1);
                break;
            case PAGE_7:
                PlayerPrefs.SetInt("page_7", 1);
                Application.LoadLevelAsync(1);
                break; 
            default:
                Debug.LogError("Unknown product Id: " + ProductId);
                break;

        }

            //After connect was provided to use we can finally finish the transaction
            ISN_SKPaymentQueue.FinishTransaction(transaction);
        }



        //--------------------------------------
        //  ISN_TransactionObserver implementation
        //--------------------------------------

        public void OnTransactionUpdated(ISN_iSKPaymentTransaction transaction)
        {

            //Transactions have been updated.
            //Let's act accordingly
            Debug.Log("transaction JSON: " + JsonUtility.ToJson(transaction));

            Debug.Log("OnTransactionComplete: " + transaction.ProductIdentifier);
            Debug.Log("OnTransactionComplete: state: " + transaction.State);

            switch (transaction.State)
            {

                case ISN_SKPaymentTransactionState.Purchasing:
                    //No actions is required here, we probably don't even have a ProductIdentifier
                    //but we can use this callback to show preloader for example, since we know that user is currently
                    //working on this transaction
                    break;

                case ISN_SKPaymentTransactionState.Purchased:
                case ISN_SKPaymentTransactionState.Restored:
                    //Our product has been successfully purchased or restored
                    //So we need to provide content to our user depends on productIdentifier
                    UnlockProducts(transaction);

                    break;
                case ISN_SKPaymentTransactionState.Deferred:
                    //iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
                    //You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
                    //reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
                    break;
                case ISN_SKPaymentTransactionState.Failed:
                    //Our purchase flow is failed.
                    //We can unlock interface and report user that the purchase is failed. 
                    Debug.Log("Transaction failed with error, code: " + transaction.Error.Code);
                    Debug.Log("Transaction failed with error, description: " + transaction.Error.Message);

                    //at this point we just need to finish the transaction
                    ISN_SKPaymentQueue.FinishTransaction(transaction);
                    break;
            }

            if (transaction.State == ISN_SKPaymentTransactionState.Failed)
            {
                Debug.Log("Error code: " + transaction.Error.Code + "\n" + "Error description:" + transaction.Error.Message);
            }
            else
            {
                Debug.Log("product " + transaction.ProductIdentifier + " state: " + transaction.State);
            }
        }

        public void OnTransactionRemoved(ISN_iSKPaymentTransaction result)
        {
            //Your application does not typically need to anything on this event,  
            //but it may be used to update user interface to reflect that a transaction has been completed.
        }

        public bool OnShouldAddStorePayment(ISN_SKProduct result)
        {
            // Return true to continue the transaction in your app.
            // Return false to defer or cancel the transaction.
            // If you return false, you can continue the transaction later using requestId <see cref="ISN_SKProduct"/>
            // 
            // we are okay, to continue transaction, so let's return true
            return true;
        }


        public void OnRestoreTransactionsComplete(SA_Result result)
        {

            // Tells the observer that the payment queue has finished sending restored transactions.
            // 
            // This method is called after all restore transactions have been processed by the payment queue. 
            // Your application is not required to do anything in this method.

            if (result.IsSucceeded)
            {
                Debug.Log("Restore Completed");
            }
            else
            {
                Debug.Log("Error: " + result.Error.Code + " message: " + result.Error.Message);
            }
        }
    }
