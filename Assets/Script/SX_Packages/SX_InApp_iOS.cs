////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using SA.iOS.StoreKit;
using SA.iOS.Examples;


    public class SX_InApp_iOS : MonoBehaviour 
    {

        private static SX_BillingClient_iOS s_paymentManager;


        private void Start()
        {


        if (s_paymentManager == null)
            {
                s_paymentManager = new SX_BillingClient_iOS();
            }


            s_paymentManager.init();
        }


        public void Purchase(string ProductId)
        {

            if (ProductId == "page_1")
                ProductId = "wfw4_page_1";
            if (ProductId == "page_2")
                ProductId = "wfw4_page_2";
            if (ProductId == "page_3")
                ProductId = "wfw4_page_3";
            if (ProductId == "page_4") 
                ProductId = "wfw4_page_4";                         
            if (ProductId == "page_5")
                ProductId = "wfw4_page_5";
            if (ProductId == "page_6")
                ProductId = "wfw4_page_6";
            if (ProductId == "page_7") 
                ProductId = "wfw4_page_7";


            if (ProductId == "wfw2_p1")
                ProductId = "wfw4_p1";
            if (ProductId == "wfw2_p2")
                ProductId = "wfw4_p2";
            if (ProductId == "wfw2_p3")
                ProductId = "wfw4_p3";
            if (ProductId == "wfw2_p4")
                ProductId = "wfw4_p4";
            if (ProductId == "wfw2_p5")
                ProductId = "wfw4_p5";

        ISN_SKPaymentQueue.AddPayment(ProductId);
        }

    }
