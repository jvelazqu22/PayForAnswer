namespace Domain.Models.Helper
{
    public class PayPalModel
    {
        public string cmd { get; set; }
        public string business { get; set; }
        public string no_shipping { get; set; }
        public string @return { get; set; }
        public string cancel_return { get; set; }
        public string notify_url { get; set; }
        public string current_code { get; set; }
        public string item_name { get; set; }
        public string amount { get; set; }
        public string SummaryDetails { get; set; }
        public string custom { get; set; }
        public int RequestStatus { get; set; }
        public PayPalResponse PayPalResponse { get; set; }
    }
}
