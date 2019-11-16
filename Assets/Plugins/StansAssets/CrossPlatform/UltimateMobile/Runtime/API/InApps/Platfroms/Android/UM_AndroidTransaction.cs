using System;
using SA.Foundation.Templates;
using SA.Android.Vending.BillingClient;

namespace SA.CrossPlatform.InApp
{
    [Serializable]
    internal class UM_AndroidTransaction : UM_AbstractTransaction<AN_Purchase>, UM_iTransaction
    {
        public UM_AndroidTransaction(SA_iResult billingResult, AN_Purchase purchase) 
        {
            if(billingResult.IsSucceeded) 
            {
                SetPurchase(purchase, false);
            } 
            else 
            {
                m_state = UM_TransactionState.Failed;
                m_error = billingResult.Error;
            }
        }

        public UM_AndroidTransaction(AN_Purchase purchase, bool isRestored) 
        {
            SetPurchase(purchase, isRestored);
        }

        private void SetPurchase(AN_Purchase purchase, bool isRestored) {
            m_id = purchase.OrderId;
            m_productId = purchase.Sku;
            m_unixTimestamp = purchase.PurchaseTime;
            m_state = isRestored ? UM_TransactionState.Restored : UM_TransactionState.Purchased;
            SetNativeTransaction(purchase);
        }

        public AN_Purchase Purchase
        {
            get { return (AN_Purchase) NativeTemplate; }
        }
    }
}