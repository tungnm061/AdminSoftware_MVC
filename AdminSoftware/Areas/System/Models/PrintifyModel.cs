using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuyAmazone.Areas.Printify.Models
{
    public class ShopPrintify
    {
        public long id { get; set; }
        public string title { get; set; }
        public string sales_channel { get; set; }

    }

    public class OrderPrintify
    {
        public string current_page { get; set; }
        public List<DataOrder> data { get; set; }
        public string first_page_url { get; set; }
        public string from { get; set; }
        public string last_page { get; set; }
        public string last_page_url { get; set; }
        public string next_page_url { get; set; }
        public string path { get; set; }
        public string per_page { get; set; }
        public string prev_page_url { get; set; }
        public string to { get; set; }
        public string total { get; set; }


    }

    public class DataOrder
    {
        public string id { get; set; }
        public AddressOrder address_to { get; set; }
        public List<LineItemOrder> line_items { get; set; }
        public MetaDataOrder metadata { get; set; }
        public string total_price { get; set; }
        public string total_shipping { get; set; }
        public string total_tax { get; set; }
        public string status { get; set; }
        public string shipping_method { get; set; }
        public DateTime created_at { get; set; }
        public DateTime sent_to_production_at { get; set; }

    }

    public class AddressOrder
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
    }

    public class LineItemOrder
    {
        public MetaDataOrder metadata { get; set; }

        public string quantity { get; set; }

        public string product_id { get; set; }

        public string variant_id { get; set; }

        public string print_provider_id { get; set; }

        public string shipping_cost { get; set; }

        public string cost { get; set; }
        public string status { get; set; }

        public DateTime sent_to_production_at { get; set; }
    }

    public class MetaDataOrder
    {
        public string order_type { get; set; }
        public DateTime shop_fulfilled_at { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string variant_label { get; set; }
        public string sku { get; set; }
        public string country { get; set; }

    }
}