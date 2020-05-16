using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracoesML.Entity
{
    public class PedidoOld
    {
        public string orderId { get; set; }
        public string sequence { get; set; }
        public string marketplaceOrderId { get; set; }
        public string marketplaceServicesEndpoint { get; set; }
        public string sellerOrderId { get; set; }
        public string origin { get; set; }
        public string affiliateId { get; set; }
        public string salesChannel { get; set; }
        public object merchantName { get; set; }
        public string status { get; set; }
        public string statusDescription { get; set; }
        public double value { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastChange { get; set; }
        public string orderGroup { get; set; }
        public Total[] totals { get; set; }
        public ItemVtex[] items { get; set; }
        //public object[] marketplaceItems { get; set; }
        public Clientprofiledata clientProfileData { get; set; }
        //public object giftRegistryData { get; set; }
        //public object marketingData { get; set; }
        //public Ratesandbenefitsdata ratesAndBenefitsData { get; set; }
        public Shippingdata shippingData { get; set; }
        public Paymentdata paymentData { get; set; }
        //public Packageattachment packageAttachment { get; set; }
        //public Seller[] sellers { get; set; }
        //public object callCenterOperatorData { get; set; }
        public string followUpEmail { get; set; }
        //public object lastMessage { get; set; }
        public string hostname { get; set; }
        //public object invoiceData { get; set; }
        //public object changesAttachment { get; set; }
        //public object openTextField { get; set; }
        public int roundingError { get; set; }
        public string orderFormId { get; set; }
        //public object commercialConditionData { get; set; }
        public bool isCompleted { get; set; }
        //public object customData { get; set; }
        public Storepreferencesdata storePreferencesData { get; set; }
        public bool allowCancellation { get; set; }
        public bool allowEdition { get; set; }
        public bool isCheckedIn { get; set; }
        public Marketplace marketplace { get; set; }
        public string authorizedDate { get; set; }
        //public object invoicedDate { get; set; }
        //public object cancelReason { get; set; }
        public Itemmetadata itemMetadata { get; set; }
    }

    public class Clientprofiledata
    {
        public string id { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string documentType { get; set; }
        public string document { get; set; }
        public string phone { get; set; }
        //public object corporateName { get; set; }
        //public object tradeName { get; set; }
        public string corporateDocument { get; set; }
        public string stateInscription { get; set; }
        //public object corporatePhone { get; set; }
        public bool isCorporate { get; set; }
        public string userProfileId { get; set; }
        //public object customerClass { get; set; }
    }

    public class Ratesandbenefitsdata
    {
        public string id { get; set; }
        public object[] rateAndBenefitsIdentifiers { get; set; }
    }

    public class Shippingdata
    {
        public string id { get; set; }
        public Address address { get; set; }
        public Logisticsinfo[] logisticsInfo { get; set; }
        //public object trackingHints { get; set; }
        public Selectedaddress[] selectedAddresses { get; set; }
    }

    public class Address
    {
        public string addressType { get; set; }
        public string receiverName { get; set; }
        public string addressId { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string street { get; set; }
        public string number { get; set; }

        public string complement { get; set; }
        public string neighborhood { get; set; }
        //public string complement { get; set; }
        //public object reference { get; set; }
        public float[] geoCoordinates { get; set; }
    }

    public class Logisticsinfo
    {
        public int itemIndex { get; set; }
        public string selectedSla { get; set; }
        public string lockTTL { get; set; }
        public double price { get; set; }
        public double listPrice { get; set; }
        public double sellingPrice { get; set; }
        //public object deliveryWindow { get; set; }
        public string deliveryCompany { get; set; }
        public string shippingEstimate { get; set; }
        public string shippingEstimateDate { get; set; }
        public Sla[] slas { get; set; }
        public string[] shipsTo { get; set; }
        public Deliveryid[] deliveryIds { get; set; }
        public string deliveryChannel { get; set; }
        //public Pickupstoreinfo pickupStoreInfo { get; set; }
        public string addressId { get; set; }
        //public object polygonName { get; set; }
    }

    public class Pickupstoreinfo
    {
        public object additionalInfo { get; set; }
        public object address { get; set; }
        public object dockId { get; set; }
        public object friendlyName { get; set; }
        public bool isPickupStore { get; set; }
    }

    public class Sla
    {
        public string id { get; set; }
        public string name { get; set; }
        public string shippingEstimate { get; set; }
        //public object deliveryWindow { get; set; }
        public int price { get; set; }
        public string deliveryChannel { get; set; }
        //public Pickupstoreinfo1 pickupStoreInfo { get; set; }
        //public object polygonName { get; set; }
        public string lockTTL { get; set; }
    }

    public class Pickupstoreinfo1
    {
        public object additionalInfo { get; set; }
        public object address { get; set; }
        public object dockId { get; set; }
        public object friendlyName { get; set; }
        public bool isPickupStore { get; set; }
    }

    public class Deliveryid
    {
        public string courierId { get; set; }
        public string courierName { get; set; }
        public string dockId { get; set; }
        public int quantity { get; set; }
        public string warehouseId { get; set; }
    }

    public class Selectedaddress
    {
        public string addressId { get; set; }
        public string addressType { get; set; }
        public string receiverName { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        //public object reference { get; set; }
        public float[] geoCoordinates { get; set; }
    }

    public class Paymentdata
    {
        public Transaction[] transactions { get; set; }
    }

    public class Transaction
    {
        public bool isActive { get; set; }
        public string transactionId { get; set; }
        public string merchantName { get; set; }
        public Payment[] payments { get; set; }
    }

    public class PaymentOld
    {
        public string id { get; set; }
        public string paymentSystem { get; set; }
        public string paymentSystemName { get; set; }
        public int value { get; set; }
        public int installments { get; set; }
        public int referenceValue { get; set; }
        //public object cardHolder { get; set; }
        //public object cardNumber { get; set; }
        public string firstDigits { get; set; }
        public string lastDigits { get; set; }
        /*public object cvv2 { get; set; }
        public object expireMonth { get; set; }
        public object expireYear { get; set; }
        public object url { get; set; }
        public object giftCardId { get; set; }
        public object giftCardName { get; set; }
        public object giftCardCaption { get; set; }
        public object redemptionCode { get; set; }*/
        public string group { get; set; }
        public string tid { get; set; }
        //public object dueDate { get; set; }
        public Connectorresponses connectorResponses { get; set; }
    }

    public class Connectorresponses
    {
        public string message { get; set; }
        public string returnCode { get; set; }
        public string tid { get; set; }
        public string authId { get; set; }
        public string nsu { get; set; }
        public string network { get; set; }
        public string firstDigits { get; set; }
        public string lastDigits { get; set; }
    }

    public class Packageattachment
    {
        public object[] packages { get; set; }
    }

    public class Storepreferencesdata
    {
        public string countryCode { get; set; }
        public string currencyCode { get; set; }
        public Currencyformatinfo currencyFormatInfo { get; set; }
        public int currencyLocale { get; set; }
        public string currencySymbol { get; set; }
        public string timeZone { get; set; }
    }

    public class Currencyformatinfo
    {
        public int CurrencydoubleDigits { get; set; }
        public string CurrencydoubleSeparator { get; set; }
        public string CurrencyGroupSeparator { get; set; }
        public int CurrencyGroupSize { get; set; }
        public bool StartsWithCurrencySymbol { get; set; }
    }

    public class Marketplace
    {
        public string baseURL { get; set; }
        //public object isCertified { get; set; }
        public string name { get; set; }
    }

    public class Itemmetadata
    {
        public ItemData[] Items { get; set; }
    }

    public class ItemData
    {
        public string Id { get; set; }
        public string Seller { get; set; }
        public string Name { get; set; }
        public string SkuName { get; set; }
        public string ProductId { get; set; }
        public string RefId { get; set; }
        public string Ean { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; }
        //public object[] AssemblyOptions { get; set; }
    }

    public class Total
    {
        public string id { get; set; }
        public string name { get; set; }
        public int value { get; set; }
    }

    public class ItemVtex
    {
        public string uniqueId { get; set; }
        public string id { get; set; }
        public string productId { get; set; }
        public string ean { get; set; }
        public string lockId { get; set; }
        public Itemattachment itemAttachment { get; set; }
        //public object[] attachments { get; set; }
        public int quantity { get; set; }
        public string seller { get; set; }
        public string name { get; set; }
        public string refId { get; set; }
        public double price { get; set; }
        public double listPrice { get; set; }
        //public object manualPrice { get; set; }
        //public object[] priceTags { get; set; }
        public string imageUrl { get; set; }
        public string detailUrl { get; set; }
        //public object[] components { get; set; }
        //public object[] bundleItems { get; set; }
        //public object[] _params { get; set; }
        //public object[] offerings { get; set; }
        public string sellerSku { get; set; }
        //public object priceValidUntil { get; set; }
        public int commission { get; set; }
        public int tax { get; set; }
        //public object preSaleDate { get; set; }
        public Additionalinfo additionalInfo { get; set; }
        public string measurementUnit { get; set; }
        public double unitMultiplier { get; set; }
        public int sellingPrice { get; set; }
        public bool isGift { get; set; }
        //public object shippingPrice { get; set; }
        public int rewardValue { get; set; }
        public int freightCommission { get; set; }
        //public object priceDefinitions { get; set; }
        public string taxCode { get; set; }
        /*public object parentItemIndex { get; set; }
        public object parentAssemblyBinding { get; set; }
        public object callCenterOperator { get; set; }
        public object serialNumbers { get; set; }*/
    }

    public class Itemattachment
    {
        public Content content { get; set; }
        public object name { get; set; }
    }

    public class Content
    {
    }

    public class Additionalinfo
    {
        public string brandName { get; set; }
        public string brandId { get; set; }
        public string categoriesIds { get; set; }
        public string productClusterId { get; set; }
        public string commercialConditionId { get; set; }
        public Dimension dimension { get; set; }
        public object offeringInfo { get; set; }
        public object offeringType { get; set; }
        public object offeringTypeId { get; set; }
    }

    public class Dimension
    {
        public double cubicweight { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public double weight { get; set; }
        public double width { get; set; }
    }

    public class SellerOld
    {
        public string id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
    }


}
